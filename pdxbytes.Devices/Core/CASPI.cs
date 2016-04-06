using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace pdxbytes.Devices.Core
{
    public class CASPI
    {
        public CASPI(Cpu.Pin sclk, Cpu.Pin cs, Cpu.Pin data)
        {
            this.sclk = new OutputPort(sclk, false);
            this.data = new InputPort(data, false, Port.ResistorMode.Disabled);
            this.cs = new OutputPort(cs, true);
        }

        private OutputPort sclk;
        private OutputPort cs;
        private InputPort data;


        public int ReadInt32()
        {
            int i;
            int d = 0;

            //if (hSPI)
            //{
            //    return hspiread32();
            //}

            sclk.Write(false);
            DelayMicroseconds(1);
            cs.Write(false);
            DelayMicroseconds(1);

            for (i = 31; i >= 0; i--)
            {
                sclk.Write(false);
                DelayMicroseconds(1);
                d <<= 1;
                if (data.Read())
                {
                    d |= 1;
                }

                sclk.Write(true);
                DelayMicroseconds(1);
            }

            cs.Write(true);
            return d;
        }

        private void DelayMicroseconds(int microseconds)
        {
            long delayTime = microseconds * 10;
            long delayStart = Utility.GetMachineTime().Ticks;
            while ((Utility.GetMachineTime().Ticks - delayStart) < delayTime) ;
        }
    }
}
