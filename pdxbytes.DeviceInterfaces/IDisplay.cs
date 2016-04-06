using System;
using Microsoft.SPOT;

namespace pdxbytes.DeviceInterfaces
{
    public interface IDisplay : IGraphicDevice
    {
        void DisplayOn();
        void Reset();
        void Sleep();
        void Wake();
        Orientations CurrentOrientation { get; set; }
        short Width { get; }
        short Height { get; }
    }
}
