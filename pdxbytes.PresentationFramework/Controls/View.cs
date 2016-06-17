using System;
using Microsoft.SPOT;

namespace pdxbytes.PresentationFramework.Controls
{
    public class View : UIElement
    {
        public View()
        {
            this.AllControls = new ViewControlCollection(this);
        }

        
        public ViewControlCollection AllControls { get; private set; }
    }
}
