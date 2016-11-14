using System;
using Microsoft.SPOT;
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Shapes
{
    public class DropShadowRectangle : Rectangle
    {
        public DropShadowRectangle(short x, short y, short width, short height, byte zindex = 1, Color backgroundcolor = null, Color shadowcolor = null, byte shadowSize = 2) : base(x, y, width, height, zindex, backgroundcolor)
        {
            if (null == shadowcolor)
                shadowcolor = Palette.ShadowMediumDark;
            ShadowColor = shadowcolor;
            ShadowEdges = Edges.Bottom | Edges.Right;
        }

        public Color ShadowColor { get; set; }
        public byte ShadowSize { get; set; }
        public Edges ShadowEdges { get; set; }

        int readcount = 0;
        //public override ushort[] ReadInternal(int position, int maxlength)
        //{
        //    var buffer = base.ReadInternal(position, maxlength);
        //    if (null == buffer)
        //        return null;
        //    var bitdepth = sizeof(short);
        //    // the buffer will not necessarily be the size of the image
        //    // but, we can get the current position from our base display buffer
        //    var currentdim = base.GetDimensions(buffer);
        //    var offsetY = currentdim.RelativeUIPosition.Y == 0 ? this.ShadowSize : 0;
        //    var offset = this.Width * bitdepth * offsetY;
        //    var drawtopedge = ((Edges.Top & this.ShadowEdges) == Edges.Top);
        //    var drawrightedge = ((Edges.Right & this.ShadowEdges) == Edges.Right);
        //    var drawbottomedge = ((Edges.Bottom & this.ShadowEdges) == Edges.Bottom);
        //    var drawleftedge = ((Edges.Left & this.ShadowEdges) == Edges.Left);
        //    ++readcount;
        //    this.ShadowColor.SetPoint();
        //    var horizontal = 0;
        //    for (int row = 0 + offsetY; row < currentdim.Height; row++)
        //    {
        //        if (drawbottomedge && row >= (this.Height - (currentdim.RelativeUIPosition.Y + this.ShadowSize)))
        //        {
        //            var color = this.ShadowColor;
        //            for (int x = 0; x < this.Width; x++)
        //            {
        //                // on the first row, don't write all the way to the end
        //                if (horizontal == 0 && x == this.Width - 1)
        //                    continue;
        //                var baseoffset = offset + bitdepth * x;
        //                buffer[baseoffset] = (byte)(color.Value >> 8);
        //                buffer[baseoffset + 1] = (byte)(color.Value);
        //            }
        //                ++horizontal;
        //            this.ShadowColor.Lighten(15);
        //        }
        //        else if(((Edges.Right & this.ShadowEdges) == Edges.Right))
        //        {
        //            for (int x = this.Width - this.ShadowSize; x < this.Width; x++)
        //            {
        //                var baseoffset = offset + bitdepth * x;
        //                buffer[baseoffset] = (byte)(this.ShadowColor.Value >> 8);
        //                buffer[baseoffset + 1] = (byte)(this.ShadowColor.Value);
        //                this.ShadowColor.Lighten(15);
        //            }
        //            this.ShadowColor.Reset();
        //        }
        //        offset += this.Width * bitdepth;
        //    }
        //    this.ShadowColor.Reset();
        //    return buffer;
        //}
    }
}
