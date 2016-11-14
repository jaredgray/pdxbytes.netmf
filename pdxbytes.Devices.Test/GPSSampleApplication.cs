using System;
using Microsoft.SPOT;
using pdxbytes.ServiceFramework;
using pdxbytes.DeviceDrivers.GPS;
using pdxbytes.Devices.GPS;
using pdxbytes.DeviceInterfaces;
using System.Collections;

namespace pdxbytes.Devices.Test
{
    public class GPSSampleApplication : ServiceApplication
    {
        public override void OnStartup()
        {
            base.OnStartup();
            base.DeviceConfigurations.Add(new FGPMMOPA6HConfiguration(new Core.SerialPortConnection()
            {
                BaudRate = System.IO.Ports.BaudRate.Baudrate9600
            }, SecretLabs.NETMF.Hardware.NetduinoPlus.Pins.GPIO_PIN_D2));
            GpsUpdates = new ArrayList();
        }

        public override void OnLoad()
        {
            base.OnLoad();
            var gpsDevice = (IGPSDevice)base.DeviceLinker.GetDeviceInterface(typeof(IGPSDevice));
            gpsDevice.PositionUpdate += GpsDevice_PositionUpdate;
            gpsDevice.Connect();
        }
        private ArrayList GpsUpdates;
        private void GpsDevice_PositionUpdate(Structures.GPSUpdate update)
        {
            GpsUpdates.Add(update);
            if(GpsUpdates.Count > 9)
            {
                DumpUpdates();
            }
        }

        private void DumpUpdates()
        {
            var updates = GpsUpdates;
            GpsUpdates = new ArrayList();
            foreach(Structures.GPSUpdate update in updates)
            {
                Debug.Print(update.Point.ToString());
            }
        }
    }
}
