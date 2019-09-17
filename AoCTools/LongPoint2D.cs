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

        public long Length => Math.Abs(x) + Math.Abs(y);
    }
}
