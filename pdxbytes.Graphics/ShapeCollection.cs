using System;
using Microsoft.SPOT;
using System.Collections;
using pdxbytes.Graphics.Shapes;
using pdxbytes.Collections;

namespace pdxbytes.Graphics
{
    public class ShapeCollection : BaseCollection
    {
        #region IEnumerable members 
        
        public ShapeCollection()
        {
        }

        public void Add(Shape address)
        {
            inner.Add(address);
        }

        public void AddRange(ShapeCollection addresses)
        {
            foreach (Shape address in addresses)
                inner.Add(address);
        }

        public void Remove(Shape address)
        {
            inner.Remove(address);
        }

        public Shape this[int i]
        {
            get { return (Shape)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }
}
