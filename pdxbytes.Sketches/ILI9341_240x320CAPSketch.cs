
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Hardware;
using pdxbytes.Graphics;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using pdxbytes.Graphics.Text;
using pdxbytes.Graphics.Shapes;
using pdxbytes.Structures;
using pdxbytes.Devices.Display;
using pdxbytes.Devices.Core;

namespace pdxbytes.Sketches.Display
{
    public static class ILI9341_240x320CAPSketch
    {
        // pin setup
        static Cpu.Pin CS_PIN = Pins.GPIO_PIN_D10;
        static Cpu.Pin DC_PIN = Pins.GPIO_PIN_D9;
        static Cpu.Pin RST_PIN = Pins.GPIO_PIN_D8;
        const uint KHZ = 84000;


        public static ILI9341_240x320CAP tft;

        public static void Run()
        {
            //84000
            //40000
            tft = new ILI9341_240x320CAP(cs: CS_PIN, dc: DC_PIN, reset: RST_PIN, speedKHz: KHZ);
            //DisplayPicture();
            //tft.ClearScreen();
            DrawObjects();
            //DisplayGradient();

            //DisplayCircles();

            //tft.ClearScreen();
            //DisplayLines();

            //DisplayColorFlow();
        }


        /*
public static void DisplayColorFlow()
{
#if NETDUINO_MINI
   StorageDevice.MountSD("SD", SPI.SPI_module.SPI1, Pins.GPIO_PIN_13);
#else
   StorageDevice.MountSD("SD", SPI.SPI_module.SPI1, Pins.GPIO_PIN_D10);
#endif
   while (true)
   {
       ReadPicture(@"SD\Pictures\ColorFlow1.bmp.24.bin", 0);
       ReadPicture(@"SD\Pictures\ColorFlow2.bmp.24.bin", 0);
       ReadPicture(@"SD\Pictures\ColorFlow3.bmp.24.bin", 0);
       ReadPicture(@"SD\Pictures\ColorFlow4.bmp.24.bin", 0);
   }
}

public static void DisplayPicture()
{
#if NETDUINO_MINI
   StorageDevice.MountSD("SD", SPI.SPI_module.SPI1, Pins.GPIO_PIN_13);
#else
   StorageDevice.MountSD("SD", SPI.SPI_module.SPI1, Pins.GPIO_PIN_D10);
#endif
   ReadPicture(@"SD\Pictures\spaceneedle.bmp.24.bin");
   ReadPicture(@"SD\Pictures\spaceneedleclose.bmp.24.bin");
   ReadPicture(@"SD\Pictures\spaceneedlesunset.bmp.24.bin");
   ReadPicture(@"SD\Pictures\spaceneedleatnight.bmp.24.bin");

   StorageDevice.Unmount("SD");
}

public static void ReadPicture(string filename, int delay = 1000)
{
   using (var filestream = new FileStream(filename, FileMode.Open))
   {
       filestream.Read(tft.SpiBuffer, 0, tft.SpiBuffer.Length);
       tft.Refresh();
       Thread.Sleep(delay);
   }
}
*/

        /*
    public static void DisplayLines()
    {
        byte red = 20;
        byte green = 1;
        byte blue = 5;
        var y = 0;

        for (; y < PITFT240x320CAP.Height; y++)
        {
            red += 2;
            green++;
            tft.DrawLine(0, 0, PITFT240x320CAP.Width, y, tft.GetRGBColor(red, green, blue));
            tft.Refresh();
        }

        red = 20;
        green = 1;
        blue = 5;
        for (; y >= 0; y--)
        {
            red += 2;
            green++;
            tft.DrawLine(PITFT240x320CAP.Width - 1, PITFT240x320CAP.Height - 1, 0, y, tft.GetRGBColor(red, green, blue));
            tft.Refresh();
        }
    }
*/
        public static void DisplayCircles()
        {
            //var xHalf = PITFT240x320CAP.Width / 2;
            //var yHalf = PITFT240x320CAP.Height / 2;
            //byte red = 1;
            //byte green = 1;
            //byte blue = 1;

            //for (var r = 1; r < xHalf; r += 2)
            //{
            //    var color = tft.GetRGBColor(red, green, blue);
            //    tft.DrawCircle(xHalf, yHalf, r, color);
            //    red += 3;
            //    green += 2;
            //    blue += 1;
            //    tft.Refresh();
            //}

            //Thread.Sleep(1000);

            //for (var I = 0; I < 2; I++)
            //{
            //    var r = 1;
            //    for (; r < xHalf; r += 2)
            //    {
            //        tft.DrawCircle(xHalf, yHalf, r, (ushort)PITFT240x320CAP.Colors.White);
            //        tft.Refresh();
            //        tft.DrawCircle(xHalf, yHalf, r, (ushort)PITFT240x320CAP.Colors.Black);
            //    }
            //    for (; r > 1; r -= 2)
            //    {
            //        tft.DrawCircle(xHalf, yHalf, r, (ushort)PITFT240x320CAP.Colors.White);
            //        tft.Refresh();
            //        tft.DrawCircle(xHalf, yHalf, r, (ushort)PITFT240x320CAP.Colors.Black);
            //    }
            //}

            Thread.Sleep(1000);
        }

        public static void DisplayGradient()
        {
            //var x = 0;
            //var y = 0;

            //while (y < PITFT240x320CAP.Height)
            //{
            //    byte red = 1;
            //    for (; red < 32; red += 3)
            //    {
            //        byte green = 1;
            //        for (; green < 33; green += 2)
            //        {
            //            byte blue = 1;
            //            for (; blue < 32; blue += 2)
            //            {
            //                var color = tft.GetRGBColor(red, green, blue);

            //                tft.DrawPixel(x++, y, color);

            //                if (x >= PITFT240x320CAP.Width)
            //                {
            //                    x = 0;
            //                    y++;
            //                }
            //            }
            //        }
            //    }
            //    tft.Refresh();
            //}
        }

        public static void DrawObjects()
        {
            Pipeline pipeline = new Pipeline(tft);
            var f = new GLcd();
            var pink = new Color(255, 0, 190);
            var blue = new Color(67, 165, 248);
            var white = new Color(255, 255, 255);
            pipeline.AddDisplay(new Rectangle(0, 0, tft.Width, tft.Height, 0, white));
            pipeline.AddDisplay(new Rectangle(0, 0, tft.Width, 40, 1, blue));
            pipeline.AddDisplay(new Hamburger(x: 10, y: 12, width: 18, height: 15, zindex: 1, foregroundcolor: white, backgroundcolor: blue));
            //pipeline.AddDisplay(new Graphics.Shapes.Rectangle(5, 5, 40, 80, 1, Color.Rgb(255, 0, 0)));
            //pipeline.AddDisplay(new Graphics.Shapes.Rectangle(65, 5, 40, 80, 1, Color.Rgb(0, 255, 0)));
            pipeline.AddDisplay(new TextBlock("SMD Oven", 40, 15, 1, 8, white, white, f));
            pipeline.Flush();


            Debug.Print("Committed to window");
            //tft.DrawString("Hello World", 0, 0, 2, 222);
            //using (Bitmap bmp = new Bitmap(FT6206240x320CAP.Width, FT6206240x320CAP.Height))
            //{
            //    var color = Microsoft.SPOT.Presentation.Media.ColorUtility.ColorFromRGB(0, 255, 0);
            //    bmp.DrawRectangle(color, 0, 0, 0, FT6206240x320CAP.Width, FT6206240x320CAP.Height, 0, 0, color, 0, 0, color, 0, 0, 1);
            //    bmp.Flush();
            //}
            
        }
    }
}
