
using System;

namespace pdxbytes.DeviceInterfaces.Configuration
{
    public abstract class DeviceConfiguration : IDeviceConfiguration
    {
        public abstract IDevice CreateDevice();
        
    }
}
