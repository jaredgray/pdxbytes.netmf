using System;
using Microsoft.SPOT;
using pdxbytes.Extensions.MathExtensions;


namespace pdxbytes.Structures
{

    public enum Colors
    {
        Black = 0x0000,
        Blue = 0x001F,
        Red = 0xF800,
        Green = 0x07E0,
        Cyan = 0x07FF,
        Magenta = 0xF81F,
        Yellow = 0xFFE0,
        White = 0xFFFF
    }

    public class Color
    {
        #region ctor

        public Color(byte r, byte g, byte b)
        {
            _value = new UInt24(r, g, b);
            this.SetColor();
        }

        /// <summary>
        /// Constructs a Color from a hex string.
        /// </summary>
        /// <param name="hex">the hex value to generate the rgb color structure. The input can include "0x" or "#" and will eliminate alpha channel if passed since the Color structure does not support alpha</param>
        public Color(string hex)
        {
            hex = hex.TrimStart('#');
            if (hex.IndexOf("0x") == 0)
                hex = hex.Substring(2);
            // not supporting alpha
            if (hex.Length == 8)
                hex = hex.Substring(2);
            
            this._value = new UInt24(hex.Substring(0, 2).ByteFromHexString(), hex.Substring(2, 2).ByteFromHexString(), hex.Substring(4, 2).ByteFromHexString());
            this.SetColor();
        }
        
        

        #endregion

        #region data

        public byte R
        {
            get { return _value.R; }
            set
            {
                if (value != _value.R)
                {
                    _value.R = value;
                }
            }
        }
        public byte G
        {
            get { return _value.G; }
            set
            {
                if (value != _value.G)
                {
                    _value.G = value;
                }
            }
        }
        public byte B
        {
            get { return _value.B; }
            set
            {
                if (value != _value.B)
                {
                    _value.B = value;
                }
            }
        }

        public UInt24 Value { get { return _value; } }
        private UInt24 _value;

        #endregion

        public static Color Transparent = new Color(1,2,3);

        public ushort[] Debug_GetRGB16BitValue()
        {
            var red = ((R & 0xF8) << 8);
            var green = ((G & 0xFC) << 3);
            var blue = (B >> 3);
            return new ushort[] { (ushort)red, (ushort)green, (ushort)blue };
        }
        public byte[] Debug_GetRGB16BitValueFromValue()
        {
            var value = To16BitValue();
            var resultcolor = From16BitValue(value);
            return new byte[] { resultcolor.R, resultcolor.G, resultcolor.B };
        }

        public ushort To16BitValue()
        {
            return To16BitValue(R, G, B);
        }

        /// <summary>
        /// Generates a RGB565 value from a 3 part rgb input - RGB888ToRGB565
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ushort To16BitValue(byte r, byte g, byte b)
        {
            /*
             * (ushort)(((R & 0xF8) << 8) | ((G & 0xFC) << 3) | (B >> 3));
             */
            var red = ((r & 0xF8) << 8);
            var green = ((g & 0xFC) << 3);
            var blue = (b >> 3);
            return (ushort)(red | green | blue);
        }

        public static uint To32BitValue(byte r, byte g, byte b)
        {
            return (uint)((r << 16) | (g << 8) | b);
        }

        /// <summary>
        /// Color generated from a 16 bit value - this method does a conversion from RGB565ToRGB888
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color From16BitValue(ushort value)
        {
            byte r = (byte)(((value) & 0xF800) >> 8);
            byte g = (byte)(((value) & 0x07E0) >> 3);
            byte b = (byte)(((value) & 0x001F) << 3);
            return new Color(r, g, b);
        }
        /* 

         public static Color From16BitValue(ushort value)
         {
             byte r = (byte)(((value) >> 11) << 3);
             byte g = (byte)((value) << 5);
             byte b = (byte)(((value) >> 5) << 2);
             return new Color(r, g, b);
         }


         public static Color From16BitValue(ushort value) // origional function
         {
             byte r = (byte)(((value) & 0xF800) >> 8);
             byte g = (byte)(((value) & 0x07E0) >> 3);
             byte b = (byte)(((value) & 0x001F) << 3);
             return new Color(r, g, b);
         }
         */

        private void SetColor()
        {
            //_value = To16BitValue(R, G, B);
            //_value = (ushort)(((R & 0xF8) << 8) | ((G & 0xFC) << 3) | (B >> 3));
        }

        public void SetPoint()
        {
            //_r = _R;
            //_g = _G;
            //_b = _B;
        }
        public void Lighten(byte amount)
        {
            _value.R += amount;
            _value.G += amount;
            _value.B += amount;
        }
        public void Reset()
        {
        }
        
        static ushort Rgb(byte r, byte g, byte b)
        {
            return (ushort)(((r & 0xF8) << 8) | ((g & 0xFC) << 3) | (b >> 3));
        }

        public override string ToString()
        {
            return R + ", " + G + ", " + B;
        }
    }
}
