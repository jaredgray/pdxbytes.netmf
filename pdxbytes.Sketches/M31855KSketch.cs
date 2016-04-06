
using pdxbytes.Devices.Temperature;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace pdxbytes.Sketches.Sensors.Temperature
{
    public class M31855KSketch
    {
        static M31855K sensor;
        static Timer et;
        public static void Run()
        {
            sensor = new M31855K(Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D4);
            et = new Timer(new System.Threading.TimerCallback(Tick), null, 1000 * 10, 1000 * 10);
        }

        static void Tick(object arg)
        {
            var result = sensor.ReadCelsius();
            OnUpdate(result);
        }
        static void OnUpdate(double celcious)
        {
            var handler = TemperatureUpdate;
            if (null != handler)
                handler(celcious);
        }

        public delegate void TemperatureUpdateDelegate(double celcious);
        public static event TemperatureUpdateDelegate TemperatureUpdate;
    }
}
