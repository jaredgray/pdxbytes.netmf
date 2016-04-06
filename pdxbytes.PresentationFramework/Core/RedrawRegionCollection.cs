using System;
using Microsoft.SPOT;
using pdxbytes.Collections;

namespace pdxbytes.PresentationFramework.Core
{
    internal class RedrawRegionCollection : BaseCollection
    {
        #region IEnumerable members 

        public RedrawRegionCollection()
        {
        }

        public void Add(RedrawRegion t)
        {
            inner.Add(t);
        }

        public void AddRange(RedrawRegionCollection collection)
        {
            foreach (RedrawRegion t in collection)
                inner.Add(t);
        }

        public void Remove(RedrawRegion t)
        {
            inner.Remove(t);
        }

        public RedrawRegion this[int i]
        {
            get { return (RedrawRegion)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }

}
