using System;
using Microsoft.SPOT;

namespace pdxbytes.PresentationFramework.Controls
{
    public class ViewControlCollection : ControlCollection
    {
        public ViewControlCollection(UIElement owner) : base(owner) { }
        protected override bool AddToViewCollection
        {
            get
            {
                return false;
            }
        }
        
    }
}
