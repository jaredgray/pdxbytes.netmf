using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public struct Vec2
    {
        public Vec2(int x, int y) { this.X = x; this.Y = y; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
