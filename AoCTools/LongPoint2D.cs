using System;

namespace AoCTools
{
    public readonly struct LongPoint2D
    {
        public readonly long x;
        public readonly long y;

        public static readonly LongPoint2D Zero = new LongPoint2D(0, 0);
        public static readonly LongPoint2D XAxis = new LongPoint2D(1, 0);
        public static readonly LongPoint2D YAxis = new LongPoint2D(0, 1);

        public long Length => Math.Abs(x) + Math.Abs(y);

        public Point2D ToPoint2D => new Point2D((int)x, (int)y);
        public Point3D ToPoint3D => new Point3D((int)x, (int)y, 0);
        public LongPoint3D ToLongPoint3D => new LongPoint3D(x, y, 0);
        public Vector2D ToVector2D => new Vector2D(x, y);
        public Vector3D ToVector3D => new Vector3D(x, y, 0);

        public LongPoint2D AxisStep
        {
            get
            {
                if (x == 0 && y == 0)
                {
                    return Zero;
                }
                else if (Math.Abs(x) >= Math.Abs(y))
                {
                    //X-Axis takes priority
                    return x > 0 ? XAxis : -XAxis;
                }

                return y > 0 ?  YAxis : -YAxis;
            }
        }

        public LongPoint2D(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator LongPoint2D((long x, long y) point) => new LongPoint2D(point.x, point.y);
        public static implicit operator (long x, long y)(LongPoint2D point) => (point.x, point.y);

        public override bool Equals(object obj)
        {
            if (!(obj is LongPoint2D other))
            {
                return false;
            }

            return x == other.x && y == other.y;
        }

        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x}, {y})";

        public static bool operator ==(in LongPoint2D lhs, in LongPoint2D rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y;

        public static bool operator !=(in LongPoint2D lhs, in LongPoint2D rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y;

        public static LongPoint2D operator +(in LongPoint2D lhs, in LongPoint2D rhs) =>
            new LongPoint2D(lhs.x + rhs.x, lhs.y + rhs.y);
        public static LongPoint2D operator -(in LongPoint2D lhs, in LongPoint2D rhs) =>
            new LongPoint2D(lhs.x - rhs.x, lhs.y - rhs.y);
        public static LongPoint2D operator *(in LongPoint2D lhs, in LongPoint2D rhs) =>
            new LongPoint2D(lhs.x * rhs.x, lhs.y * rhs.y);
        public static LongPoint2D operator *(in LongPoint2D lhs, long rhs) =>
            new LongPoint2D(lhs.x * rhs, lhs.y * rhs);
        public static LongPoint2D operator *(long lhs, in LongPoint2D rhs) =>
            new LongPoint2D(rhs.x * lhs, rhs.y * lhs);

        public static LongPoint2D operator -(in LongPoint2D value) =>
            new LongPoint2D(-value.x, -value.y);

        public void Deconstruct(out long x, out long y)
        {
            x = this.x;
            y = this.y;
        }
    }
}
