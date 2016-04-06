
using pdxbytes.Graphics.Behavior;
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Shapes
{
    public abstract class Shape : Graphics.DisplayBuffer
    {
        public Shape() { }
        public Shape(short x, short y, short width, short height, int zindex, Color backgroundcolor)
            : base(x, y, width, height)
        {
            this.Zindex = zindex;
            this.BackgroundColor = backgroundcolor;
        }
        /// <summary>
        /// supported only on touch input devices
        /// </summary>
        public event TouchedEventHandler Touched;

        public int Zindex { get; set; }
        public Color BackgroundColor { get; set; }
        
        //public abstract void Draw(IGraphicDevice context);
    }
}
