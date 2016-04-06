//using System;
//using Microsoft.SPOT;

//namespace pdxbytes.Graphics.Shapes
//{
//    public class Circle : Shape
//    {
//        /// <summary>
//        /// defaults to x and y in the center
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <param name="radius"></param>
//        /// <param name="zindex"></param>
//        /// <param name="backgroundcolor"></param>
//        public Circle(short x, short y, short radius, byte zindex = 1, ushort borderColor, ushort backgroundcolor = Color.Transparent)
//            : base(x, y, (short)(radius * 2), (short)(radius * 2), zindex, backgroundcolor)
//        { }
//        public override void Cleanup()
//        {
//            throw new NotImplementedException();
//        }
//        void drawCircle(short x0, short y0, short r, ushort color)
//        {
//            short f = 1 - r;
//            short ddF_x = 1;
//            short ddF_y = -2 * r;
//            short x = 0;
//            short y = r;

//            drawPixel(x0, y0 + r, color);
//            drawPixel(x0, y0 - r, color);
//            drawPixel(x0 + r, y0, color);
//            drawPixel(x0 - r, y0, color);

//            while (x < y)
//            {
//                if (f >= 0)
//                {
//                    y--;
//                    ddF_y += 2;
//                    f += ddF_y;
//                }
//                x++;
//                ddF_x += 2;
//                f += ddF_x;

//                drawPixel(x0 + x, y0 + y, color);
//                drawPixel(x0 - x, y0 + y, color);
//                drawPixel(x0 + x, y0 - y, color);
//                drawPixel(x0 - x, y0 - y, color);
//                drawPixel(x0 + y, y0 + x, color);
//                drawPixel(x0 - y, y0 + x, color);
//                drawPixel(x0 + y, y0 - x, color);
//                drawPixel(x0 - y, y0 - x, color);
//            }
//        }
//        public override byte[] ReadInternal(int position, int maxlength)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
