using System;
using Microsoft.SPOT;
using pdxbytes.Devices.Core;
using Microsoft.SPOT.Hardware;
using pdxbytes.Structures;
using pdxbytes.DeviceInterfaces;
using System.Text;
using System.Collections;
using System.Threading;

namespace pdxbytes.Devices.Display
{
    public class FT6206CAPI2C : I2CPlug, IDevice, ITouchInterface
    {
        const byte FT6206_ADDR = 0x38;

        const byte FT6206_G_FT5201ID = 0xA8;
        const byte FT6206_REG_NUMTOUCHES = 0x02;

        const byte FT6206_NUM_X = 0x33;
        const byte FT6206_NUM_Y = 0x34;

        const byte FT6206_REG_MODE = 0x00;
        const byte FT6206_REG_GESTID = 0x01;
        const byte FT6206_REG_CALIBRATE = 0x02;
        const byte FT6206_REG_WORKMODE = 0x00;
        const byte FT6206_REG_FACTORYMODE = 0x40;
        const byte FT6206_REG_THRESHHOLD = 0x80;
        const byte FT6206_REG_POINTRATE = 0x88;
        const byte FT6206_REG_FIRMVERS = 0xA6;
        const byte FT6206_REG_CHIPID = 0xA3;
        const byte FT6206_REG_VENDID = 0xA8;
        const byte FT6206_REG_CTRL = 0x86; // 0x00 == Active, 0x01 = Switching from Active to Monitor
        const byte FT6206_REG_ACTIVE = 0x88; // not sure what default is.. and i think this is hz
        const byte FT6206_REG_GMODE = 0xA4;

        // calibrated for Adafruit 2.8" ctp screen
        const byte FT6206_DEFAULT_THRESSHOLD = 1; 
        /// <summary>
        /// initializes a new instance of the FT6206 Capacitive touch sensor
        /// </summary>
        /// <param name="address">physical address space of the i2c device</param>
        /// <param name="irq">the irq interrupt pin on the tft screen</param>
        public FT6206CAPI2C(Cpu.Pin irq, Cpu.PWMChannel capacitance = Cpu.PWMChannel.PWM_NONE) : base(FT6206_ADDR, 400)
            // Hardcode Note: the clock pegs somewhere around 400 on the i2c, it has been defaulted to this rate.
        {

            this.Input = new InterruptPort(irq, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeHigh);
            this.Input.OnInterrupt += new NativeEventHandler(Input_OnInterrupt);
            //this.ReadThread = new Timer(Process, null, Timeout.Infinite, Timeout.Infinite);
            if (capacitance != Cpu.PWMChannel.PWM_NONE)
                CapPort = new CapacitancePort(capacitance);
        }

        public event TouchDelegate Touched;

        private Timer ReadThread;

        public CapacitancePort CapPort { get; set; }

        public void Initialize()
        {
            Debug.Print("Initialize");
            //base.WriteToRegister(FT6206_ADDR, 0x00);
            base.WriteToRegister(FT6206_REG_THRESHHOLD, FT6206_DEFAULT_THRESSHOLD, false);
            //base.WriteToRegister(FT6206_REG_CTRL, 0x01, false);
            //this.Input.EnableInterrupt();
            //0x1E == 30
            //0x3C == 60
            //0x64 == 100
            base.WriteToRegister(FT6206_REG_ACTIVE, 0x1E, false);
            base.WriteToRegister(FT6206_REG_GMODE, 0x01, false);

            //var activeval = base.ReadRegisterByte(FT6206_REG_ACTIVE, false);
            ////var gmode = base.ReadRegisterByte(FT6206_REG_GMODE, false);
            ////Debug.Print("Active: " + ((int)activeval).ToString() + ", G_MODE: " + gmode.ToString());
            ////int vid = 0, pid = 0;
            var vid = base.ReadRegisterByte(FT6206_REG_VENDID, false);
            var pid = base.ReadRegisterByte(FT6206_REG_CHIPID, false);
            if (null != CapPort)
                this.CapPort.Off();
            if ((vid != 17) || (pid != 6))
            {
                Debug.Print("FAULT reading vendor/chip id " + vid + "/" + pid);
            }
            else
            {
                this.IsConnected = true;
                Debug.Print("vid: " + vid + " pid: " + pid);
            }
        }
        public DateTime LastTap = DateTime.MinValue;
        private InterruptPort Input { get; set; }
        private void Input_OnInterrupt(uint port, uint data, DateTime time)
        {
            //Debug.Print("interrupt");
            //this.ReadThread.Change(20, Timeout.Infinite);
            this.Read(null);
        }
        int readcount = 0;
        const int read_threshold = 50;
        string[] lines = new string[read_threshold];
        byte[] i2cdat = new byte[16];


