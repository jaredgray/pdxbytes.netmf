using System;
using Microsoft.SPOT;

namespace pdxbytes.Devices.Core
{
    public interface ISPI : IDisposable
    {

        void Write(byte[] writeBuffer);
        void Write(ushort[] writeBuffer);
        void WriteRead(byte[] writeBuffer, byte[] readBuffer);
        void WriteRead(ushort[] writeBuffer, ushort[] readBuffer);
        void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadOffset);
        void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, int startReadOffset);
        void WriteRead(byte[] writeBuffer, int writeOffset, int writeCount, byte[] readBuffer, int readOffset, int readCount, int startReadOffset);
        void WriteRead(ushort[] writeBuffer, int writeOffset, int writeCount, ushort[] readBuffer, int readOffset, int readCount, int startReadOffset);
    }
}
