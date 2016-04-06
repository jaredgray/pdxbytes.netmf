using System;
using Microsoft.SPOT;
using pdxbytes.PresentationFramework.Controls;
using pdxbytes.Devices.Display;
using pdxbytes.DeviceDrivers.Display;
using pdxbytes.DeviceDrivers.Touch;
using pdxbytes.PresentationFramework;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace pdxbytes.Devices.Test
{
    public class MyApplication : App
    {
        public MyApplication()
        {
            this.Configure();
        }

        private void Configure()
        {
            // add display driver
            base.DeviceConfigurations.Add(new ILI9341DeviceConfiguration(Pins.GPIO_PIN_D10, Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D8, Microsoft.SPOT.Hardware.SPI.SPI_module.SPI1, 84000));
            base.DeviceConfigurations.Add(new FT6206CAPI2CDeviceConfiguration(Pins.GPIO_PIN_D7, PWMChannels.PWM_PIN_D6));

            this.MainView = new View()
            {
                Width = ILI9341_240x320CAP.WIDTH,
                Height = ILI9341_240x320CAP.HEIGHT,
                BackgroundColor = Palette.White
            };

            var panel = new Panel()
            {
                Width = ILI9341_240x320CAP.WIDTH,
                Height = 40,
                BackgroundColor = Palette.Blue
            };

            var burger = new Hamburger()
            {
                X = 10,
                Y = 12,
                Width = 18,
                Height = 15,
                ForegroundColor = Palette.White,
                BackgroundColor = Palette.Blue,
                Zindex = 4,
                HitAreaOffset = 10
            };
            panel.Controls.Add(burger);

            this.MainView.Controls.Add(panel);

        }
    }
}
