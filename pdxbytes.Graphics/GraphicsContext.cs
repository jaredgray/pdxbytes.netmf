
using pdxbytes.DeviceInterfaces;

namespace pdxbytes.Graphics
{
    public class GraphicsContext
    {
        public GraphicsContext(IGraphicDevice device)
        {
            this.Device = device;
        }

        public IGraphicDevice Device { get; private set; }
        
        private void Swap(ref int a, ref int b)
        {
            var t = a; a = b; b = t;
        }
        //public void DrawPixel(int x, int y, ushort color)
        //{
        //    SetPixel(x, y, color);
        //    if (AutoRefreshScreen)
        //    {
        //        Refresh();
        //    }
        //}

        //// Bresenham's algorithm: http://en.wikipedia.org/wiki/Bresenham's_line_algorithm
        //public void DrawLine(int startX, int startY, int endX, int endY, ushort color)
        //{
        //    int steep = (System.Math.Abs(endY - startY) > System.Math.Abs(endX - startX)) ? 1 : 0;

        //    if (steep != 0)
        //    {
        //        Swap(ref startX, ref startY);
        //        Swap(ref endX, ref endY);
        //    }

        //    if (startX > endX)
        //    {
        //        Swap(ref startX, ref endX);
        //        Swap(ref startY, ref endY);
        //    }

        //    int dx, dy;
        //    dx = endX - startX;
        //    dy = System.Math.Abs(endY - startY);

        //    int err = dx / 2;
        //    int ystep = 0;

        //    if (startY < endY)
        //    {
        //        ystep = 1;
        //    }
        //    else
        //    {
        //        ystep = -1;
        //    }

        //    for (; startX < endX; startX++)
        //    {
        //        if (steep != 0)
        //        {
        //            SetPixel(startY, startX, color);
        //        }
        //        else
        //        {
        //            SetPixel(startX, startY, color);
        //        }
        //        err -= dy;
        //        if (err < 0)
        //        {
        //            startY += ystep;
        //            err += dx;
        //        }
        //    }
        //    if (AutoRefreshScreen)
        //    {
        //        Refresh();
        //    }
        //}

        //public void DrawCircle(int centerX, int centerY, int radius, ushort color)
        //{
        //    int f = 1 - radius;
        //    int ddF_x = 1;
        //    int ddF_y = -2 * radius;
        //    int x = 0;
        //    int y = radius;

        //    SetPixel(centerX, centerY + radius, color);
        //    SetPixel(centerX, centerY - radius, color);
        //    SetPixel(centerX + radius, centerY, color);
        //    SetPixel(centerX - radius, centerY, color);

        //    while (x < y)
        //    {
        //        if (f >= 0)
        //        {
        //            y--;
        //            ddF_y += 2;
        //            f += ddF_y;
        //        }

        //        x++;
        //        ddF_x += 2;
        //        f += ddF_x;

        //        SetPixel(centerX + x, centerY + y, color);
        //        SetPixel(centerX - x, centerY + y, color);
        //        SetPixel(centerX + x, centerY - y, color);
        //        SetPixel(centerX - x, centerY - y, color);

        //        SetPixel(centerX + y, centerY + x, color);
        //        SetPixel(centerX - y, centerY + x, color);
        //        SetPixel(centerX + y, centerY - x, color);
        //        SetPixel(centerX - y, centerY - x, color);
        //    }
        //    if (AutoRefreshScreen)
        //    {
        //        Refresh();
        //    }
        //}
        //public void ClearScreen(ushort color = (ushort)Colors.Black)
        //{
        //var high = (byte)(color >> 8);
        //var low = (byte)color;

        //var index = 0;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;
        //SpiBuffer[index++] = high;
        //SpiBuffer[index++] = low;

        //Array.Copy(SpiBuffer, 0, SpiBuffer, 16, 16);
        //Array.Copy(SpiBuffer, 0, SpiBuffer, 32, 32);
        //Array.Copy(SpiBuffer, 0, SpiBuffer, 64, 64);
        //Array.Copy(SpiBuffer, 0, SpiBuffer, 128, 128);
        //Array.Copy(SpiBuffer, 0, SpiBuffer, 256, 256);

        //index = 512;
        //var line = 0;
        //var Half = Height / 2;
        //while (++line < Half - 1)
        //{
        //    Array.Copy(SpiBuffer, 0, SpiBuffer, index, 256);
        //    index += 256;
        //}

        ////Array.Copy(SpiBuffer, 0, SpiBuffer, index, SpiBuffer.Length / 2);

        //if (AutoRefreshScreen)
        //{
        //    Refresh();
        //}
        //}
    }
}
