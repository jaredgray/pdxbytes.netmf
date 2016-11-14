using System;
using Microsoft.SPOT;

namespace pdxbytes.DeviceInterfaces
{
    public interface IGPSDevice : IDevice
    {
        void Connect();
        void Disconnect();

        event GPSUpdateHandler PositionUpdate;
    }
}
