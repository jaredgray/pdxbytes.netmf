
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Shapes
{
    public class Hamburger : Shape
    {
        public Hamburger()
        {

        }
        public Hamburger(short x, short y, short width, short height, int zindex = 1, Color foregroundcolor = null, Color backgroundcolor = null)
            : base(x, y, width, height, zindex, backgroundcolor)
        {
            this.ForegroundColor = foregroundcolor;
        }
        public Color ForegroundColor { get; set; }
        public override byte[] ReadInternal(int position, int maxlength)
        {
            if (null == this.ForegroundColor || null == this.BackgroundColor)
                return null;
            var part = (short)(this.Height / 5);
            var allocation = new byte[this.Width * this.Height * sizeof(short)];
            int pos = 0;
            Drawing.ReadSolidColor(allocation, pos, this.Width, part, this.ForegroundColor);
            pos+=(this.Width * part * sizeof(short));
            Drawing.ReadSolidColor(allocation, pos, this.Width, part, this.BackgroundColor);
            pos += (this.Width * part * sizeof(short));
            Drawing.ReadSolidColor(allocation, pos, this.Width, part, this.ForegroundColor);
            pos += (this.Width * part * sizeof(short));
            Drawing.ReadSolidColor(allocation, pos, this.Width, part, this.BackgroundColor);
            pos += (this.Width * part * sizeof(short));
            Drawing.ReadSolidColor(allocation, pos, this.Width, part * 5, this.ForegroundColor);
            return allocation;
        }

        public override void Cleanup()
        {
        }
    }
}
