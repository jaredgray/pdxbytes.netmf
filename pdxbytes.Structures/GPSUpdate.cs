using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public class GPSUpdate
    {
        public DateTime TimestampUTC { get; set; }
        public GPSPlot Point { get; set; }
    }
}
