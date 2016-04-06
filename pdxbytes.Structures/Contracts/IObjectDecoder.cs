using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures.Contracts
{
    public interface IObjectDecoder
    {
        void Decode(Array items, int count);
        void Decode(Array items, int startIndex, int count);
        event OnObjectDecoded Decoded;
    }
    public delegate void OnObjectDecoded(object sender, object data);
}
