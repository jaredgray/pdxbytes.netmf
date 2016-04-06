
using pdxbytes.Collections;

namespace pdxbytes.Structures
{

    public class ColorCollection : BaseCollection
    {
        #region IEnumerable members 

        public ColorCollection()
        {
        }

        public void Add(Color t)
        {
            inner.Add(t);
        }

        public void AddRange(ColorCollection collection)
        {
            foreach (Color t in collection)
                inner.Add(t);
        }

        public void Remove(Color t)
        {
            inner.Remove(t);
        }

        public Color this[int i]
        {
            get { return (Color)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }

}
