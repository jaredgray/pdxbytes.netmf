using System;
using Microsoft.SPOT;
using pdxbytes.Devices.Wifi;

namespace pdxbytes.Sketches
{
    public class Esp8266WifiSketch
    {
        static Esp8266 device;
        public static void Run()
        {
            device = new Esp8266(new SerialWifiConnection()
            {
                BaudRate = System.IO.Ports.BaudRate.Baudrate9600,
                ReadTimeoutMs = 5000,
                SSID = "VNET",
                Password = "{7AE754BA-C379-4BAD-9801"
            });
           device.Connect();

            
        }
    }
}
