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

        public long Length => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
    }
}
