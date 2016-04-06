using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pdxbytes.Devices.Controllers;
using System.Threading;
using Microsoft.SPOT;
using pdxbytes.Devices.Temperature;

namespace pdxbytes.Devices.Tests
{
    [TestClass]
    public class PIDControllerTest
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var oven = new MockOven(15, 300);
        //    var context = new PIDContext()
        //    {
        //        SetPoint = 100,
        //        UpdateFrequency = 1.0 / 4
        //    };
        //    var config = new PIDConfiguration()
        //    {
        //        ProportionalGain = 40,
        //        IntegralGain = 20,
        //        DerivativeGain = 10
        //    };
        //    var pid = new PIDController(context, config);
        //    pid.TargetReached += Pid_TargetReached;
        //    System.Collections.ArrayList rows = new System.Collections.ArrayList();
          
        //    for (int i = 0; i < context.SetPoint; ++i)
        //    {
        //        rows.Add("|");
        //    }
        //    var setpoint = (int)context.SetPoint - 1;
        //    oven.TurnElementOn();
        //    while (Reading)
        //    {
        //        Thread.Sleep(250);
        //        var calc = pid.SetCurrentValue(oven.Temperature);
        //        if (calc.Result < 103900)
        //            oven.TurnElementOff();
        //        else
        //            oven.TurnElementOn();
        //        var current = (int)pid.Context.CurrentValue;
        //        rows[setpoint] = rows[setpoint].ToString() + "_";
        //        for (int i = 0; i < setpoint + 1; ++i)
        //        {
        //            if (current == i)
        //            {
        //                rows[setpoint - i] = rows[setpoint - i].ToString() + ".";
        //            }
        //            else
        //                rows[setpoint - i] = rows[setpoint - i].ToString() + " ";
        //        }
        //    }
        //    foreach (var line in pid.DebugInfo)
        //        Debug.Print(line.ToString());
        //    foreach (var line in rows)
        //    {
        //        Debug.Print(line.ToString());
        //    }
        //}
        private void Pid_TargetReached(PIDContext context)
        {
        }

        [TestMethod]
        public void TestSimulation()
        {
            this.Simulate(200);
        }
        private void Simulate(double duration)
        {
            var oven = new MockOven(15, 300);
            var context = new PIDContext()
            {
                SetPoint = 100,
                UpdateFrequency = 1.0 / 4
            };
            var config = new PIDConfiguration()
            {
                ProportionalGain = 40,
                IntegralGain = 20,
                DerivativeGain = 10
            };
            var pid = new PIDController(context, config);
            pid.TargetReached += Pid_TargetReached;
            var sim = new SimulationParameters()
            {
                ConstantHeaterWatts = 1150,
                OvenWidth = .356,
                OvenDepth = .254,
                OvenHeight = .203,
                ConstantOutsideAirTemperature = 15,
                DissapationAirToPcb = 2,
                DissapationAirToOven = 1000,
                DissapationElementToAir = 600,
                DissapationOvenToAir = 1000,
                VolatileAirTemperature = 15,
                VolatileOvenContainerTemperature = 15,
                VolitilePcbTemperature = 15
            };

            for (var t = 0.0; t < duration; t += context.UpdateFrequency)
            {
                var calc = pid.SetCurrentValue(sim.VolatileAirTemperature);
                this.SimulateTime(context.UpdateFrequency, calc.Result, sim);

                double wt = sim.VolatileAirTemperature < -100 ? -100 : sim.VolatileAirTemperature > 1000 ? 1000 : sim.VolatileAirTemperature;
            }
        }

