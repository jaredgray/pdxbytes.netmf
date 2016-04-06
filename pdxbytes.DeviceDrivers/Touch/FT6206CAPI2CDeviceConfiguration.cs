using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;
using pdxbytes.DeviceInterfaces.Configuration;
using Microsoft.SPOT.Hardware;
using pdxbytes.Devices.Display;

namespace pdxbytes.DeviceDrivers.Touch
{
    public class FT6206CAPI2CDeviceConfiguration : IDeviceConfiguration
    {
        public FT6206CAPI2CDeviceConfiguration(Cpu.Pin irq, Cpu.PWMChannel capacitance = Cpu.PWMChannel.PWM_NONE)
        {
            this.IRQ = irq;
            this.Cap = capacitance;
        }
        private Cpu.Pin IRQ { get; set; }
        public Cpu.PWMChannel Cap { get; set; }
        public IDevice CreateDevice()
        {
            return new FT6206CAPI2C(IRQ, Cap);
        }
    }
}
