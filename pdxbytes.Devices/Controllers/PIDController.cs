using System;
using Microsoft.SPOT;
using System.Collections;

namespace pdxbytes.Devices.Controllers
{
    public class PIDController
    {
        public PIDController(PIDContext context, PIDConfiguration configuration)
        {
            this.Context = context;
            this.Configuration = configuration;
            this.DebugInfo = new ArrayList();
            //this.Context.CurrentTime = this.Context.LastUpdateTime = DateTime.Now;
        }

        public int Passes { get; set; }

        public PIDContext Context { get; set; }

        public PIDConfiguration Configuration { get; set; }

        public ArrayList DebugInfo { get; set; }

        /// <summary>
        /// Sets the current process variable where the controller will decide if a change needs to be made
        /// </summary>
        /// <param name="pv"></param>
        public PIDCalculation SetCurrentValue(double pv)
        {
            this.Context.CurrentValue = pv;
            var calc = this.CalculatePID();
            this.Context.LastValue = this.Context.CurrentValue;
            this.DebugInfo.Add(calc.CurrentError.ToString() + "\t" + calc.Result.ToString());
            // the error tells us if we've hit our setpoint are under it or have overshot
            if (calc.CurrentError <= 0)
                this.OnTargetReached();
            else
            {
                // notify the listeners that a change is needed
            }
            return calc;
        }

        #region Internal processing  

        /// <summary>
        /// performs the calculation of the pid based on the present value
        /// </summary>
        /// <param name="pv">present value (or process variable) of the sampled control</param>
        /// <returns></returns>
        protected PIDCalculation CalculatePID()
        {
            // if over set incremental status to Decellerating
            // if under set incremental status to Accellerating
            this.OnCalculationBegin();
            var error = this.Context.SetPoint - this.Context.CurrentValue;

            //if (error <= 0)
            //    return new PIDCalculation() { CurrentError = error };

            //var tdeltatime = this.Context.CurrentTime.Subtract(this.Context.LastUpdateTime);
            var result = this.CalculateProportionalTerm(error) + this.CalculateIntegralTerm(error, this.Context.UpdateFrequency) + this.CalculateDerivativeTerm(error, this.Context.UpdateFrequency);
            ++this.Passes;
            this.Context.LastError = error;
            this.OnCalculationComplete();
            return new PIDCalculation() { CurrentError = error, Result = result };
        }

        /// <summary>
        /// performs any preliminary adjustments needed before running calculations
        /// </summary>
        protected void OnCalculationBegin()
        {
            var dt = DateTime.Now;
            this.Context.CurrentTime = dt;
        }

        /// <summary>
        /// performs any post adjustments needed after running calculations
        /// </summary>
        protected void OnCalculationComplete()
        {
            this.Context.LastUpdateTime = this.Context.CurrentTime;
        }

        /// <summary>
        /// calculates the proportional term Pout = Kp e(t)
        /// </summary>
        /// <param name="error">the current error in the pid</param>
        /// <returns></returns>
        protected double CalculateProportionalTerm(double error)
        {
            return this.Configuration.ProportionalGain * error;
        }

        /// <summary>
        /// calculates the integral term Iout = Ki S{t-o} e(r) dr
        /// </summary>
        /// <param name="error">the current error in the pid</param>
        /// <param name="tdelta">the time delta</param>
        /// <returns></returns>
        protected double CalculateIntegralTerm(double error, double tdelta)
        {
            this.Context.CompiledError = (0.9 * this.Context.CompiledError) + error;
            return this.Configuration.IntegralGain * this.Context.CompiledError;
        }

        /// <summary>
        /// calculates the derivative term Dout = Kd d/dt e(t)
        /// </summary>
        /// <param name="error">the current error in the pid</param>
        /// <param name="tdelta">the time delta</param>
        /// <returns></returns>
        protected double CalculateDerivativeTerm(double error, double tdelta)
        {
            if (this.Passes == 0)
                return 0;
            //Kd * ((error - PrevError) / deltaTime);
            //return this.Configuration.DerivativeGain * ((error - this.Context.LastError) / tdelta);
            if (tdelta == 0)
                return 0;
            return this.Configuration.DerivativeGain * ((this.Context.CurrentValue - this.Context.LastValue) / this.Context.UpdateFrequency);
        }

        /// <summary>
        /// Fires the target reached event to any listeners
        /// </summary>
        protected void OnTargetReached()
        {
            var handler = this.TargetReached;
            if (null != handler)
                handler(this.Context);
        }

        #endregion

        /// <summary>
        /// the signature that handles the complete event of the pid control loop
        /// </summary>
        /// <param name="context">current state of the pid loop</param>
        public delegate void TargetReachedHandler(PIDContext context);
        /// <summary>
        /// fired when the control has hit the desired target
        /// </summary>
        public event TargetReachedHandler TargetReached;
    }
    public class PIDCalculation
    {
        public double CurrentError { get; set; }
        public double Result { get; set; }
    }
    public class PIDConfiguration
    {
        public double ProportionalGain { get; set; }
        public double IntegralGain { get; set; }
        public double DerivativeGain { get; set; }
    }
    public class PIDContext
    {
        public double UpdateFrequency { get; set; }
        internal DateTime LastUpdateTime { get; set; }
        internal DateTime CurrentTime { get; set; }
        internal double CompiledError { get; set; }
        internal double LastError { get; set; }
        public double SetPoint { get; set; }
        public double LastValue { get; set; }
        public double CurrentValue { get; internal set; }
        public bool AchievedSetPoint { get; internal set; }
        public PIDIncrementalStatus IncrementalStatus { get; internal set; }

    }
    public enum PIDIncrementalStatus
    {
        Accellerating = 1,
        Decellerating = 2
    }
}
