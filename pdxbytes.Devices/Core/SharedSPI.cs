using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace pdxbytes.Devices.Core
{
    public class SharedSPI : ISPI
    {
        private static int SPIHandles;
        private static SPI Device;
        private static object writelock = new object();
        public SharedSPI(SPI.Configuration config)
        {
            this.ChipSelect = new OutputPort(config.ChipSelect_Port, !config.ChipSelect_ActiveState);
            this.ActiveState = config.ChipSelect_ActiveState;
            this.Config = new SPI.Configuration(Cpu.Pin.GPIO_NONE,
                    config.BusyPin_ActiveState,
                    config.ChipSelect_SetupTime,
                    config.ChipSelect_HoldTime,
                    config.Clock_IdleState,
                    config.Clock_Edge,
                    config.Clock_RateKHz,
                    config.SPI_mod,
                    config.BusyPin,
                    config.BusyPin_ActiveState);
            
            if (null == Device)
                Device = new SPI(this.Config);
            Interlocked.Increment(ref SPIHandles);
        }

        private SPI.Configuration Config;
        private OutputPort ChipSelect;
        private bool ActiveState;

        #region ISPI members 

        public void Write(ushort[] writeBuffer)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.Write(writeBuffer);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void Write(byte[] writeBuffer)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.Write(writeBuffer);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void WriteRead(ushort[] writeBuffer, ushort[] readBuffer)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.WriteRead(writeBuffer, readBuffer);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void WriteRead(byte[] writeBuffer, byte[] readBuffer)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.WriteRead(writeBuffer, readBuffer);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void WriteRead(ushort[] writeBuffer, ushort[] readBuffer, int startReadOffset)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.WriteRead(writeBuffer, readBuffer, startReadOffset);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void WriteRead(byte[] writeBuffer, byte[] readBuffer, int startReadOffset)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.WriteRead(writeBuffer, readBuffer, startReadOffset);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void WriteRead(ushort[] writeBuffer, int writeOffset, int writeCount, ushort[] readBuffer, int readOffset, int readCount, int startReadOffset)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadOffset);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void WriteRead(byte[] writeBuffer, int writeOffset, int writeCount, byte[] readBuffer, int readOffset, int readCount, int startReadOffset)
        {
            lock (writelock)
            {
                this.ChipSelect.Write(this.ActiveState);
                Device.Config = this.Config;
                Device.WriteRead(writeBuffer, writeOffset, writeCount, readBuffer, readOffset, readCount, startReadOffset);
                this.ChipSelect.Write(!this.ActiveState);
            }
        }

        public void Dispose()
        {
            Interlocked.Decrement(ref SPIHandles);
            if(0 == SPIHandles)
            {
                Device.Dispose();
                Device = null;
            }
        }

        #endregion
    }
}
