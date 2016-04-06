
using pdxbytes.DeviceInterfaces;
using pdxbytes.DeviceInterfaces.Configuration;
using System;
using VikingErik.NetMF.MicroLinq;

namespace pdxbytes.DeviceLinking
{
    public class DeviceLink
    {
        public DeviceLink() { _Devices = new IDeviceCollection(); }


        public void ConfigureDevices(DeviceConfigurationCollection configurations)
        {
            
            foreach (var config in configurations)
                this.ConfigureDevice((IDeviceConfiguration)config);
        }

        private void ConfigureDevice(IDeviceConfiguration config)
        {
            var device = config.CreateDevice();
            this.Devices.Add(device);

            // detect the need to create a uicontroller
        }

        public IDeviceCollection Devices
        {
            get { return _Devices; }
        }
        private IDeviceCollection _Devices;


        //public T GetDeviceInterface<T>()
        //    where T : class
        //{
        //    foreach(var device in this.Devices)
        //    {
        //        if (device is T)
        //            return (T)device;
        //    }

        //    return null;
        //}

        public object GetDeviceInterface(Type t)
        {
            foreach (var device in this.Devices)
            {
                if (device.GetType().GetInterfaces().Any(x => x == t))
                    return device;
            }

            return null;
        }
    }

}
