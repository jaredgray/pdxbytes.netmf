using System;
using Microsoft.SPOT;
using System.IO.Ports;
using System.IO;
using System.Text;

namespace pdxbytes.Devices.Core
{
    public abstract class SerialPortDevice
    {
        public SerialPortDevice(SerialPortConnection connectioninfo)
        {
            ConnectionInfo = connectioninfo;
            BufferSize = 1024;
        }
        public bool IsConnected { get; set; }

        private SerialPortConnection ConnectionInfo { get; set; }
        private SerialPort Port { get; set; }

        protected int BufferSize { get; set; }


        protected void Open()
        {
            if (IsConnected)
                return;
            IsConnected = true;
            Debug.Print("Opening connection to serial device");
            try
            {
                Port = new SerialPort(ConnectionInfo.ComPort, (int)ConnectionInfo.BaudRate, ConnectionInfo.Pairity, ConnectionInfo.DataBits, ConnectionInfo.StopBits);
                Port.ReadTimeout = ConnectionInfo.ReadTimeoutMs; // Set to 10ms. Default is -1?!
                Port.DataReceived += Port_DataReceived;
                Port.Open();
            }
            catch (Exception ex)
            {
                IsConnected = false;
                this.OnFailedConnection(ex);
            }

        }
        protected void Close()
        {
            if (IsConnected)
            {
                IsConnected = false;
                Port.Close();
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.OnDataReceived(e);
        }

        protected virtual void OnDataReceived(SerialDataReceivedEventArgs args)
        {
            Debug.Print("data received");
            var buffer = new byte[BufferSize];
            int len = 0;
            StringBuilder sb = new StringBuilder();
            while (0 < (len = Read(buffer, 0, BufferSize)))
            {
                if (len > 0)
                {
                    sb.Append(Encoding.UTF8.GetChars(buffer));
                }
                buffer = new byte[BufferSize];
            }
            if (sb.Length > 0)
                OnDataReceived(sb.ToString());
        }

        protected virtual void OnDataReceived(string data)
        {

        }

        protected virtual void OnFailedConnection(Exception ex)
        { }

        protected virtual void OnConnect()
        {

        }


        protected void DiscardInBuffer()
        {
            if (IsConnected)
            {
                Port.DiscardInBuffer();
            }
        }
        protected void DiscardOutBuffer()
        {
            if (IsConnected)
            {
                Port.DiscardOutBuffer();
            }
        }
        protected void Flush()
        {
            if (IsConnected)
            {
                Port.Flush();
            }
        }
        protected int Read(byte[] buffer, int offset, int count)
        {
            if (IsConnected)
            {
                try
                {
                    return Port.Read(buffer, offset, count);
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.ToString());
                }
            }
            return 0;
        }
        protected long Seek(long offset, SeekOrigin origin)
        {
            if (IsConnected)
            {
                return Port.Seek(offset, origin);
            }
            return 0;
        }
        protected void SetLength(long value)
        {
            if (IsConnected)
            {
                Port.SetLength(value);
            }
        }
        protected void Write(string value)
        {
            if (IsConnected)
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                Write(buffer, 0, buffer.Length);
            }
        }
        protected void Write(byte[] buffer, int offset, int count)
        {
            if (IsConnected)
            {
                Port.Write(buffer, offset, count);
            }
        }
    }
}
