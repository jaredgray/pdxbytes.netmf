using System;
using Microsoft.SPOT;
using pdxbytes.Graphics.Shapes;
using pdxbytes.Structures;

namespace pdxbytes.PresentationFramework.Controls.Material
{
    public class MaterialPanel : Panel
    {
        protected override void CreateSurface()
        {
            this.Surface = new DropShadowRectangle(0, 0, 0, 0);
        }
        public Color ShadowColor { get { return ((DropShadowRectangle)this.Surface).ShadowColor; } set { ((DropShadowRectangle)this.Surface).ShadowColor = value; } }
        public byte ShadowSize { get { return ((DropShadowRectangle)this.Surface).ShadowSize; } set { ((DropShadowRectangle)this.Surface).ShadowSize = value; } }
        public Edges ShadowEdges { get { return ((DropShadowRectangle)this.Surface).ShadowEdges; } set { ((DropShadowRectangle)this.Surface).ShadowEdges = value; } }
    }
}
