using System;
using Microsoft.SPOT;
using pdxbytes.Collections;

namespace pdxbytes.PresentationFramework.Controls
{
    /// <summary>
    /// this collection adds controls sorted from smallest to largest z-index. if the z-index is changed after added to the collection, the item will not be re-indexed.
    /// </summary>
    public class ControlCollection : BaseCollection
    {
        #region IEnumerable members 
        private static int Z = 0;
        public ControlCollection()
        {
        }

        public void Add(Control t)
        {
            bool inserted = false;
            if (t.Zindex > 0)
            {
                var innercount = this.inner.Count;
                for (int i = 0; i < innercount; i++)
                {
                    if (this[i].Zindex > t.Zindex)
                    {
                        inserted = true;
                        inner.Insert(i , t);
                        break;
                    }
                }
            }
            else
            {
                System.Threading.Interlocked.Increment(ref Z);
                t.Zindex = Z;
            }
            if (!inserted)
                this.inner.Insert(0, t);
            if (this.AddToViewCollection)
                App.Current.MainView.AllControls.Add(t);
        }

        public void AddRange(ControlCollection collection)
        {
            foreach (Control t in collection)
                this.Add(t);
        }

        public void Remove(Control t)
        {
            inner.Remove(t);
        }

        public Control this[int i]
        {
            get { return (Control)inner[i]; }
            set { inner[i] = value; }
        }

        protected virtual bool AddToViewCollection { get { return true; } }

        #endregion
    }

}
