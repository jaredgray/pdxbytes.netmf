using System;
using Microsoft.SPOT;

namespace Extensions.ByteExtensions
{
    public static class ByteExtensionsImpl
    {
        public static byte[] ToByteArray(this bool[] bits)
        {
            var bytes = new byte[bits.Length / 8];
            //bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));
            for (int i = 0, j = 0; j < bits.Length; i++, j += 8)
            {
                // Create byte from bits where LSB is read first.
                for (int offset = 0; offset < 8; offset++)
                    bytes[i] |= (byte)((bits[j + offset] ? 1 : 0) << (7 - offset));
            }

            return bytes;
        }
    }
}
