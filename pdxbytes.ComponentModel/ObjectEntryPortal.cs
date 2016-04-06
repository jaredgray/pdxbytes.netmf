using System;
using Microsoft.SPOT;
using System.Threading;

namespace pdxbytes.ComponentModel
{
    internal class ObjectEntryPortal
    {
        static ObjectEntryPortal()
        {
            Repository = new System.Collections.Hashtable();
            lockobject = new object();
        }
        private static object lockobject;
        private static int CurrentObjectIndex = 0;
        private static System.Collections.Hashtable Repository;

        public static int Add(object value)
        {
            lock (lockobject)
            {
                Interlocked.Increment(ref CurrentObjectIndex);
                Repository[CurrentObjectIndex] = value;
                return CurrentObjectIndex;
            }
        }
        public static object Get(int id)
        {
            lock(lockobject)
            {
                return Repository[id];
            }
        }
        public static void Set(int id, object value)
        {
            lock(lockobject)
            {
                Repository[id] = value;
            }
        }
    }
}
