
using pdxbytes.Collections;

namespace pdxbytes.Structures
{

    public class Vec2Collection : BaseCollection
    {
        #region IEnumerable members 

        public Vec2Collection()
        {
        }

        public Vec2Collection(params Vec2[] items)
            : base(items)
        {

        }

        public void Add(Vec2 t)
        {
            inner.Add(t);
        }

        public void AddRange(Vec2Collection collection)
        {
            foreach (Vec2 t in collection)
                inner.Add(t);
        }

        public void Remove(Vec2 t)
        {
            inner.Remove(t);
        }

        public Vec2 this[int i]
        {
            get { return (Vec2)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }

}
