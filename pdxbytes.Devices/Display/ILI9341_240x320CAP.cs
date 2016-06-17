using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using pdxbytes.Devices.Core;
using System.Threading;
using pdxbytes.DeviceInterfaces;

namespace pdxbytes.Devices.Display
{
    public enum DelayTimes : int
    {
        None = 0,
        Short = 10,
        Mid = 50,
        Long = 150
    }
    public class ILI9341_240x320CAP : BaseTFT, IDisplay
    {
        public const short WIDTH = 240;
        public const short HEIGHT = 320;
        public override short Width { get { return WIDTH; } }
        public override short Height { get { return HEIGHT; } }

        public enum LcdCommand
        {
            NOP = 0x0,
            SWRESET = 0x01,
            RDDID = 0x04,
            RDDST = 0x09,
            SLPIN = 0x10,
            SLPOUT = 0x11,
            PTLON = 0x12,
            NORON = 0x13,
            INVOFF = 0x20,
            INVON = 0x21,
            //Gamma curve selected 
            GAMMASET = 0x26,
            DISPOFF = 0x28,
            DISPON = 0x29,
            CASET = 0x2A,
            RASET = 0x2B,
            PASET = 0x2B,
            RAMWR = 0x2C,
            RAMRD = 0x2E,
            COLMOD = 0x3A,
            PIXFMT = 0x3A,
            MADCTL = 0x36,
            FRMCTR1 = 0xB1,
            FRMCTR2 = 0xB2,
            FRMCTR3 = 0xB3,
            INVCTR = 0xB4,
            DISSET5 = 0xB6,
            DFUNCTR = 0xB6,
            PWCTR1 = 0xC0,
            PWCTR2 = 0xC1,
            PWCTR3 = 0xC2,
            PWCTR4 = 0xC3,
            PWCTR5 = 0xC4,
            VMCTR1 = 0xC5,
            VMCTR2 = 0xC7,
            RDID1 = 0xDA,
            RDID2 = 0xDB,
            RDID3 = 0xDC,
            RDID4 = 0xDD,
            PWCTR6 = 0xFC,
            GMCTRP1 = 0xE0,
            GMCTRN1 = 0xE1
        }

        /// <summary>
        /// constructs and initializes the capacitive touch screen ILI9341
        /// </summary>
        /// <param name="cs">Chip select pin for the SPI interface to the screen</param>
        /// <param name="dc">Data command Ouput port to the screen</param>
        /// <param name="reset">Optional reset pin to the screen</param>
        /// <param name="spiModule">Spi module for the touchscreen</param>
        /// <param name="speedKHz">Speend in Khz driving the SPI interface</param>
        /// <param name="capacitance">The FT6206CAPI2C chip does not like to run out of the box. In software we toggle a pwm channel to the chip during initialization. This may not be needed but is here in case the chip behaves the same</param>
        public ILI9341_240x320CAP(Cpu.Pin cs, Cpu.Pin dc, Cpu.Pin reset, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)40000)
        {
            DataCommandPort = new OutputPort(dc, false);
            //ResetPort = new OutputPort(reset, true);

            this.NormalState = new ExtendedSpiConfiguration(
                SPI_mod: spiModule,
                ChipSelect_Port: cs,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: speedKHz,
                BitsPerTransfer: 8);

            Spi = new SharedSPI(this.NormalState);
            
        }

        private ExtendedSpiConfiguration NormalState;

        
        #region IGraphicDevice members

        public override void BeginDraw(short x, short y, short width, short height)
        {
            this.SetAddressWindow(x, y, (short)(x + width - 1), (short)(y + height - 1));
            DataCommandPort.Write(Data);
        }

        public override void WriteBuffer(byte[] buffer)
        {
            Spi.Write(buffer);
        }

        public void DrawData(short x, short y, short width, short height, byte[] buffer)
        {
            this.BeginDraw(x, y, width, height);
            this.WriteBuffer(buffer);
        }

        public override int BufferSize { get { return this.Width * 128; } }

        public override int Stride { get { return this.Width; } }

        #endregion

        #region ITFT implementation

        public override void Dispose()
        {
            Spi.Dispose();
            Spi = null;
            DataCommandPort = null;
            ResetPort = null;
        }

        public override void Reset()
        {
            ResetPort.Write(true);
            Thread.Sleep(5);
            ResetPort.Write(false);
            Thread.Sleep(20);
            ResetPort.Write(true);
            Thread.Sleep(150);
        }

        public override void Sleep()
        {
            //??
        }

        public override void Wake()
        {
            WriteCommand((byte)LcdCommand.SLPOUT, DelayTimes.Long);  // out of sleep mode
        }

        public override void DisplayOn()
        {
            WriteCommand((byte)LcdCommand.DISPON, DelayTimes.Short);  // display on
        }

        public override byte ReadCommand(byte command)
        {
            byte[] result = new byte[1];
            Spi.WriteRead(new byte[] { 0xD9, 0x10, command }, result);
            return result[0];
            // need to set the chip select low and high manually, not sure if we can just create an output on the cs pin when the 
            // spi interface has already been initiated.

        }

        #endregion

        #region Initialization

