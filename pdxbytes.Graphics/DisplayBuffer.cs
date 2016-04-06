
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
        public virtual byte[] Read(int maxlength)
        {
            if(ReadPosition >= this.Length)
            {
                // reset the read pointer
                ReadPosition = 0;
                return null;
            }
            //Debug.Print("Reading at position " + this.ReadPosition);
            var result = ReadInternal(this.ReadPosition, maxlength);
            ReadPosition += result.Length;
            return result;
        }
        public abstract byte[] ReadInternal(int position, int maxlength);
        public abstract void Cleanup();

        public void Dispose()
        {
        }
    }
}
