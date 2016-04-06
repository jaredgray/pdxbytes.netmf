using System;
using Microsoft.SPOT;

namespace pdxbytes.PresentationFramework.Controls
{
    public class ViewControlCollection : ControlCollection
    {
        protected override bool AddToViewCollection
        {
            get
            {
                return false;
            }
        }
        
    }
}
