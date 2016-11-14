using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public struct RelativeRect
    {
        public short Width { get; set; }
        public short Height { get; set; }

        public Vec216 RelativeUIPosition { get; set; }

    }
}
