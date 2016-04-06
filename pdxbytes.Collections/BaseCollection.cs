using System;
using System.Collections;

namespace pdxbytes.Collections
{

    public abstract class BaseCollection : IEnumerable, IDisposable
    {
        public BaseCollection() { this.inner = new ArrayList(); }
        public BaseCollection(IEnumerable items)
        {
            this.inner = new ArrayList();
            foreach (var item in items)
                this.inner.Add(item);
        }
        protected ArrayList inner;

        public int Count { get { return inner.Count; } }

        public void RemoveAt(int index)
        {
            inner.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        public void Clear()
        {
            this.inner.Clear();
        }
        public virtual void Dispose()
        {
            this.Clear();
            this.inner = null;

        }
        

        //public void Sort(ArrayList items, int lo0, int hi0, IComparer comparer)
        //{
        //    int i;
        //    int j;
        //    //int v;
        //    object tmpItem;

        //    for (i = lo0 + 1; ilo0) && (comparer.Compare(this.inner[j - 1], tmpItem) > 0) )
        //{
        //        this.inner[j] = this.inner[j - 1];
        //        j--;
        //    }

        //    this.inner[j] = tmpItem;
        //}


    }
}
