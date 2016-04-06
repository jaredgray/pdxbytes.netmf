using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures.Contracts
{
    public interface ITimeStructure
    {
        long Timestamp { get; set; }
    }
}
