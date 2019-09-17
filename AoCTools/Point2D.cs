using System;

namespace AoCTools
{
    public readonly struct Point2D
    {
        public readonly int x;
        public readonly int y;

        public static readonly Point2D Zero = new Point2D(0, 0);
        public static readonly Point2D XAxis = new Point2D(1, 0);
        public static readonly Point2D YAxis = new Point2D(0, 1);

        public Point2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Point2D((int x, int y) point) => new Point2D(point.x, point.y);
        public static implicit operator (int x, int y)(Point2D point) => (point.x, point.y);

        public override bool Equals(object obj)
        {
            if (!(obj is Point2D other))
            {
                return false;
            }

            return x == other.x && y == other.y;
        }

        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x}, {y})";

        public static bool operator ==(in Point2D lhs, in Point2D rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y;

        public static bool operator !=(in Point2D lhs, in Point2D rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y;

        public static Point2D operator +(in Point2D lhs, in Point2D rhs) =>
            new Point2D(lhs.x + rhs.x, lhs.y + rhs.y);
        public static Point2D operator -(in Point2D lhs, in Point2D rhs) =>
            new Point2D(lhs.x - rhs.x, lhs.y - rhs.y);
        public static Point2D operator *(in Point2D lhs, in Point2D rhs) =>
            new Point2D(lhs.x * rhs.x, lhs.y * rhs.y);
        public static Point2D operator *(in Point2D lhs, int rhs) =>
            new Point2D(lhs.x * rhs, lhs.y * rhs);
        public static Point2D operator *(int lhs, in Point2D rhs) =>
            new Point2D(rhs.x * lhs, rhs.y * lhs);

        public int Length => Math.Abs(x) + Math.Abs(y);
    }
}
