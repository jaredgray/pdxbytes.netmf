using System;
using Microsoft.SPOT;
using pdxbytes.Collections;

namespace pdxbytes.Graphics.Shapes
{
    public class ShapeCollection : BaseCollection
    {
        #region IEnumerable members 

        public ShapeCollection()
        {
        }

        public void Add(Shape t)
        {
            bool inserted = false;
            for (int i = 0; i < this.inner.Count; i++)
            {
                if (this[i].Zindex > t.Zindex)
                {
                    inserted = true;
                    inner.Insert(i - 1, t);
                }
            }
            if (!inserted)
                this.inner.Insert(0, t);
        }

        public void AddRange(ShapeCollection collection)
        {
            foreach (Shape t in collection)
                this.Add(t);
        }

        public void Remove(Shape t)
        {
            inner.Remove(t);
        }

        public Shape this[int i]
        {
            get { return (Shape)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }
}
