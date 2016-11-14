using System;
using pdxbytes.Structures;

namespace pdxbytes.Graphics.Text
{
    /// <summary>
    /// represents a relative area as a container for display objects
    /// </summary>
    public class DisplayBlock
    {
        public DisplayBlock() { }
        public DisplayBlock(uint width, uint height) { Width = width; Heignt = height; }

        public uint Width { get; set; }
        public uint Heignt { get; set; }
    }
    public class Character
    {

        public Character() { }

        public DisplayRegion Dimensions { get; set; }

        public byte Size { get; set; }
        public Font Font { get; set; }
        public Color Color { get; set; }

        public void Draw(char character, DisplayRegion clip, byte[] buffer)
        {

            /*
                the natural location of the character comes from the Dimensions property, this is where the character is placed logically on the screen.
                the clip that is passed in enforces the character to only draw the portion that is relavant to it
                

                ____________________________________________
                |                                          |
                |                                          |
                |                                          |
                |                                          |
                |       __Display Region_______            |
                |       |                     |            |
                |       |        ________     |            |
                |       |        |       |    |            |
                |       |        |       |    |            |
                |       ---------|       |-----            |
                |                |       |                 |
                |                ---------                 |
                |                character                 |
                |                                          |
                |                                          |
                --------------------------------------------
            */

            var startdraw_x = Drawing.Max(this.Dimensions.Position.X, clip.Position.X);
            var startdraw_y = Drawing.Max(this.Dimensions.Position.Y, clip.Position.Y);

            var targetregion = Drawing.Clip(this.Dimensions, clip);

            

            for (byte i = 0; i < this.Font.Size; i++)
            {
                byte temp = this.Font.Data[character - 0x20][i];
                for (byte f = 0; f < 8; f++)
                {
                    if (((temp >> f) & 0x01) > 0)
                    {
                        // todo: determine position in target buffer
                        var position = 0;

                        var data = Drawing.ReadSolidColor(this.Size, this.Size, this.Color);
                        throw new NotImplementedException();// need to implement copying the data to the buffer
                       // Array.Copy(data, 0, buffer, position, data.Length);                        
                    }
                }
            }
        }
    }
}
