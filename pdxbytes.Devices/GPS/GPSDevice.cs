using System;
using Microsoft.SPOT;
using pdxbytes.Devices.Core;
using pdxbytes.DeviceInterfaces;
using static Microsoft.SPOT.Hardware.Cpu;
using Microsoft.SPOT.Hardware;
using pdxbytes.Structures;

namespace pdxbytes.Devices.GPS
{
    public class GPSDevice : SerialPortDevice, IGPSDevice
    {
        public GPSDevice(SerialPortConnection connectioninfo, Pin fixPin) : base(connectioninfo)
        {
            this.fixPin = fixPin;
            base.BufferSize = 256;
        }

        private Pin fixPin { get; set; }
        public InputPort FixPort { get; set; }
        public void Initialize()
        {
            //FixPort = new InterruptPort(this.fixPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            //FixPort.OnInterrupt += FixPort_OnInterrupt;
        }

        private void FixPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            Debug.Print("GPS fixPort d1: " + data1 + ", d2: " + data2);
        }

        protected override void OnDataReceived(string data)
        {
            //$GPRMC,194530.000,A,3051.8007,N,10035.9989,W,1.49,111.67,310714,,,A*74 
            if (null != data)
            {
                var lines = data.Split('\r', '\n');

                foreach(var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length > 7 && parts[0] == "$GPRMC" && parts[2] == "A")
                    {
                        var lonhemishpere = parts[4];
                        var londata = ParseLocation(parts[3], lonhemishpere);
                        var lathemishpere = parts[6];
                        var latdata = ParseLocation(parts[5], lathemishpere);

                        if(null != latdata && null != londata)
                        {
                            this.OnUpdateReady(new GPSUpdate()
                            {
                                //TimestampUTC = new DateTime(long.Parse(parts[1])),
                                Point = new GPSPlot()
                                {
                                    X = londata,
                                    Y = latdata
                                }
                            });
                        }
                    }
                }
            }
        }

        protected virtual void OnUpdateReady(GPSUpdate update)
        {
            var handler = PositionUpdate;
            if (null != handler)
                PositionUpdate(update);
        }

        protected GPSPosition ParseLocation(string data, string hemisphere)
        {
            var sign = "";
            if (hemisphere == "S" || hemisphere == "W")
                sign = "-";

            var decindex = data.IndexOf('.') - 2;

            if(decindex > 0)
            {
                var p1 = double.Parse(sign + data.Substring(0, decindex));
                var p2 = double.Parse(sign + data.Substring(decindex)) / 60;
                return new GPSPosition()
                {
                    Value = p1 + p2,
                    Data = data + "," + hemisphere
                };
            }

            return null;
        }

        public void Connect()
        {
            base.Open();
        }

        public void Disconnect()
        {
            base.Close();
        }

        public event GPSUpdateHandler PositionUpdate;
    }
}
