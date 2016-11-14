using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;
using pdxbytes.DeviceInterfaces.Configuration;
using pdxbytes.DeviceLinking;
using System.Threading;

namespace pdxbytes.ServiceFramework
{
    public class ServiceApplication : IApp
    {
        public ServiceApplication()
        {
            _current = this;
            _deviceconfigurations = new DeviceConfigurationCollection();
        }
        public DeviceConfigurationCollection DeviceConfigurations { get { return _deviceconfigurations; } }
        private DeviceConfigurationCollection _deviceconfigurations;
        protected DeviceLink DeviceLinker { get; set; }
        public static ServiceApplication Current { get { return _current; } }
        private static ServiceApplication _current;
        private bool Running { get; set; }
        public void AddDeviceConfiguration(IDeviceConfiguration config)
        {
            DeviceConfigurations.Add(config);
        }

        public void Run()
        {
            this.OnStartup();

            this.DeviceLinker = new DeviceLink();
            this.DeviceLinker.ConfigureDevices(this.DeviceConfigurations);

            this.OnLoad();

            while (this.Running)
                Thread.Sleep(1000);

            this.OnShutDown();
        }

        public virtual void OnLoad()
        {

        }
        public virtual void OnStartup()
        {

        }
        public virtual void OnShutDown()
        {

        }
    }
}
