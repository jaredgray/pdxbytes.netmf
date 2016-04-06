

using pdxbytes.Structures;

namespace pdxbytes.Graphics
{
    public class DisplayRegion
    {
        public DisplayRegion() { }

        public Vec216 Position { get; set; }
        public uint Width { get; set; }
        public uint Heignt { get; set; }


    }
    public class Drawing
    {
        //public static void FillRelativeBuffer(DisplayBuffer buffer, ushort color)
        //{
        //    var bytea = (byte)(color >> 8);
        //    var byteb = (byte)(color);
        //    for(var i = 0; i < buffer.Length; i+=2)
        //    {
        //        buffer.Data[i] = bytea;
        //        buffer.Data[i+1] = byteb;
        //    }
        //}

        public static byte[] ReadSolidColor(DisplayBuffer buffer, int start, int length, Color color)
        {
            var c = color.Value;
            var readlen = (start + length > buffer.Length - sizeof(short)) ? (buffer.Length - sizeof(short)) - start : length;
            byte[] result = new byte[readlen];
            var bytea = (byte)(c >> 8);
            var byteb = (byte)(c);
            for (var i = 0; i < readlen; i += 2)
            {
                result[i] = bytea;
                result[i + 1] = byteb;
            }
            return result;
        }
        public static byte[] ReadSolidColor(int width, int height, Color color)
        {
            //var readlen = (start + length > buffer.Length - sizeof(short)) ? (buffer.Length - sizeof(short)) - start : length;
            var c = color.Value;
            var readlen = width * height * sizeof(short);
            byte[] result = new byte[readlen];
            var bytea = (byte)(c >> 8);
            var byteb = (byte)(c);
            for (var i = 0; i < readlen; i += 2)
            {
                result[i] = bytea;
                result[i + 1] = byteb;
            }
            return result;
        }
        public static void ReadSolidColor(byte[] buffer, int start, int width, int height, Color color)
        {
            var c = color.Value;
            var readlen = (width * height * sizeof(short)) + start;
            if (start + readlen > buffer.Length)
                readlen = buffer.Length;

            var bytea = (byte)(c >> 8);
            var byteb = (byte)(c);
            for (var i = start; i < readlen; i += 2)
            {
                buffer[i] = bytea;
                buffer[i + 1] = byteb;
            }
        }

        public static uint Max(uint a, uint b)
        {
            if (a > b)
                return a;
            return b;
        }
        public static short Max(short a, short b)
        {
            if (a > b)
                return a;
            return b;
        }

        public static short Min(short a, short b)
        {
            if (a < b)
                return a;
            return b;
        }
        public static uint Min(uint a, uint b)
        {
            if (a < b)
                return a;
            return b;
        }

        public static DisplayRegion Clip(DisplayRegion source, DisplayRegion with)
        {
            // we need the max for the source's x and y
            var Tx = Max(source.Position.X, with.Position.X);
            var Ty = Max(source.Position.Y, with.Position.Y);
            // a sanity check here might be good if there are a bunch of faults.
            // I'm not expecting a lot of faults so for now I'm going to leave this check out

            // now we need to figure out the width and height available to draw
            // Tw = Tx + Min(Cx + Cw, Sx + Sw);

            var Tw = Min((uint)(source.Position.X + source.Width), (uint)( with.Position.X + with.Width));
            var Th = Min((uint)(source.Position.Y + source.Heignt), (uint)(with.Position.Y + source.Heignt));

            return new DisplayRegion() { Heignt = Th, Width = Tw, Position = new Vec216(Tx, Ty) };

        }
    }
}
