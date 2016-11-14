using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;
using pdxbytes.Structures;
using pdxbytes.Encoders;

namespace pdxbytes.Devices.Display
{
    public abstract class BaseTFT : IDisplay, IDisposable
    {
        public abstract int BufferSize { get; }

        public abstract int Stride { get; }

        public abstract ColorEncoder Encoder { get; }

        public abstract void Dispose();

        public abstract void Initialize();

        public abstract void DisplayOn();

        public abstract byte ReadCommand(byte command);

        public abstract void Reset();

        public abstract void Sleep();

        public abstract void Wake();
        
        public abstract void BeginDraw(short x, short y, short width, short height);

        public abstract void WriteBuffer(UInt24Collection buffer);
        
        public Orientations CurrentOrientation { get; set; }
        public abstract short Width { get; }
        public abstract short Height { get; }
    }
}
