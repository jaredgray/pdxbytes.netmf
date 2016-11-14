using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    [Flags]
    public enum Edges
    {
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8
    }
}
