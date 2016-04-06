using System;
using Microsoft.SPOT;
using pdxbytes.Collections;

namespace pdxbytes.DeviceInterfaces
{

    public class IDeviceCollection : BaseCollection
    {
        #region IEnumerable members 

        public IDeviceCollection()
        {
        }

        public void Add(IDevice t)
        {
            inner.Add(t);
        }

        public void AddRange(IDeviceCollection collection)
        {
            foreach (IDevice t in collection)
                inner.Add(t);
        }

        public void Remove(IDevice t)
        {
            inner.Remove(t);
        }

        public IDevice this[int i]
        {
            get { return (IDevice)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }

}
