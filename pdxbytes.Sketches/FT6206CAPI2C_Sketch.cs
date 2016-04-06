using System;
using Microsoft.SPOT;
using pdxbytes.Devices.Display;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace pdxbytes.Sketches
{
    public class FT6206CAPI2C_Sketch
    {
        // pin setup
        static Cpu.Pin TOUCH_PIN = Pins.GPIO_PIN_D7;
        static Cpu.PWMChannel CAPACITANCE_CHANNEL = PWMChannels.PWM_PIN_D6;

        static FT6206CAPI2C touchdisplay;
        public static void Run()
        {
            //touchInterrupt: TOUCH_PIN
            //capacitance: CAPACITANCE_CHANNEL
            touchdisplay = new FT6206CAPI2C(irq: TOUCH_PIN, capacitance: CAPACITANCE_CHANNEL);
            touchdisplay.Touched += Touchdisplay_Touched;
            touchdisplay.Initialize();
        }

        private static void Touchdisplay_Touched(Structures.Touch touch)
        {
            Debug.Print("touch: " + touch.Touch1.X + "," + touch.Touch1.Y);
        }
        
    }
}
