using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace pdxbytes.Devices.Core
{
    public class CapacitancePort
    {
        public CapacitancePort(Cpu.PWMChannel port, bool initializeOn = true)
        {
            if (port != Cpu.PWMChannel.PWM_NONE)
                this.Port = new PWM(port, 200, .5, false);
            if (initializeOn)
                this.On();
        }
        private PWM Port { get; set; }

        public void Off()
        {
            if (null != this.Port)
                this.Port.Stop();
        }

        public void On()
        {
            if (null != this.Port)
                this.Port.Start();
        }
    }
}
