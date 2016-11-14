using System;
using Microsoft.SPOT;
using pdxbytes.Structures;
using System.Collections;

namespace pdxbytes.Encoders
{
    public class RGB565Encoder : ColorEncoder
    {
        public RGB565Encoder()
        {
            ColorKeys = new Hashtable();
        }
        public Hashtable ColorKeys;
        public override ushort[] Encode(UInt24Collection data)
        {
            var result = new ushort[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                var current = data[i];
                var hashcode = current.GetHashCode();
                if (ColorKeys.Contains(hashcode))
                {
                    result[i] = (ushort)ColorKeys[hashcode];
                    continue;
                }
                var red = ((current.R & 0xF8) << 8);
                var green = ((current.G & 0xFC) << 3);
                var blue = (current.B >> 3);
                result[i] = (ushort)(red | green | blue);
                ColorKeys[hashcode] = result[i];
            }
            return result;
        }
    }
}
