using System;
using Microsoft.SPOT;
using System.IO.Ports;
using System.Text;
using pdxbytes.Devices.Bluetooth;
using pdxbytes.Sketches.Sensors.Temperature;

namespace pdxbytes.Sketches.Bluetooth
{
    public static class HC06Sketch
    {
        public static void Run()
        {
            // bluetoothmodule = new BC417(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);

            var bytes = Encoding.UTF8.GetBytes("hello");
            var str = Encoding.UTF8.GetChars(bytes);
            bluetoothdevice = new BluetoothDevice(Serial.COM1, BaudRate.Baudrate38400, Parity.None, 8, StopBits.One, "bc417_004");
            bluetoothdevice.DataRecieved += Bluetoothdevice_DataRecieved;
            bluetoothdevice.DeviceStatusChanged += Bluetoothdevice_DeviceStatusChanged;
            bluetoothdevice.Connect();

            M31855KSketch.TemperatureUpdate += M31855KSketch_TemperatureUpdate;
        }

        private static void M31855KSketch_TemperatureUpdate(double celcious)
        {
            bluetoothdevice.Write("" + celcious + " C\r\n");
        }

        private static void Bluetoothdevice_DeviceStatusChanged(BluetoothDevice device, DeviceStatusCodes status)
        {
            Debug.Print("Status Changed: " + status.ToString());
        }

        private static void Bluetoothdevice_DataRecieved(string data, DateTime time)
        {
            Debug.Print("Data Recieved: " + data);
        }
        
        private static BluetoothDevice bluetoothdevice;
    }
}
