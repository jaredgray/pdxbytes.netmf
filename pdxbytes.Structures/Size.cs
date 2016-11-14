using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public struct Size
    {
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
