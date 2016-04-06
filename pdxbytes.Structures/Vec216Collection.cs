using pdxbytes.Collections;

namespace pdxbytes.Structures
{

    public class Vec216Collection : BaseCollection
    {
        #region IEnumerable members 

        public Vec216Collection()
        {
        }

        public void Add(Vec216 t)
        {
            inner.Add(t);
        }

        public void AddRange(Vec216Collection collection)
        {
            foreach (Vec216 t in collection)
                inner.Add(t);
        }

        public void Remove(Vec216 t)
        {
            inner.Remove(t);
        }

        public Vec216 this[int i]
        {
            get { return (Vec216)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }

}
