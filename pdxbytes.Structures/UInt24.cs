using System;
using Microsoft.SPOT;
using System.Runtime.InteropServices;

namespace pdxbytes.Structures
{
   // [StructLayout(LayoutKind.Sequential)]
    public class UInt24
    {
        public UInt24(UInt32 value)
        {
            R = (byte)(value & 0xFF);
            G = (byte)(value >> 8);
            B = (byte)(value >> 16);
        }
        public UInt24(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public override int GetHashCode()
        {
            return R + G + B;
        }



        //public UInt32 Value { get { return (uint)(_r | (_g << 8) | (_b << 16)); } }
    }
}
