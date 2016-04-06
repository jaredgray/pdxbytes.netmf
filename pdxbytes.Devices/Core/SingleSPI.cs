using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace pdxbytes.Devices.Core
{
    public class SingleSPI : ISPI
    {
        public SingleSPI(SPI.Configuration config)
        {
            Device = new SPI(config);
        }

        private SPI Device;

        public void Write(ushort[] writeBuffer)
        {
            Device.Write(writeBuffer);
        }
        public void Write(byte[] writeBuffer)
        {
            Device.Write(writeBuffer);
        }

        public void WriteRead(ushort[] writeBuffer, ushort[] readBuffer)
        {
            Device.WriteRead(writeBuffer, readBuffer);
        }

        public void WriteRead(byte[] writeBuffer, byte[] readBuffer)
        {
            Device.WriteRead(writeBuffer, readBuffer);
        }

        public void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, int startReadOffset)
        {
            Device.WriteRead(writeBuffer, readBuffer, startReadOffset);
        }

        public void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadOffset)
        {
            Device.WriteRead(writeBuffer, readBuffer, startReadOffset);
        }

        public void WriteRead(ushort[] writeBuffer, int writeOffset, int writeCount, ushort[] readBuffer, int readOffset, int readCount, int startReadOffset)
        {
            Device.WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadOffset);
        }

        public void WriteRead(byte[] writeBuffer, int writeOffset, int writeCount, byte[] readBuffer, int readOffset, int readCount, int startReadOffset)
        {
            Device.WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadOffset);
        }

        public void Dispose()
        {
            Device.Dispose();
            Device = null;
        }

    }
}
