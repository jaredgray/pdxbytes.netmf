using System;
using Microsoft.SPOT;
using pdxbytes.Devices.Core;
using pdxbytes.DeviceInterfaces;
using Microsoft.SPOT.Hardware;
using static Microsoft.SPOT.Hardware.Cpu;

namespace pdxbytes.Devices.GPS
{
    /*
     This class was built and tested for the Freetronics gps module: http://www.freetronics.com.au/products/gps
     */
    public class FGPMMOPA6H : GPSDevice, IGPSDevice
    {
        public FGPMMOPA6H(SerialPortConnection connectioninfo, Pin fixPin) : base(connectioninfo, fixPin)
        {
        }
        
    }
}
