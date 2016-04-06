using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace pdxbytes.Devices.StepperMotors
{
    public class ULN2003MotorController
    {
        public ULN2003MotorController(Cpu.Pin pin0, Cpu.Pin pin1, Cpu.Pin pin2, Cpu.Pin pin3)
        {
            port1 = new OutputPort(pin0, true);
            port2 = new OutputPort(pin1, false);
            port3 = new OutputPort(pin2, false);
            port4 = new OutputPort(pin3, false);

            index = 0;
            SetPattern(pattern1[index]);

            this.delay = 2;
        }

        private byte[] pattern1 = { 0,1,2,3,4,5,6,7 };

        OutputPort port1;
        OutputPort port2;
        OutputPort port3;
        OutputPort port4;

        private int delay;
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        private int index;
        
        private void SetPattern(byte p)
        {
            //switch (p)
            //{
            //    case 1:
            //        port1.Write(true);
            //        port2.Write(false);
            //        port3.Write(false);
            //        port4.Write(false);
            //        break;
            //    case 2:
            //        port1.Write(false);
            //        port2.Write(true);
            //        port3.Write(false);
            //        port4.Write(false);
            //        break;
            //    case 4:
            //        port1.Write(false);
            //        port2.Write(false);
            //        port3.Write(true);
            //        port4.Write(false);
            //        break;
            //    case 8:
            //        port1.Write(false);
            //        port2.Write(false);
            //        port3.Write(false);
            //        port4.Write(true);
            //        break;
            //}
            switch (p)
            {
                case 0:
                    port1.Write(false);
                    port2.Write(false);
                    port3.Write(false);
                    port4.Write(true);
                    break;
                case 1:
                    port1.Write(false);
                    port2.Write(false);
                    port3.Write(true);
                    port4.Write(true);
                    break;
                case 2:
                    port1.Write(false);
                    port2.Write(false);
                    port3.Write(true);
                    port4.Write(false);
                    break;
                case 3:
                    port1.Write(false);
                    port2.Write(true);
                    port3.Write(true);
                    port4.Write(false);
                    break;
                case 4:
                    port1.Write(false);
                    port2.Write(true);
                    port3.Write(false);
                    port4.Write(false);
                    break;
                case 5:
                    port1.Write(true);
                    port2.Write(true);
                    port3.Write(false);
                    port4.Write(false);
                    break;
                case 6:
                    port1.Write(true);
                    port2.Write(false);
                    port3.Write(false);
                    port4.Write(false);
                    break;
                case 7:
                    port1.Write(true);
                    port2.Write(false);
                    port3.Write(false);
                    port4.Write(true);
                    break;
                default:
                    port1.Write(false);
                    port2.Write(false);
                    port3.Write(false);
                    port4.Write(false);
                    break;
            }
        }

        public void StepRight()
        {
            index = index == 7 ? 0 : index += 1;

            SetPattern(pattern1[index]);
            Thread.Sleep(this.delay);
        }

        public void StepRight(int Steps)
        {
            int i = 0;
            for (int r = 0; r < Steps; r++)
            {
                SetPattern(pattern1[System.Math.Abs(i)]);
                i += 1;
                i = i % 4;
                Thread.Sleep(this.delay);
            }
        }

        public void StepLeft()
        {
            index = index == 0 ? 7 : index -= 1;

            SetPattern(pattern1[index]);
            Thread.Sleep(this.delay);
        }

        public void StepLeft(int Steps)
        {
            int i = 3;
            for (int r = 0; r < Steps; r++)
            {
                SetPattern(pattern1[System.Math.Abs(i)]);
                i = i == 0 ? 3 : i -= 1;
                Thread.Sleep(this.delay);
            }
        }

        public void Wait(int Delay)
        {
            Thread.Sleep(Delay);
        }
    }
}
