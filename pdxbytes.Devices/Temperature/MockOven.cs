using System;
using Microsoft.SPOT;

namespace pdxbytes.Devices.Temperature
{
    public class MockOven : IOven
    {
        public MockOven(double minTemp, double maxTemp)
        {
            this.MaxTemperature = maxTemp;
            this.MinTemperature = minTemp;
        }
        private const double TempGain = .385;
        private void Tick(object state)
        {
            _ticking = true;
            if (this.IsOn)
            {
                if (this._temperature + TempGain < this.MaxTemperature)
                    this._temperature += TempGain;
                else if (this._temperature != this.MaxTemperature)
                    this._temperature = this.MaxTemperature;
            }
            else
            {
                if (this._temperature - TempGain / 3 > this.MinTemperature)
                    this._temperature -= TempGain / 3;
                else if (this._temperature != this.MinTemperature)
                    this._temperature = this.MinTemperature;
            }
        }
        private bool _ticking;
        private System.Threading.Timer Timer;
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public void TurnElementOn()
        {
            if (!_ticking)
                this.Timer = new System.Threading.Timer(Tick, null, 250, 250);
            if (!this.IsOn)
            {
                this.IsOn = true;
                Debug.Print("Turning element on");
            }
        }
        public void TurnElementOff()
        {
            if (this.IsOn)
            {
                this.IsOn = false;
                Debug.Print("Turning element off");
            }
        }

        public double Temperature { get { return _temperature; } }
        private double _temperature;


        #region internal

        private bool IsOn { get; set; }

        #endregion
    }
}
