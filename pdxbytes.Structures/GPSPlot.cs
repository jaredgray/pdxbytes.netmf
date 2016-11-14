using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public class GPSPlot
    {
        public GPSPosition X { get; set; }

        public GPSPosition Y { get; set; }


        public override string ToString()
        {
            return X.Value + ", " + Y.Value;
        }
    }
}
