using System;
using Microsoft.SPOT;

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

        public Color(byte r, byte g, byte b)
        {
            this._R = r;
            this._G = g;
            this.B = b;
        }
        public static Color Transparent = new Color(1,2,3);
        public static ushort Rgb(byte r, byte g, byte b)
        {
            return (ushort)(((r & 0xF8) << 8) | ((g & 0xFC) << 3) | (b >> 3));
        }

        public byte R
        {
            get { return _R; }
            set
            {
                if (value != _R)
                {
                    _R = value;
                    this.SetColor();
                }
            }
        }
        private byte _R;
        public byte G
        {
            get { return _G; }
            set
            {
                if (value != _G)
                {
                    _G = value;
                    this.SetColor();
                }
            }
        }
        private byte _G;
        public byte B
        {
            get { return _B; }
            set
            {
                if (value != _B)
                {
                    _B = value;
                    this.SetColor();
                }
            }
        }
        private byte _B;

        public ushort Value { get { return _value; } }
        private ushort _value;
        
        private void SetColor()
        {
            _value = (ushort)(((R & 0xF8) << 8) | ((G & 0xFC) << 3) | (B >> 3));
        }
    }
}
