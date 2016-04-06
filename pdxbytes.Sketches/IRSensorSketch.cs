using System;
using Microsoft.SPOT;
using pdxbytes.Devices.IR;
using Microsoft.SPOT.Hardware;

namespace pdxbytes.Sketches
{
    public class IRSensorSketch
    {
        static IRSensor sensor;
        public static void Run(Cpu.Pin switchpin)
        {
            port = new OutputPort(switchpin, currentstate);
          
            // PwmDecoder decoder = new PwmDecoder();
            IRDecoder decoder = new IRDecoder(new IRDecoderOptions()
            {
                HeaderLength = 2,
                PercentAccuracy = 0.8,
                Rounding = 50,
                TrueTicks = 1600,
                RecordDataPulseHigh = true
            });
            sensor = new IRSensor(SecretLabs.NETMF.Hardware.NetduinoPlus.Pins.GPIO_PIN_D0, decoder);
            sensor.Received += Sensor_Received;
        }

        static OutputPort port;
        static bool currentstate;
        static DateTime debounce;
        static long debouncetime = TimeSpan.TicksPerMillisecond * 50;
        private static void Sensor_Received(byte[] bytes)
        {
            if (bytes.Length != 4)
                return;
            if (DateTime.Now.Subtract(debounce) < TimeSpan.FromTicks(debouncetime))
                return;
            currentstate = !currentstate;
            Debug.Print("Switching to " + currentstate.ToString());
            port.Write(currentstate);
            debounce = DateTime.Now;
        }
    }
}
