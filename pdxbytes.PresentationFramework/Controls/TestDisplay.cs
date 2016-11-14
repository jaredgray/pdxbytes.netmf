using System;
using Microsoft.SPOT;
using pdxbytes.Graphics.Shapes;

namespace pdxbytes.PresentationFramework.Controls
{
    public class TestDisplay : Control
    {
        protected override void CreateSurface()
        {
            this.Surface = new TestDisplayBuffer();
        }
    }
}
