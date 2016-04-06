using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures.Contracts
{
    public interface IPulse : ITimeStructure
    {
        bool State { get; set; }
    }
}
