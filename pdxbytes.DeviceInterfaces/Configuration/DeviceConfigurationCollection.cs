using pdxbytes.Collections;

namespace pdxbytes.DeviceInterfaces.Configuration
{
    public class DeviceConfigurationCollection : BaseCollection
    {
        #region IEnumerable members 

        public DeviceConfigurationCollection()
        {
        }

        public void Add(IDeviceConfiguration t)
        {
            inner.Add(t);
        }

        public void AddRange(DeviceConfigurationCollection collection)
        {
            foreach (IDeviceConfiguration t in collection)
                inner.Add(t);
        }

        public void Remove(IDeviceConfiguration t)
        {
            inner.Remove(t);
        }

        public IDeviceConfiguration this[int i]
        {
            get { return (IDeviceConfiguration)inner[i]; }
            set { inner[i] = value; }
        }

        #endregion
    }

}
