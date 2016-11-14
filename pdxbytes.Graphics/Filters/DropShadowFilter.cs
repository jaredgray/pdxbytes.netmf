using System;
using Microsoft.SPOT;
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Filters
{
    public class DropShadowFilter
    {
        //public static unsafe bool ApplyConvolution(Size size, byte[] data, ConvolutionMatrix m)
        //{
        //    // Avoid divide by zero errors
        //    if (0 == m.Factor) return false;
        //    var bitdepth = sizeof(short);
        //    var stride = size.Width * bitdepth;
        //    var pixelbuffer = new PixelDataBuffer(size, data, bitdepth);
        //    // a 3 row buffer which allows us to update the data buffer while
        //    // still using it for our convolution
        //    var tempbuffer = new byte[size.Width * 3 * bitdepth];

        //    for (int y = 0; y < size.Height; ++y)
        //    {
        //        for (int x = 0; x < size.Width; x += 2)
        //        {
        //            var pixel00 = pixelbuffer.GetColor(x, y);
        //            var pixel01 = pixelbuffer.GetColor(x, y);
        //            var pixel02 = pixelbuffer.GetColor(x, y);
        //            var pixel10 = pixelbuffer.GetColor(x, y);
        //            var pixel11 = pixelbuffer.GetColor(x, y);
        //            var pixel12 = pixelbuffer.GetColor(x, y);
        //            var pixel20 = pixelbuffer.GetColor(x, y);
        //            var pixel21 = pixelbuffer.GetColor(x, y);
        //            var pixel22 = pixelbuffer.GetColor(x, y);

        //            int nPixel = ((((pSrc[3] * m.TopLeft) + (pSrc[7] * m.TopMid) + (pSrc[11] * m.TopRight) +
        //                            (pSrc[3 + stride] * m.MidLeft) + (pSrc[7 + stride] * m.Pixel) + (pSrc[11 + stride] * m.MidRight) +
        //                            (pSrc[3 + stride2] * m.BottomLeft) + (pSrc[7 + stride2] * m.BottomMid) + (pSrc[11 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

        //            nPixel = Clamp(0, 255, nPixel);

        //            p[7 + stride] = (byte)nPixel;

        //            nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[6] * m.TopMid) + (pSrc[10] * m.TopRight) +
        //                        (pSrc[2 + stride] * m.MidLeft) + (pSrc[6 + stride] * m.Pixel) + (pSrc[10 + stride] * m.MidRight) +
        //                        (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[6 + stride2] * m.BottomMid) + (pSrc[10 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

        //            nPixel = Clamp(0, 255, nPixel);

        //            p[6 + stride] = (byte)nPixel;

        //            nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[9] * m.TopRight) +
        //                        (pSrc[1 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[9 + stride] * m.MidRight) +
        //                        (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[9 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

        //            nPixel = Clamp(0, 255, nPixel);

        //            p[5 + stride] = (byte)nPixel;


        //            p += 4;
        //            pSrc += 4;
        //        }
        //        p += nOffset;
        //        pSrc += nOffset;
        //    }

        //    b.UnlockBits(bmData);
        //    bSrc.UnlockBits(bmSrc);

        //    return true;
        //}


        public static UInt24Collection ApplyConvolutionFilter(Size size, UInt24Collection data, ConvolutionFilterBase filter)
        {
            var bitdepth = sizeof(short);
            var pixelbuffer = new PixelDataBuffer(size, data, bitdepth);
            // a 3 row buffer which allows us to update the data buffer while
            // still using it for our convolution
            var resultBuffer = new UInt24Collection(data.Length);

            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterWidth = filter.XSize();
            int filterHeight = filter.YSize();

            int filterOffset = (filterWidth - 1) / 2;
            int byteOffset = 0;
            var resultOffset = 0;
            for (int offsetY = filterOffset; offsetY < size.Height - filterOffset; offsetY++)
            {

                for (int offsetX = filterOffset; offsetX < size.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;
                    byteOffset = ((offsetY) * pixelbuffer.Stride) + offsetX;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                        {
                            var pixelColorOffset = pixelbuffer.GetColor(offsetX + filterX, offsetY + filterY);

                            red += (double)(pixelColorOffset.R) * filter.FilterMatrix[filterY + filterOffset][filterX + filterOffset];
                            green += (double)(pixelColorOffset.G) * filter.FilterMatrix[filterY + filterOffset][filterX + filterOffset];
                            blue += (double)(pixelColorOffset.B) * filter.FilterMatrix[filterY + filterOffset][filterX + filterOffset];
                        }
                    }

                    red = Clamp(0, 255, (int)(filter.Factor * red + filter.Bias));
                    green = Clamp(0, 255, (int)(filter.Factor * green + filter.Bias));
                    blue = Clamp(0, 255, (int)(filter.Factor * blue + filter.Bias));

                    var color = new Color((byte)red, (byte)green, (byte)blue);
                    resultBuffer[byteOffset] = color.Value;
                }
            }

            return resultBuffer;
            // copy the rest of the resultbuffer back to the source
        }
        private static int Clamp(int low, int high, int input)
        {
            if (input > high)
                return high;
            if (input < low)
                return low;
            return input;
        }
    }
    public abstract class ConvolutionFilterBase
    {
        public abstract string FilterName
        {
            get;
        }


        public abstract double Factor
        {
            get;
        }


        public abstract double Bias
        {
            get;
        }


        public abstract double[][] FilterMatrix
        {
            get;
        }

        public abstract int XSize();
        public abstract int YSize();
    }
    public class Blur3x3Filter : ConvolutionFilterBase
    {
        public override string FilterName
        {
            get { return "Blur3x3Filter"; }
        }


        private double factor = 1.0;
        public override double Factor
        {
            get { return factor; }
        }


        private double bias = 0.0;
        public override double Bias
        {
            get { return bias; }
        }

        const double multiplier = 0.2;
        private double[][] filterMatrix =
            new double[][] { new double[] { 0.0, multiplier, 0.0, },
                       new double[] { multiplier, multiplier, multiplier, },
                       new double[] { 0.0, multiplier, multiplier, }, };


        public override double[][] FilterMatrix
        {
            get { return filterMatrix; }
        }

        public override int XSize()
        {
            return 3;
        }

        public override int YSize()
        {
            return 3;
        }
    }

    //public class ConvolutionMatrix
    //{
    //    public int TopLeft = 0, TopMid = 0, TopRight = 0;
    //    public int MidLeft = 0, Pixel = 1, MidRight = 0;
    //    public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
    //    public int Factor = 1;
    //    public int Offset = 0;
    //    public void SetAll(int nVal)
    //    {
    //        TopLeft = TopMid = TopRight =
    //            MidLeft = Pixel = MidRight =
    //            BottomLeft = BottomMid = BottomRight = nVal;
    //    }
    //}
}
