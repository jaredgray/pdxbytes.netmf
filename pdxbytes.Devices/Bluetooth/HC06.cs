using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Text;

namespace pdxbytes.Devices.Bluetooth
{
    public class HC06
    {
        public HC06(Cpu.Pin rx, Cpu.Pin tx)
        {
            this.Rx = new InputPort(rx, false, Port.ResistorMode.PullUp);
            this.Rx.OnInterrupt += Rx_OnInterrupt;
            this.Tx = new OutputPort(tx, false);
        }

        private void Rx_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            Debug.Print(data1.ToString());
            Debug.Print(data2.ToString());
        }

        public InputPort Rx { get; set; }
        public OutputPort Tx { get; set; }


    }
}
