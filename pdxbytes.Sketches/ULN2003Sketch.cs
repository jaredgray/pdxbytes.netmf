using System;
using Microsoft.SPOT;
using pdxbytes.Devices.StepperMotors;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace pdxbytes.Sketches
{
    public class ULN2003Sketch
    {
        public ULN2003Sketch()
        {
            controller = new ULN2003MotorController(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3);
            controller.Delay = 1000;
        }

        ULN2003MotorController controller;

        int position = 0;
        bool direction = true;
        const int MAXPosition = 10;
        const int MINPosition = 0;

        public void Loop()
        {
            if (direction)
            {
                controller.StepRight();
                ++position;
            }
            else
            {
                controller.StepLeft();
                --position;
            }
            if (position >= MAXPosition)
                direction = false;
            else if (position <= MINPosition)
                direction = true;
        }


        public static void Run()
        {
            ULN2003Sketch sketch = new ULN2003Sketch();
            while (true)
                sketch.Loop();
        }
    }
}
