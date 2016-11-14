
using pdxbytes.Structures;
using System;

namespace pdxbytes.Graphics
{
    public abstract class DisplayBuffer : IDisposable
    {
        public DisplayBuffer() { }
        public DisplayBuffer(short x, short y, short width, short height)
        {
            this.Length = width * height * sizeof(ushort);
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
        public int BitDepth { get { return _bitdepth; } }
        private const int _bitdepth = sizeof(short);
        public short X { get; set; }
        public short Y { get; set; }
        public short Width { get { return _width; } set { if (value != _width) { _width = value; RecalculateLength(); } } }
        private short _width;
        public short Height { get { return _height; } set { if (value != _height) { _height = value; RecalculateLength(); } } }
        private short _height;
        private void RecalculateLength()
        {
            this.Length = Width * Height * sizeof(ushort);
        }
        public int Length { get; private set; }
        public virtual bool ApplyPositionUpdates { get { return false; } }

        public virtual Vec216 GetCurrentPosition()
        {
            return new Vec216()
            {
                X = (short)((this.ReadPosition / 2) % this.Width),
                Y = (short)((this.ReadPosition) / this.Width)
            };
        }
        private int ReadPosition;
        private int LastReadPosition;

        /// <summary>
        /// gets the position on the x,y axis of the current display
        /// </summary>
        /// <returns></returns>
        public virtual Vec216 GetCurrentUIPosition()
        {
            return new Vec216()
            {
                X = (short)(((this.ReadPosition / 2) % this.Width) / sizeof(short)),
                Y = (short)(((this.ReadPosition) / this.Width) / sizeof(short))
            };
        }
        public RelativeRect GetDimensions(ushort[] buffer)
        {
            var readamount = this.ReadPosition + buffer.Length >= this.Length ? this.Length - this.ReadPosition : buffer.Length;
            return new RelativeRect()
            {
                Width = this.Width,
                Height = (short)((readamount) / (this.Width)),
                RelativeUIPosition = GetCurrentUIPosition()
            };
        }
        public virtual UInt24Collection Read(int maxlength)
        {
            if(ReadPosition >= this.Length)
            {
                // reset the read pointer
                ReadPosition = 0;
                return null;
            }
            //Debug.Print("Reading at position " + this.ReadPosition);
            var result = ReadInternal(this.ReadPosition, maxlength);
            LastReadPosition = ReadPosition;
            ReadPosition += result.Length;
            return result;
        }
        public abstract UInt24Collection ReadInternal(int position, int maxlength);
        public abstract void Cleanup();

        protected virtual void SetPixel(int x, int y, Color color, UInt24Collection buffer)
        {
            var offset = y * this.Width;
            buffer[offset + x] = (color.Value);
        }

        public void Dispose()
        {
        }
    }
}
