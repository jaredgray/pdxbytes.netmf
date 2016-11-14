using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;
using pdxbytes.DeviceInterfaces.Configuration;
using pdxbytes.Devices.Core;
using pdxbytes.Devices.GPS;
using static Microsoft.SPOT.Hardware.Cpu;

namespace pdxbytes.DeviceDrivers.GPS
{
    public class FGPMMOPA6HConfiguration : IDeviceConfiguration
    {
        public FGPMMOPA6HConfiguration(SerialPortConnection connectionInfo, Pin fixPin)
        {
            this.connectionInfo = connectionInfo;
            this.FixPin = FixPin;
        }
        private SerialPortConnection connectionInfo;
        public Pin FixPin { get; set; }
        private FGPMMOPA6H device;
        public IDevice CreateDevice()
        {
            device = new FGPMMOPA6H(this.connectionInfo, FixPin);

            return device;
        }
    }
}
