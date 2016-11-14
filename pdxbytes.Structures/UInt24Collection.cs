using System;
using Microsoft.SPOT;
using System.Collections;
using pdxbytes.Collections;

namespace pdxbytes.Structures
{
    public class UInt24Collection 
    {
        public UInt24Collection(int size)
        {
            inner = new UInt24[size];
        }
        #region Base collection

        protected UInt24[] inner;

        public int Count { get { return inner.Length; } }

        public int Length { get { return inner.Length; } }

        public IEnumerator GetEnumerator()
        {
            return inner.GetEnumerator();
        }
        
        public virtual void Dispose()
        {
            this.inner = null;

        }

        #endregion
        #region IEnumerable members 

        public UInt24Collection()
        {
        }

        //public void Add(UInt24 t)
        //{
        //    inner.Add(t);
        //}

        //public void AddRange(UInt24Collection collection)
        //{
        //    foreach (UInt24 t in collection)
        //        inner.Add(t);
        //}

        //public void Remove(UInt24 t)
        //{
        //    inner.Remove(t);
        //}

        public UInt24 this[int i]
        {
            get { return (UInt24)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }
}
