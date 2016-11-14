using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public class PixelDataBuffer
    {
        public PixelDataBuffer(Size size, UInt24Collection data, int bitdepth)
        {
            this._data = data;
            this._size = size;
            this._bitdepth = bitdepth;
            this.Stride = size.Width * bitdepth;
        }
        private UInt24Collection _data;
        private Size _size;
        private int _bitdepth;

        public int Stride { get; private set; }

        //public void SetPixel(int x, int y, Color color)
        //{
        //    var index = y * this._size.Width + x;
        //    this._data[index] = color.Value;
        //}
        //public static void SetPixel(int x, int y, int width, int bitdepth, ushort color, ushort[] buffer)
        //{
        //    //TODO: fix index
        //    var index = y * width + x;
        //    buffer[index] = color;
        //}

        public Color GetColor(int x, int y)
        {
            //TODO: fix index
            var index = y * this._size.Width + x;
            //var data = _data[index]; // this[index];
            //return new Color(data.R, data.G, data.B);
            return Palette.Black;
        }
        //public UInt24 this[int index]
        //{
        //    get
        //    {
        //        return this._data[index];
        //        //var b0 = this._data[index * _bitdepth];
        //        //var b1 = _data[index * _bitdepth + 1];
        //        //return (ushort)((b1 << 8) | b0);
        //    }
        //    set
        //    {
        //        this._data[index] = value;
        //        //_data[index * _bitdepth] = (byte)(value >> 8);
        //        //_data[index * _bitdepth + 1] = (byte)value;
        //    }
        //}
    }
}
