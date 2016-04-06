using System;
using Microsoft.SPOT;

namespace pdxbytes.Devices
{
    internal static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return null == value || value == "";
        }
    }
}
