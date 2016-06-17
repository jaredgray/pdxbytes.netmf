using System;
using Microsoft.SPOT;
using pdxbytes.Structures;

namespace pdxbytes.Structures
{
    public class Touch
    {
       
        public Touch() { }
        public Touch(Vec2 t1) { this.Touch1 = t1; }
        public Touch(Vec2 t1, Vec2 t2) { this.Touch1 = t1; this.Touch2 = t2; this.Touched = DateTime.Now; }

        public Vec2 Touch1 { get; set; }
        public Vec2 Touch2 { get; set; }

        public DateTime Touched { get; set; }

    }
}
