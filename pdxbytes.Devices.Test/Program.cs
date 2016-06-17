
using Microsoft.SPOT;
using pdxbytes.Sketches;
using pdxbytes.Sketches.Bluetooth;
using pdxbytes.Sketches.Display;
using pdxbytes.Sketches.Sensors.Temperature;
using System.Threading;

namespace pdxbytes.Devices.Test
{
    public class Program
    {
        public static void Main()
        {

            MyApplication app = new MyApplication();
            app.Run();

            //Esp8266WifiSketch.Run();

            //ILI9341_240x320CAPSketch.Run();
            //FT6206CAPI2C_Sketch.Run();

            //var state = System.Diagnostics.DebuggerBrowsableState.Collapsed;
            //if(state == System.Diagnostics.DebuggerBrowsableState.Collapsed)
            //{
            //    Debug.Print("loaded debuggerbrowsablestate");
            //}
            // write your code here




            //M31855KSketch.Run();

            //HC06Sketch.Run();

            //IRSensorSketch.Run(SecretLabs.NETMF.Hardware.NetduinoPlus.Pins.GPIO_PIN_D1);

            //ULN2003Sketch.Run();

            while (true)
            {
                Thread.Sleep(1000);

            }
        }

    }
}
