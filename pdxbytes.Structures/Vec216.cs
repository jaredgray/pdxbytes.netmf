using System;
using Microsoft.SPOT;

namespace pdxbytes.Structures
{
    public struct Vec216
    {
        public Vec216(short x, short y) { this.X = x; this.Y = y; }

        public short X { get; set; }
        public short Y { get; set; }

        public override bool Equals(object obj)
        {
            //var other = (Vec216)obj;
            //if (null == other) return false;
            //return other.X == this.X && other.Y == this.Y;
            return this == (Vec216)obj;
        }
        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }

        public static bool operator ==(Vec216 a, Vec216 b) { return a.X == b.X && a.Y == b.Y; }
        public static bool operator !=(Vec216 a, Vec216 b) { return a.X != b.X || a.Y != b.Y; }
    }
}
