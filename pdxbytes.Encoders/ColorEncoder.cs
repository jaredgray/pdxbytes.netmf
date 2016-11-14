using System;
using Microsoft.SPOT;
using pdxbytes.Structures;

namespace pdxbytes.Encoders
{
    public abstract class ColorEncoder
    {
        public abstract ushort[] Encode(UInt24Collection data);
    }
}
