using pdxbytes.Structures;
using System;
using System.Collections;

namespace pdxbytes.Graphics.Text
{
    public class TextBlock : Shapes.Shape
    {
        public TextBlock(string text, short x, short y, byte zindex, byte size, Color forecolor, Color backgroundcolor, Font font)
            : base(x, y, size, size, zindex, backgroundcolor)
        {
            this.Font = font;
            this.FontSize = (byte)(size / 8);
            this.ForeColor = forecolor;
            this.CurrentX = x;
            this.CurrentY = y;
            this.Text = text;
        }
        public Color ForeColor { get; set; }
        public string Text { get; set; }
        public Font Font { get; set; }
        public byte FontSize { get; set; }
        public override void Cleanup()
        {
            this.CurrentX = this.X;
            this.CurrentY = this.Y;
            //charidx = fontbyte = fontbytebitposition = 0;
        }
        public short CurrentX { get; set; }
        public short CurrentY { get; set; }
        public override Vec216 GetCurrentPosition()
        {
            return new Vec216(this.CurrentX, this.CurrentY);
        }
        bool Reading = false;
        public override byte[] Read(int maxlength)
        {
            if (bytelist.Count == 0 && !Reading)
            {
                Reading = true;
                this.DrawString(this.Text, this.X, this.Y, this.FontSize, this.ForeColor);
            }
            if (bytelist.Count == 0)
            {
                Reading = false;
                return null;
            }
            var current = (Block)bytelist[0];
            this.CurrentX = current.Position.X;
            this.CurrentY = current.Position.Y;
            bytelist.RemoveAt(0);
            return current.Data;
        }
        const byte FONT_X = 8;
        
        ArrayList bytelist = new ArrayList();
        struct Block { public byte[] Data; public Vec216 Position; }
        public void DrawString(string ascii, short x, short y, byte size, Color color)
        {
            for (int l = 0; l < ascii.Length; l++)
            {
                var c = ascii[l];

                if ((c < 32) || (c > 126))
                {
                    c = '?';
                }
                for (byte i = 0; i < FONT_X; i++)
                {
                    byte temp = this.Font.Data[c - 0x20][i];
                    for (byte f = 0; f < 8; f++)
                    {
                        if (((temp >> f) & 0x01) > 0)
                        {
                            bytelist.Add(new Block()
                            {
                                Data = Drawing.ReadSolidColor(size, size, color),
                                Position = new Vec216((short)(x + (i * size)), (short)(y + (f * size)))
                            });
                        }
                    }
                }
                
                x += (byte)(size * (FONT_X - 2));
                // if x > screen width drop to a new line
            }
        }
        

        public override byte[] ReadInternal(int position, int maxlength)
        {
            throw new NotImplementedException();
        }

        public override bool ApplyPositionUpdates
        {
            get
            {
                return true;
            }
        }
    }
}
