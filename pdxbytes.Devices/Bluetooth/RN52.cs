using System;
using Microsoft.SPOT;
using System.IO.Ports;

namespace pdxbytes.Devices.Bluetooth
{
    public class RN52 : BluetoothDevice
    {
        public RN52(string port, string deviceName = null)
            : base(port, BaudRate.Baudrate115200, Parity.None, 8, StopBits.One, deviceName)
        {
            base.CurrentBaud = BaudRate.Baudrate115200;
        }
    }
}
