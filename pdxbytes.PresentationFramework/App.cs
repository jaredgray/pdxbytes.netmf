
using pdxbytes.PresentationFramework.Core;
using pdxbytes.DeviceInterfaces;
using pdxbytes.DeviceLinking;
using pdxbytes.DeviceInterfaces.Configuration;
using pdxbytes.PresentationFramework.Controls;
using System.Threading;
using System;

namespace pdxbytes.PresentationFramework
{
    public abstract class App : IApp
    {
        public App()
        {
            _current = this;
            _deviceconfigurations = new DeviceConfigurationCollection();
        }

        public static App Current { get { return _current; } }
        private static App _current;
        private bool Running { get; set; }

        public View MainView
        {
            get { return _MainView; }
            set
            {
                if (value != _MainView)
                {
                    _MainView = value;
                    if(null != this.Controller)
                        this.Controller.SetMainView(value);
                }
            }
        }
        private View _MainView;

        public View NavigationView
        {
            get
            {
                return _NavigationView;
            }
            set
            {
                if (value != _NavigationView)
                {
                    _NavigationView = value;
                    if (null != this.Controller)
                        this.Controller.SetNavigationView(value);
                }

            }
        }
        private View _NavigationView;

        /// <summary>
        /// The UIController
        /// </summary>
        internal UIController Controller { get; set; }
        
        public DeviceConfigurationCollection DeviceConfigurations { get { return _deviceconfigurations; } }
        private DeviceConfigurationCollection _deviceconfigurations;

        private DeviceLink Linker { get; set; }

        public void ShutDown()
        {
            this.Running = false;
        }

        public void Run()
        {
            this.OnStartup();

            if (null == this.MainView)
                throw new System.Exception("A UI type application needs the MainView property set before running.");

            this.Controller = new UIController();
            this.Linker = new DeviceLink();
            this.Linker.ConfigureDevices(this.DeviceConfigurations);

            var display = (IDisplay)this.Linker.GetDeviceInterface(typeof(IDisplay));
            if (null == display)
                throw new System.Exception("A UI type application requires a device configuration for a display");
            
            this.Running = true;

            this.Controller.Display = display;

            var touchinterface = (ITouchInterface)this.Linker.GetDeviceInterface(typeof(ITouchInterface));
            if (null != touchinterface)
                this.Controller.TouchInterface = touchinterface;

            this.Controller.SetMainView(this.MainView);
            this.Controller.SetNavigationView(this.NavigationView);

            this.Controller.Startup();

            this.OnLoad();

            while (this.Running)
                Thread.Sleep(1000);

            this.OnShutDown();
        }

        public void AddDeviceConfiguration(IDeviceConfiguration config)
        {
            this.DeviceConfigurations.Add(config);
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
