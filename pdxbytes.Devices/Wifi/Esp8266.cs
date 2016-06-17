using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.IO.Ports;
using pdxbytes.Devices.Core;
using System.Threading;
using System.Text;

namespace pdxbytes.Devices.Wifi
{
    /// <summary>
    /// provides an interface to the ESP8266 wifi module from Addicore (http://www.addicore.com/products-p/130.htm)
    /// </summary>
    public class Esp8266 : SerialPortDevice
    {
        public Esp8266(SerialWifiConnection connectioninfo)
            : base(connectioninfo)
        {
            State = WifiStates.Closed;
        }
        const int BUFFER_SIZE = 1024;

        public WifiStates State { get; set; }

        public void Connect()
        {
            if (IsConnected)
                return;
            base.Open();
            // read from device....
            Debug.Print("sending at start command");
            base.Write(new byte[] 
            {
                (byte)'A',
                (byte)'T',
                (byte)'+',
                (byte)'R',
                (byte)'S',
                (byte)'T',
                (byte)'\r',
                (byte)'\n'
            }, 0, 8);
            //base.Write("AT+RST\r\n");
        }

        protected override void OnDataReceived(SerialDataReceivedEventArgs args)
        {
            // if state is closed, just wait for string data otherwise process bytes
            base.OnDataReceived(args);
        }

        protected override void OnDataReceived(string value)
        {
            if (value == "ready")
            {
                Debug.Print("Connected!!");
                State = WifiStates.Open;
            }
            else
            {
                Debug.Print(value);
            }
        }
    }
    public enum WifiStates
    {
        Closed = 0,
        Open = 1,
        Faulted = 2
    }
    public class SerialWifiConnection : SerialPortConnection
    {
        public SerialWifiConnection()
        {

        }

        public string SSID { get; set; }
        public string Password { get; set; }
    }
}
