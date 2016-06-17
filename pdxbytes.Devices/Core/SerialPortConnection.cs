using System;
using Microsoft.SPOT;
using System.IO.Ports;

namespace pdxbytes.Devices.Core
{
    public class SerialPortConnection
    {
        public SerialPortConnection()
        {
            this.ComPort = Serial.COM1;
            this.BaudRate = BaudRate.Baudrate115200;
            this.Pairity = Parity.None;
            this.DataBits = 8;
            this.StopBits = StopBits.One;
            this.ReadTimeoutMs = 10;
        }
        public string ComPort { get; set; }
        public BaudRate BaudRate { get; set; }
        public Parity Pairity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public int ReadTimeoutMs { get; set; }
    }
}
