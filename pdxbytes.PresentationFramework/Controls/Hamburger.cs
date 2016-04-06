using System;
using Microsoft.SPOT;
using pdxbytes.Structures;

namespace pdxbytes.PresentationFramework.Controls
{
    public class Hamburger : Control
    {
        public Hamburger()
        {
            this.Surface = new Graphics.Shapes.Hamburger();
        }

        public Color ForegroundColor
        {
            get { return ((Graphics.Shapes.Hamburger)this.Surface).ForegroundColor; }
            set
            {
                ((Graphics.Shapes.Hamburger)this.Surface).ForegroundColor = value;
            }
        }
    }
}
