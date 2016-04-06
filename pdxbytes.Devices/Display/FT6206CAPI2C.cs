using System;
using Microsoft.SPOT;
using pdxbytes.Devices.Core;
using Microsoft.SPOT.Hardware;
using pdxbytes.Structures;
using pdxbytes.DeviceInterfaces;

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
        const byte FT6206_REG_CALIBRATE = 0x02;
        const byte FT6206_REG_WORKMODE = 0x00;
        const byte FT6206_REG_FACTORYMODE = 0x40;
        const byte FT6206_REG_THRESHHOLD = 0x80;
        const byte FT6206_REG_POINTRATE = 0x88;
        const byte FT6206_REG_FIRMVERS = 0xA6;
        const byte FT6206_REG_CHIPID = 0xA3;
        const byte FT6206_REG_VENDID = 0xA8;

        // calibrated for Adafruit 2.8" ctp screen
        const byte FT6206_DEFAULT_THRESSHOLD = 10; 
        /// <summary>
        /// initializes a new instance of the FT6206 Capacitive touch sensor
        /// </summary>
        /// <param name="address">physical address space of the i2c device</param>
        /// <param name="irq">the irq interrupt pin on the tft screen</param>
        public FT6206CAPI2C(Cpu.Pin irq, Cpu.PWMChannel capacitance = Cpu.PWMChannel.PWM_NONE) : base(FT6206_ADDR, 400)
            // Hardcode Note: the clock pegs somewhere around 400 on the i2c, it has been defaulted to this rate.
        {

            this.Input = new InterruptPort(irq, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
            this.Input.OnInterrupt += new NativeEventHandler(Input_OnInterrupt);

            this.DebounceTime = new TimeSpan(10 * 50);
            if (capacitance != Cpu.PWMChannel.PWM_NONE)
                CapPort = new CapacitancePort(capacitance);
        }

        public event TouchDelegate Touched;

        public CapacitancePort CapPort { get; set; }

        public void Initialize()
        {
            //base.WriteToRegister(FT6206_ADDR, 0x00);
            base.WriteToRegister(FT6206_REG_THRESHHOLD, FT6206_DEFAULT_THRESSHOLD, false);
            //int vid = 0, pid = 0;
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
        public TimeSpan DebounceTime { get; set; }
        public DateTime LastTap = DateTime.MinValue;
        private InterruptPort Input { get; set; }
        private void Input_OnInterrupt(uint port, uint data, DateTime time)
        {
            this.Read();
        }
        private void Read()
        {
            base.WriteToRegister(FT6206_ADDR, 0x00);
            byte[] i2cdat = new byte[16];
            base.ReadFromRegister(FT6206_ADDR, i2cdat);
            var touches = i2cdat[0x02];

            if (touches == 0 || touches > 2)
            {
                Debug.Print("Waa Waaaaa....");
                return;
            }
            
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
            this.OnTouch(new Touch(new Vec2(touchX[0], touchY[0]), new Vec2(touchX[1], touchY[1])));
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
