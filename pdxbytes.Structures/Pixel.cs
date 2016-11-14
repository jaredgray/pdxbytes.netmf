using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public struct Pixel
    {
        public Pixel(byte left, byte right)
        {
            R = G = B = 0;
        }
        public ushort GetValue()
        {
            return (ushort)((((R >> 3) << 11) & 0xF800) | (((G >> 2) << 5) & 0x07E0) | ((B >> 3) & 0x001F));
        }
        
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}