        protected void InitializeFrameRateControl()
        {
            WriteCommand((byte)LcdCommand.FRMCTR1, DelayTimes.None, 0x01, 0x2C, 0x2D);
            WriteCommand((byte)LcdCommand.FRMCTR2, DelayTimes.None, 0x01, 0x2C, 0x2D); 
            WriteCommand((byte)LcdCommand.FRMCTR3, DelayTimes.None, 0x01, 0x2C, 0x2D, 0x01, 0x2C, 0x2D);
        }

        protected void InitializeUnknownCommands()
        {
            this.WriteCommand(0xEF, DelayTimes.None, 0x80, 0x02);
            this.WriteCommand(0xCF, DelayTimes.None, 0x00, 0xC1, 0x30);
            this.WriteCommand(0xED, DelayTimes.None, 0x64, 0x03, 0x12, 0x81);
            this.WriteCommand(0xE8, DelayTimes.None, 0x85, 0x00, 0x78);
            this.WriteCommand(0xCB, DelayTimes.None, 0x39, 0x2c, 0x00, 0x34, 0x02);
            this.WriteCommand(0xF7, DelayTimes.None, 0x20);
            this.WriteCommand(0xEA, DelayTimes.None, 0x00, 0x00);
        }

        protected void InitializePowerControl()
        {
            this.WriteCommand((byte)LcdCommand.PWCTR1, DelayTimes.None, 0x23);
            this.WriteCommand((byte)LcdCommand.PWCTR2, DelayTimes.None, 0x10);
        }

        protected void InitializeVMControl()
        {
            this.WriteCommand((byte)LcdCommand.VMCTR1, DelayTimes.None, 0x3e, 0x28);
            this.WriteCommand((byte)LcdCommand.VMCTR2, DelayTimes.None, 0x86);
        }

        protected void InitializeMemoryAccessControl()
        {
            this.WriteCommand((byte)LcdCommand.MADCTL, DelayTimes.None, 0x48);
            this.WriteCommand((byte)LcdCommand.PIXFMT, DelayTimes.None, 0x55);
            this.WriteCommand((byte)LcdCommand.FRMCTR1, DelayTimes.None, 0x00, 0x18);

        }

        protected void InitializeDisplayFunctionControl()
        {
            this.WriteCommand((byte)LcdCommand.DFUNCTR, DelayTimes.None, 0x08, 0x82, 0x27);
        }

        protected void InitializeGamma()
        {
            this.WriteCommand(0xF2, DelayTimes.None, 0x00);// 3Gamma Function Disable 
            this.WriteCommand((byte)LcdCommand.GAMMASET, DelayTimes.None, 0x01);//Gamma curve selected 
            this.WriteCommand((byte)LcdCommand.GMCTRP1, DelayTimes.None, 0x0F, 0x31, 0x2B, 0x0C, 0x0E, 0x08, 0x4E, 0xF1, 0x37, 0x07, 0x10, 0x03, 0x0E, 0x09, 0x00);
            this.WriteCommand((byte)LcdCommand.GMCTRN1, DelayTimes.None, 0x00, 0x0E, 0x14, 0x03, 0x11, 0x07, 0x31, 0xC1, 0x48, 0x08, 0x0F, 0x0C, 0x31, 0x36, 0x0F);

        }

        public override void Initialize()
        {

            this.InitializeUnknownCommands();

            this.InitializePowerControl();

            this.InitializeVMControl();

            this.InitializeMemoryAccessControl();

            this.InitializeDisplayFunctionControl();

            this.Wake(); //Exit Sleep 

            this.DisplayOn(); //Display on 

            
        }

        #endregion
        

        #region private/protected interface 

        private void SetAddressWindow(short x0, short y0, short x1, short y1)
        {
            // col address set, x start, x end
            this.WriteCommand((byte)LcdCommand.CASET, DelayTimes.None, (byte)(x0 >> 8), (byte)(x0), (byte)(x1 >> 8), (byte)(x1));
            // row address set: y start, y end
            this.WriteCommand((byte)LcdCommand.RASET, DelayTimes.None, (byte)(y0 >> 8), (byte)(y0), (byte)(y1 >> 8), (byte)(y1));
            // write to ram
            this.WriteCommand((byte)LcdCommand.RAMWR);
        }

        private void SetPixel(int x, int y, ushort color)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height)) return;
            var index = ((y * Width) + x);
        }


        protected void WriteCommand(byte cmd, DelayTimes waitAfterCommand = DelayTimes.None, params byte[] data)
        {
            DataCommandPort.Write(Command);
            Write(cmd);
            if (waitAfterCommand != DelayTimes.None)
                Thread.Sleep((int)waitAfterCommand);
            if (null != data)
            {
                DataCommandPort.Write(Data);
                for (var i = 0; i < data.Length; i++)
                {
                    Write(data[i]);
                }
            }
        }

        protected void Write(byte bit8)
        {

            SpiBOneByteBuffer[0] = bit8;
            Spi.Write(SpiBOneByteBuffer);
        }


        private const bool Data = true;
        private const bool Command = false;
        protected readonly byte[] SpiBOneByteBuffer = new byte[1];
        protected OutputPort DataCommandPort;
        protected OutputPort ResetPort;
        protected ISPI Spi;

        #endregion
    }
}