        private void Read(object ctx)
        {
            //base.WriteToRegister(FT6206_ADDR, 0x00);
            i2cdat = new byte[16];
            //Debug.Print("Reading");
            base.ReadFromRegister(FT6206_ADDR, i2cdat);
            //this.Input.ClearInterrupt();
            //this.ReadThread.Change(0, Timeout.Infinite);
            Process(null);

            //if (readcount == read_threshold)
            //{
            //    for(int i = read_threshold - 1; i > -1; --i)
            //    {
            //        Debug.Print(lines[i]);
            //    }
            //    readcount = 0;
            //}

            //StringBuilder sb = new StringBuilder();
            //foreach(var byt in i2cdat)
            //{
            //    sb.Append(byt.ToString() + "|");
            //}
            //lines[readcount] = sb.ToString();
            //++readcount;

        }

        private void Process(object ctx)
        {
            var touches = i2cdat[2];

            if (touches == 0 || touches > 2)
            {
                //Debug.Print("Waa Waaaaa....");
                return;
            }

            int[] touchX = new int[2], touchY = new int[2], touchID = new int[2];

            int x1 = touchX[0], y1 = touchY[0], x2 = touchX[1], y2 = touchY[1];
            var t1 = new Touch(InterpretTouch(i2cdat, 0), InterpretTouch(i2cdat, 1));
            this.OnTouch(t1);
        }

        private Vec2 InterpretTouch(byte[] data, int offset)
        {
            int x, y;
            x = data[3 + offset * 6] & 0x0F;
            x <<= 8;
            x |= data[4 + offset * 6];

            y = data[5 + offset * 6] & 0x0F;
            y <<= 8;
            y |= data[6 + offset * 6];

            //touchID[i] = i2cdat[0x05 + i * 6] >> 4;

            return new Vec2(x, y);
        }
        
        protected void OnTouch(Touch t)
        {
            var handler = this.Touched;
            if (null != handler)
                handler(t);
        }

        #region public interface 

        public bool IsConnected { get; set; }

        public Orientations NaturalOrientation
        {
            get
            {
                return Orientations.Vertical_TopUp;
            }
        }

        public CoordinateSystems CoordinateSystem
        {
            get
            {
                return CoordinateSystems.BottomRight;
            }
        }

        public Vec2 GetTouchPoint()
        {
            base.WriteToRegister(FT6206_ADDR, 0);

            byte[] i2cdat = new byte[16];
            base.ReadFromRegister(FT6206_ADDR, i2cdat);

            var touches = i2cdat[0x02];
            //int x = 0, y = 0;

            if (touches == 0 || touches > 2)
            {
                return new Vec2(0, 0);
            }

            /*
            if (touches == 2) Serial.print('2');
            for (uint8_t i=0; i<16; i++) {
             // Serial.print("0x"); Serial.print(i2cdat[i], HEX); Serial.print(" ");
            }
            */

            /*
            Serial.println();
            if (i2cdat[0x01] != 0x00) {
              Serial.print("Gesture #"); 
              Serial.println(i2cdat[0x01]);
            }
            */

            //Serial.print("# Touches: "); Serial.print(touches);
            int[] touchX = new int[2], touchY = new int[2], touchID = new int[2];
            for (byte i = 0; i < 2; i++)
            {
                touchX[i] = i2cdat[0x03 + i * 6] & 0x0F;
                touchX[i] <<= 8;
                touchX[i] |= i2cdat[0x04 + i * 6];
                touchY[i] = i2cdat[0x05 + i * 6] & 0x0F;
                touchY[i] <<= 8;
                touchY[i] |= i2cdat[0x06 + i * 6];
                touchID[i] = i2cdat[0x05 + i * 6] >> 4;
            }
            return new Vec2(touchX[0], touchY[0]);
        }

        public bool IsTouched()
        {
            var result = base.ReadRegisterByte(FT6206_REG_NUMTOUCHES);
            return ((result == 1) || (result == 2));
        }

        #endregion
    }
}
