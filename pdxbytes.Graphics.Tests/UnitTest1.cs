using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pdxbytes.Structures;
using System.Text;
using Microsoft.SPOT;

namespace pdxbytes.Graphics.Tests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        public void TestMethod1()
        {
            //byte width = 50, height = 50;
            //var bitdepth = sizeof(short);
            //var data = new byte[width * height * bitdepth];
            //var color = Palette.Blue;

            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        data[(y * width * bitdepth) + x] = (byte)(color.Value >> 8);
            //        data[(y * width * bitdepth) + x + 1] = (byte)(color.Value);
            //    }
            //}


            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        var p = new Pixel(data[(y * width * bitdepth) + x], data[(y * width * bitdepth) + x + 1]);
            //        //var c = p.GetColor();
            //    }
            //}
        }

        [TestMethod]
        public void BitTest()
        {
            var color = Palette.Blue.Value;
            var fulldisplay = Binary(color);
            Debug.Print(fulldisplay);

            Debug.Print(Binary(Palette.Blue.R));
            Debug.Print(Binary(Palette.Blue.G));
            Debug.Print(Binary(Palette.Blue.B));

            foreach(var value in Palette.Blue.Debug_GetRGB16BitValue())
            {
                Debug.Print(Binary(value));
            }
            foreach (var value in Palette.Blue.Debug_GetRGB16BitValueFromValue())
            {
                Debug.Print(Binary(value));
            }
            
            //var b1 = (byte)color;
            //var b2 = (byte)(color >> 8);

            //var d1 = Binary(b1);
            //var d2 = Binary(b2);

            //ushort result = (ushort)((b2 << 8) | b1);
            //var resultdisplay = Binary(result);

            var data = new UInt24Collection(1);
            var buffer = new PixelDataBuffer(new Size(1, 1), data, 2);
           // buffer.SetPixel(0, 0, Palette.Blue);

            var result = buffer.GetColor(0, 0);

            //for (byte i = 0; i < 254; i++)
            //{
            //    var rgbresult = RGB888ToRGB565(i, i, i);
            //    var color = RGB565ToRGB888(rgbresult); 
            //}
        }
        public static string Binary(byte b)
        {
            StringBuilder str = new StringBuilder(8);
            int[] bl = new int[8];

            for (int i = 0; i < bl.Length; i++)
            {
                bl[bl.Length - 1 - i] = ((b & (1 << i)) != 0) ? 1 : 0;
            }

            foreach (int num in bl) str.Append(num);

            return str.ToString();
        }
        public static string Binary(ushort b)
        {
            StringBuilder str = new StringBuilder(16);
            int[] bl = new int[16];

            for (int i = 0; i < bl.Length; i++)
            {
                bl[bl.Length - 1 - i] = ((b & (1 << i)) != 0) ? 1 : 0;
            }

            foreach (int num in bl) str.Append(num);

            return str.ToString();
        }
        public static string Binary(uint b)
        {
            StringBuilder str = new StringBuilder(32);
            int[] bl = new int[32];

            for (int i = 0; i < bl.Length; i++)
            {
                bl[bl.Length - 1 - i] = ((b & (1 << i)) != 0) ? 1 : 0;
            }

            foreach (int num in bl) str.Append(num);

            return str.ToString();
        }
        public static string Binary(UInt24 rgb)
        {
            StringBuilder str = new StringBuilder(32);
            int[] bl = new int[24];


            byte r = rgb.R;
            byte g = rgb.G;
            byte b = rgb.B;
            for (int i = 0; i < 8; i++)
            {
                bl[bl.Length - 1 - i] = ((r & (1 << i)) != 0) ? 1 : 0;
            }
            for (int i = 0; i < 8; i++)
            {
                bl[(bl.Length - 1 - i) + 8] = ((g & (1 << i)) != 0) ? 1 : 0;
            }
            for (int i = 0; i < 8; i++)
            {
                bl[(bl.Length - 1 - i) + 16] = ((g & (1 << i)) != 0) ? 1 : 0;
            }

            foreach (int num in bl) str.Append(num);

            return str.ToString();
        }
        public static ushort Rgb(byte r, byte g, byte b)
        {
            return (ushort)(((r & 0xF8) << 8) | ((g & 0xFC) << 3) | (b >> 3));
        }

        ushort RGB888ToRGB565(byte r, byte g, byte b)
        {
            return (ushort) ((((r >> 3) << 11) & 0xF800) | (((g >> 2) << 5) & 0x07E0) | ((b >> 3) & 0x001F));
        }
        Color2 RGB565ToRGB888(ushort value)
        {
            byte b = (byte)(((value) & 0x001F) << 3);
            byte g = (byte)(((value) & 0x07E0) >> 3);
            byte r = (byte)(((value) & 0xF800) >> 8);
            return new Color2(r, g, b);
        }
    }
    class Color2
    {
        public Color2(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R;
        public byte G;
        public byte B;
    }
}
