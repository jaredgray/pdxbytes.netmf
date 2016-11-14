using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces.Configuration;

namespace pdxbytes.DeviceInterfaces
{
    public interface IApp
    {
        void Run();
        void AddDeviceConfiguration(IDeviceConfiguration config);
        void OnLoad();
        void OnStartup();
        void OnShutDown();
    }
}
