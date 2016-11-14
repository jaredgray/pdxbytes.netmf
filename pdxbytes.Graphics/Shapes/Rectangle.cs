
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Shapes
{
    public class Rectangle : Shape
    {
        public Rectangle(short x, short y, short width, short height, byte zindex = 1, Color backgroundcolor = null)
            : base(x, y, width, height, zindex, backgroundcolor)
        {
        }
        UInt24Collection data;
        public override UInt24Collection ReadInternal(int position, int maxlength)
        {
            if (null == this.BackgroundColor)
                return null;
            if (position >= this.Length)
                return null;
            else if(null == data)
                data = Drawing.ReadSolidColor(this, position, maxlength, this.BackgroundColor);
            return data;
        }

        public override void Cleanup()
        {
            data.Dispose();
            data = null;
        }

        //public override void Draw(IGraphicDevice context)
        //{
        //    Drawing.FillRelativeBuffer(this, this.BackgroundColor);
        //    context.DrawBuffer(this);
        //}
    }
}
