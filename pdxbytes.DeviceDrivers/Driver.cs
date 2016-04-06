using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;

namespace pdxbytes.DeviceDrivers
{
    public abstract class Driver
    {
        public Driver() { }

        public abstract void Configure(IApp app);
    }
}
