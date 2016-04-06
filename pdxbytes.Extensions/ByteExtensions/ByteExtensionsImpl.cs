using System;
using Microsoft.SPOT;

namespace pdxbytes.Extensions.ByteExtensions
{
    public static class ByteExtensionsImpl
    {
        //public static byte[] ToByteArray(this bool[] bits)
        //{
         
        //    var bytes = new byte[bits.Length / 8];
        //    //bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));
        //    for (int i = 0, j = 0; j < bits.Length; i++, j += 8)
        //    {
        //        // Create byte from bits where LSB is read first.
        //        for (int offset = 0; offset < 8; offset++)
        //            bytes[i] |= (byte)((bits[j + offset] ? 1 : 0) << (7 - offset));
        //    }

        //    return bytes;
        //}

        public static byte[] ToByteArray(this bool[] input)
        {
            if (input.Length % 8 != 0)
            {
                throw new ArgumentException("input");
            }
            byte[] ret = new byte[input.Length / 8];
            for (int i = 0; i < input.Length; i += 8)
            {
                int value = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (input[i + j])
                    {
                        value += 1 << (7 - j);
                    }
                }
                ret[i / 8] = (byte)value;
            }
            return ret;
        }

        public static string ToDisplayString(this bool[] input)
        {
            string ret = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (i % 8 == 0)
                    ret += " ";
                ret += (input[i] ? "1" : "0");
            }
            return ret;
        }
    }
}
