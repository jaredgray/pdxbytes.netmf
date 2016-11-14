using System;
using Microsoft.SPOT;
using pdxbytes.Structures;
using pdxbytes.Graphics.Filters;

namespace pdxbytes.Graphics.Shapes
{
    public class TestDisplayBuffer : Shape
    {
        public TestDisplayBuffer()
        {
            Width = 20;
            Height = 20;
        }
        public override void Cleanup()
        {
        }

        double gaussian(double x, double sigma)
        {
            const double pi = 3.141592653589793;
            return System.Math.Exp(-(x * x) / (2.0 * sigma * sigma)) / (System.Math.Sqrt(2.0 * pi) * sigma);
        }

        public override UInt24Collection ReadInternal(int position, int maxlength)
        {
            var color = Palette.Blue;
            var shadowfrom = Palette.Black;
            var shadowto = Palette.Gray10;


            var buffer = new UInt24Collection(this.Width * this.Height);
            var shadowsize = 3;

            for (int row = 0; row < this.Height; row++)
            {
                for (int col = 0; col < this.Width; col++)
                {
                    base.SetPixel(row, col, Palette.White, buffer);
                }
            }

            for (int row = shadowsize; row < this.Height - shadowsize; row++)
            {
                for (int col = shadowsize; col < this.Width - shadowsize; col++)
                {
                    base.SetPixel(row, col, color, buffer);
                }
            }

            buffer = DropShadowFilter.ApplyConvolutionFilter(new Size(this.Width, this.Height), buffer, new Blur3x3Filter());

            return buffer;
        }
        //public override byte[] ReadInternal(int position, int maxlength)
        //{
        //    if (position >= this.Length)
        //        return null;
        //    var color = Palette.White;
        //    var c1A = (byte)(color.Value >> 8);
        //    var c1B = (byte)color.Value;
        //    var color2 = Palette.Blue;
        //    var c2A = (byte)(color2.Value >> 8);
        //    var c2B = (byte)color2.Value;
        //    var bitdepth = sizeof(short);
        //    var buffer = new byte[this.Width * this.Height * bitdepth];
        //    var offset = 0;
        //    for (int row = 0; row < this.Height / 2; row++)
        //    {
        //        for (int col = 0; col < this.Width; col++)
        //        {
        //            buffer[offset + (col * bitdepth)] = c1A;
        //            buffer[offset + (col * bitdepth) + 1] = c1B;
        //        }
        //        offset += this.Width * bitdepth;
        //    }
        //    for (int row = this.Height / 2; row < this.Height; row++)
        //    {
        //        for (int col = 0; col < this.Width; col++)
        //        {
        //            buffer[offset + (col * bitdepth)] = c2A;
        //            buffer[offset + (col * bitdepth) + 1] = c2B;
        //        }
        //        offset += this.Width * bitdepth;
        //    }
        //    return buffer;
        //}
    }
}
