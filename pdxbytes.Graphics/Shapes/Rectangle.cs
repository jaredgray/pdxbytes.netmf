
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Shapes
{
    public class Rectangle : Shape
    {
        public Rectangle(short x, short y, short width, short height, byte zindex = 1, Color backgroundcolor = null)
            : base(x, y, width, height, zindex, backgroundcolor)
        {
        }
        byte[] stride;
        public override byte[] ReadInternal(int position, int maxlength)
        {
            if (null == this.BackgroundColor)
                return null;
            if (position >= this.Length)
                return null;
            else if (null == stride)
                stride = Drawing.ReadSolidColor(this, position, maxlength, this.BackgroundColor);
            return stride;
        }

        public override void Cleanup()
        {
            stride = null;
        }

        //public override void Draw(IGraphicDevice context)
        //{
        //    Drawing.FillRelativeBuffer(this, this.BackgroundColor);
        //    context.DrawBuffer(this);
        //}
    }
}