        private void SimulateTime(double updateFrequency, double percentHeat, SimulationParameters sim)
        {
            percentHeat = percentHeat < 0 ? 0 : percentHeat > 100 ? 100 : percentHeat;

            double heaterAdded = percentHeat * sim.ConstantHeaterWatts * sim.ConstantOvenHeatFactor;
            double effectiveAirOven = sim.DissapationAirToOven * (sim.OvenDepth * sim.OvenHeight * sim.OvenWidth);

            /*

            The density of air is about 1.24 kg/m3 [m3 = cubed] - I'm assuming this is at sea level at some reasonable temperature
            
            ** Total air-mass of volume **
                m == 1.24 * V3 == n kg
                this means:
                mass (m) == (the density of air or 1.24kg) * (a unit or cubic volume) == number of kg
            
            ** Internal energy change will be **
                ΔE = mCp ΔT
                where 
                    - ΔE is the internal energy of the source
                    - Cp is the heat capacity of air (0.239 kcal/°C)
                    - ΔT is the result of the transfer of energy 
                resulting in
                    ΔT == (ΔE)/(mCp) == (n kcal*t)/(volumetric air mass in kg)*(air heat capacity) == rate of energy transfer to air/t
                    where
                        - t = some unit of time
            
            in practice for one iteration of this method would be something like this:
                - kcal will be our heat from the element    == percentHeat * sim.ConstantHeaterWatts * sim.ConstantOvenHeatFactor
                - volumetric air mass                       == sim.OvenDepth * sim.OvenHeight * sim.OvenWidth
                - air heat capacity                         == 0.239
            */

            // each unit is in meters - this gives us the 
            double kcal = (percentHeat * sim.ConstantHeaterWatts * updateFrequency) / sim.ConstantOvenHeatFactor;
            double volumetricAirMass = (sim.OvenDepth * sim.OvenHeight * sim.OvenWidth) * 1.24;


            double heatAddedToAir = kcal / (volumetricAirMass * 0.239);
            heatAddedToAir = heatAddedToAir < 0 ? 0 : heatAddedToAir;

            //double heatAddedToAir = (heaterAdded - sim.VolatileAirTemperature) * (1 - System.Math.Exp(-updateFrequency / sim.DissapationElementToAir)) / sim.DissapationElementToAir;
            //heatAddedToAir = heatAddedToAir < 0 ? 0 : heatAddedToAir;

            /*
                ok, so we got some additive heat 
                Heat dissapation model
                - element to air
                - air temp lost to sensor
                - air temp lost to pcb (this could include the rack too)
                - air temp lost to oven shell
                - oven shell temp lost to outside environment

            */

            // heat lost from the oven to the outside environment
            double heatDissapationFromOvenAirToPcb = (sim.VolatileAirTemperature - sim.VolitilePcbTemperature) * (1 - System.Math.Exp(-updateFrequency / sim.DissapationAirToPcb)) / sim.DissapationAirToPcb;

            double heatDissapationFromOvenToOutside = (sim.VolatileOvenContainerTemperature - sim.ConstantOutsideAirTemperature) * (1 - System.Math.Exp(-updateFrequency / sim.DissapationOvenToAir)) / sim.DissapationOvenToAir;

            double heatDissapationFromVolatileAirToOven = (sim.VolatileAirTemperature - sim.VolatileOvenContainerTemperature) * (1 - System.Math.Exp(-updateFrequency / effectiveAirOven)) / effectiveAirOven;

            //double heatDissapationFromOvenAirToSensor = (sim.VolatileOvenContainerTemperature - sim.ConstantOutsideAirTemperature) * (1 - System.Math.Exp(-dt / sim.DissapationAirToPcb)) / sim.DissapationAirToPcb;
            sim.VolitilePcbTemperature = sim.VolitilePcbTemperature + heatDissapationFromOvenAirToPcb;
            //food = food + heatToFood;
            sim.VolatileAirTemperature = sim.VolatileAirTemperature + heatAddedToAir - heatDissapationFromOvenAirToPcb - heatDissapationFromVolatileAirToOven;
            //water = water + heatAddedToWater - heatLostWaterContainer - heatLostWaterAir - heatLostToSensor - heatToFood;

            //sensor = sensor + heatLostToSensor;
            sim.VolatileOvenContainerTemperature = sim.VolatileOvenContainerTemperature + heatDissapationFromVolatileAirToOven - heatDissapationFromOvenToOutside;
            //container = container + heatLostWaterContainer - heatLostContainerAir;
        }
    }

    public class SimulationParameters
    {
        public double ConstantHeaterWatts { get; set; }
        public double ConstantOvenHeatFactor { get { return 10000000; } }

        public double DissapationElementToAir { get; set; }

        public double DissapationAirToOven { get; set; }

        public double DissapationAirToPcb { get; set; }

        /// <summary>
        /// the amount of dissipated heat to the outside world from the oven
        /// </summary>
        public double DissapationOvenToAir { get; set; }

        public double OvenWidth { get; set; }
        public double OvenHeight { get; set; }
        public double OvenDepth { get; set; }

        public double ConstantOutsideAirTemperature { get; set; }
        public double VolatileOvenContainerTemperature { get; set; }
        public double VolitilePcbTemperature { get; set; }
        public double VolatileAirTemperature { get; set; }
    }
}
