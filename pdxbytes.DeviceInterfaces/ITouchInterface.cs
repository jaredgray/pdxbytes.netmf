using System;
using Microsoft.SPOT;
using pdxbytes.Structures;

namespace pdxbytes.DeviceInterfaces
{
    public interface ITouchInterface
    {
        Orientations NaturalOrientation { get; }
        CoordinateSystems CoordinateSystem { get; }
        event TouchDelegate Touched;
    }

    public delegate void TouchDelegate(Touch touch);

}
