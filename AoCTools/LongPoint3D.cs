using System;

namespace AoCTools
{
    public readonly struct LongPoint3D
    {
        public readonly long x;
        public readonly long y;
        public readonly long z;

        public static readonly LongPoint3D Zero = new LongPoint3D(0, 0, 0);
        public static readonly LongPoint3D XAxis = new LongPoint3D(1, 0, 0);
        public static readonly LongPoint3D YAxis = new LongPoint3D(0, 1, 0);
        public static readonly LongPoint3D ZAxis = new LongPoint3D(0, 0, 1);

        public long Length => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public Point2D ToPoint2D => new Point2D((int)x, (int)y);
        public Point3D ToPoint3D => new Point3D((int)x, (int)y, (int)z);
        public LongPoint2D ToLongPoint2D => new LongPoint2D(x, y);
        public Vector2D ToVector2D => new Vector2D(x, y);
        public Vector3D ToVector3D => new Vector3D(x, y, z);

        public LongPoint3D AxisStep
        {
            get
            {
                if (x == 0 && y == 0 && z == 0)
                {
                    return Zero;
                }
                else if (Math.Abs(x) >= Math.Abs(y) && Math.Abs(x) >= Math.Abs(z))
                {
                    //X-Axis takes priority
                    return x > 0 ? XAxis : -XAxis;
                }
                else if (Math.Abs(y) >= Math.Abs(z))
                {
                    //Y-Axis has next priority
                    return y > 0 ? YAxis : -YAxis;
                }

                return z > 0 ? ZAxis : -ZAxis;
            }
        }

        public LongPoint3D(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator LongPoint3D((long x, long y, long z) point) =>
            new LongPoint3D(point.x, point.y, point.z);

        public static implicit operator (long x, long y, long z)(LongPoint3D point) =>
            (point.x, point.y, point.z);

        public override bool Equals(object obj)
        {
            if (!(obj is LongPoint3D other))
            {
                return false;
            }

            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode() => HashCode.Combine(x, y, z);
        public override string ToString() => $"({x}, {y}, {z})";

        public static bool operator ==(in LongPoint3D lhs, in LongPoint3D rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;

        public static bool operator !=(in LongPoint3D lhs, in LongPoint3D rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;

        public static LongPoint3D operator +(in LongPoint3D lhs, in LongPoint3D rhs) =>
            new LongPoint3D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        public static LongPoint3D operator -(in LongPoint3D lhs, in LongPoint3D rhs) =>
            new LongPoint3D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        public static LongPoint3D operator *(in LongPoint3D lhs, in LongPoint3D rhs) =>
            new LongPoint3D(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        public static LongPoint3D operator *(in LongPoint3D lhs, long rhs) =>
            new LongPoint3D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        public static LongPoint3D operator *(long lhs, in LongPoint3D rhs) =>
            new LongPoint3D(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);

        public static LongPoint3D operator -(in LongPoint3D value) =>
            new LongPoint3D(-value.x, -value.y, -value.z);

        public void Deconstruct(out long x, out long y, out long z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }
}
