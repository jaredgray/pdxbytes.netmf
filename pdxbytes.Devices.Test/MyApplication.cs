using System;
using Microsoft.SPOT;
using pdxbytes.PresentationFramework.Controls;
using pdxbytes.Devices.Display;
using pdxbytes.DeviceDrivers.Display;
using pdxbytes.DeviceDrivers.Touch;
using pdxbytes.PresentationFramework;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using pdxbytes.Structures;
using pdxbytes.PresentationFramework.Controls.Material;

namespace pdxbytes.Devices.Test
{
    public class MyApplication : App
    {
        public MyApplication()
        {
        }

        public override void OnStartup()
        {
            base.OnStartup();
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
                BackgroundColor = Palette.Gray10
            };

            var panel = new MaterialPanel()
            {
                Width = ILI9341_240x320CAP.WIDTH,
                Height = 40,
                BackgroundColor = Palette.Blue,
                ShadowSize = 2,
                ShadowColor = Palette.ShadowLightDark,
                ShadowEdges = Edges.Bottom
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

            var materialpanel = new MaterialPanel()
            {
                Width = ILI9341_240x320CAP.WIDTH - 10,
                Height = 80,
                BackgroundColor = Palette.White,
                X = 5,
                Y = 10 + 40,
                ShadowSize = 2,
                ShadowColor = Palette.ShadowLightDark
            };

            this.MainView.Controls.Add(materialpanel);

            //var testnode = new TestDisplay()
            //{
            //    X = 50,
            //    Y = (short)(10 + materialpanel.Y + materialpanel.Height)
            //};
            //this.MainView.Controls.Add(testnode);
        }
    }
}
