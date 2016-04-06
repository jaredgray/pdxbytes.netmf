using System;
using Microsoft.SPOT;
using pdxbytes.Structures.Contracts;

namespace pdxbytes.Structures
{
    public struct Pulse : IPulse
    {
        public bool State { get; set; }
        public long Timestamp { get; set; }
    }
}
