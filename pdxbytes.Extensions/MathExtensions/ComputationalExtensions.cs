using System;
using Microsoft.SPOT;

namespace pdxbytes.Extensions.MathExtensions
{
    public static class ComputationalExtensions
    {
        public static int Round(this int i, int nearest)
        {
            if (nearest <= 0 || nearest % 10 != 0)
                throw new ArgumentOutOfRangeException("nearest", "Must round to a positive multiple of 10");

            return (i + 5 * nearest / 10) / nearest * nearest;
        }
        public static long Round(this long i, int nearest)
        {
            if (nearest <= 0 || nearest % 10 != 0)
                throw new ArgumentOutOfRangeException("nearest", "Must round to a positive multiple of 10");

            return (i + 5 * nearest / 10) / nearest * nearest;
        }

        /// <summary>
        /// Converts a Hex string (2 character) to a number
        /// </summary>
        /// <param name="hexNumber">The Hex string (ex.: "0F")</param>
        /// <returns>The decimal value</returns>
        public static byte ByteFromHexString(this string hexNumber)
        {
            // Always in upper case
            hexNumber = hexNumber.ToUpper();
            // Contains all Hex posibilities
            string ConversionTable = "0123456789ABCDEF";
            // Will contain the return value
            byte RetVal = 0;
            // Will increase
            byte Multiplier = 1;

            for (int Index = hexNumber.Length - 1; Index >= 0; --Index)
            {
                RetVal += (byte)(Multiplier * (ConversionTable.IndexOf(hexNumber[Index])));
                Multiplier = (byte)(Multiplier * ConversionTable.Length);
            }

            return RetVal;
        }
    }
}
