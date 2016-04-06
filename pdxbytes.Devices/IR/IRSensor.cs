using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using pdxbytes.Structures.Contracts;
using pdxbytes.Extensions.MathExtensions;
using pdxbytes.Extensions.ByteExtensions;
using pdxbytes.Structures;

namespace pdxbytes.Devices.IR
{
    public class IRSensor
    {
        public IRSensor(Cpu.Pin inputPin, IObjectDecoder decoder)
        {
            this.IRInputPin = new InterruptPort(inputPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            this.IRInputPin.OnInterrupt += IRInputPin_OnInterrupt;
            this.RCtimeoutTimer = new Timer(new TimerCallback(RCtimeout), null, Timeout.Infinite, Timeout.Infinite);
            this.decoder = decoder;
            this.decoder.Decoded += Decoder_Decoded;
        }

        private void Decoder_Decoded(object sender, object data)
        {
            if (null == data || !(data is byte[]))
                return;
            this.OnReceived((byte[])data);
        }

        private void IRInputPin_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            RecordPulse(data1, data2, time);
        }

        private InterruptPort IRInputPin;
        private const int rc_timeout = 20;
        private Timer RCtimeoutTimer;
        private IObjectDecoder decoder;

        // Raw IR data

        private int position = 0; // Current position in IRDataItems


        private Pulse[] IRDataItems = new Pulse[512];

        private void RecordPulse(uint data1, uint data2, DateTime time)
        {
            // Record the timestamp and state
            IRDataItems[position].Timestamp = time.Ticks / 10;
            IRDataItems[position].State = (data2 == 1); // pin state as true/false

            // Increment the position
            ++position;

            // Start from beginning on overflow
            if (position > IRDataItems.Length)
            {
                position = 0;
            }

            // Reset the timeout timer
            RCtimeoutTimer.Change(rc_timeout, Timeout.Infinite);    // set / reset the timeout timer
        }

        private void RCtimeout(object o)
        {
            // Record the final state
            IRDataItems[position].Timestamp = DateTime.Now.Ticks / 10;
            IRDataItems[position].State = IRInputPin.Read();

            // Turn off the timeout timer
            RCtimeoutTimer.Change(Timeout.Infinite, Timeout.Infinite);

            // Decode the received code
            Decode(position);

            // Reset the position and wipe out the buffer
            position = 0;

            for (int i = 0; i < IRDataItems.Length - 1; i++)
            {
                IRDataItems[i].Timestamp = 0;
                IRDataItems[i].State = false;
            }
        }

        private void Decode(int marks)
        {
            Debug.Print(marks.ToString() + " Marks detected");
            decoder.Decode(IRDataItems, marks + 1);
        }

        private void OnReceived(byte[] bytes)
        {
            var handler = Received;
            if (null != handler)
                handler(bytes);
        }
        public event BytesDecoded Received;
    }

    public class IRDecoderOptions
    {
        /// <summary>
        /// the number of pulses that are sent as a header.. currently we are not decoding the header
        /// </summary>
        public int HeaderLength { get; set; }
        /// <summary>
        /// how accurate you want the true pulse to be.. this is similar to Rounding but gives you a percentage threshold
        /// </summary>
        public double PercentAccuracy { get; set; }
        /// <summary>
        /// the length of time considered for a true pulse
        /// </summary>
        public long TrueTicks { get; set; }
        /// <summary>
        /// rounding can be used to normalize small differences in timing.. if ticks end in say 97 or 42.. 
        /// we can round them with this to the nearest nth
        /// </summary>
        public int Rounding { get; set; }
        /// <summary>
        /// indicates that the pulses should be recorded when signal is high
        /// </summary>
        public bool RecordDataPulseHigh { get; set; }
        /// <summary>
        /// indicates that the pulses should be recorded when signal is low
        /// </summary>
        public bool RecordDataPulseLow { get; set; }
    }
    public class IRDecoder : IObjectDecoder
    {
        public IRDecoder(IRDecoderOptions options)
        {
            this.options = options;
        }
        private IRDecoderOptions options;
        public void Decode(Array items, int count)
        {
            Decode(items, 0, count);
        }

        //Endianess is not taken into account during this decoding.
        public void Decode(Array items, int startIndex, int count)
        {

            //startIndex += options.HeaderLength;
            int totalbitcount = (count - startIndex);
            int bitcount = totalbitcount - (2 * options.HeaderLength); // 2 bits per pulse
            if (!options.RecordDataPulseLow)
                bitcount /= 2;
            if (!options.RecordDataPulseHigh)
                bitcount /= 2;
            // if the bitcount is not divisible by 8 we don't have even bytes.. todo...
            int bytecount = totalbitcount / 8;
            if (bytecount <= 0 || (bitcount % 8 != 0))
                return;

            var bits = new bool[bitcount];
            var headerbits = new bool[options.HeaderLength];
            int bitindex = 0;
            if (!options.RecordDataPulseHigh && !options.RecordDataPulseLow)
                return;
            for (int i = startIndex + 1; i < totalbitcount; ++i)
            {
                IPulse last = (IPulse)items.GetValue(i - 1);
                IPulse current = (IPulse)items.GetValue(i);

                var timefromlast = (current.Timestamp - last.Timestamp).Round(options.Rounding);

                //Debug.Print("@" + (i - 1) + " - T: " + timefromlast);
                if (i <= options.HeaderLength)
                {
                    headerbits[i - 1] = DecodeTrueBit(timefromlast);
                }
                else
                {
                    // this looks backwards (current.State) but it's right
                    if(!current.State && options.RecordDataPulseHigh)
                    {
                        if(bitindex < bits.Length)
                            bits[bitindex] = DecodeTrueBit(timefromlast);
                        ++bitindex;
                    }
                    if(current.State && options.RecordDataPulseLow)
                    {
                        bits[bitindex] = DecodeTrueBit(timefromlast);
                        ++bitindex;
                    }
                }

            }
            var bytes = bits.ToByteArray();
            Debug.Print("---------------------");
            Debug.Print(bits.ToDisplayString());
            foreach (var b in bytes)
                Debug.Print("0x" + b.ToString("X2") + " - " + ((int)b).ToString());
            OnReceived(bytes);
        }

        private void OnReceived(byte[] bytes)
        {
            var handler = Decoded;
            if (null != handler)
                handler(this, bytes);
        }
        
        public event OnObjectDecoded Decoded;

        private bool DecodeTrueBit(long interval)
        {
            if ((interval < options.TrueTicks * options.PercentAccuracy) || (interval > options.TrueTicks * (1 + (1 - options.PercentAccuracy))))
            {
                return false;
            }
            return true;
        }
    }
    public delegate void BytesDecoded(byte[] bytes);
    // not finished at all
    public class PwmDecoder : IObjectDecoder
    {
        public event OnObjectDecoded Decoded;

        public void Decode(Array items, int count)
        {
            Decode(items, 0, count);
        }

        public void Decode(Array items, int startIndex, int count)
        {

            long offset = ((ITimeStructure)items.GetValue(startIndex)).Timestamp;
            long lasttime = offset;
            for (int i = startIndex; i < count; i++)
            {
                ITimeStructure structure = (ITimeStructure)items.GetValue(i);

                var timefromlast = (structure.Timestamp - lasttime).Round(100);

                Debug.Print(timefromlast.ToString());

                lasttime = structure.Timestamp;
            }

        }
    }
}
