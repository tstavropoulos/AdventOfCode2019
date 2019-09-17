using System;

namespace AoCTools
{
    public readonly struct Vector2D
    {
        public readonly double x;
        public readonly double y;

        public static readonly Vector2D Zero = new Vector2D(0.0, 0.0);
        public static readonly Vector2D XAxis = new Vector2D(1.0, 0.0);
        public static readonly Vector2D YAxis = new Vector2D(0.0, 1.0);

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2D((double x, double y) point) => new Vector2D(point.x, point.y);
        public static implicit operator (double x, double y)(Vector2D point) => (point.x, point.y);

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2D other))
            {
                return false;
            }

            return x == other.x && y == other.y;
        }

        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x}, {y})";

        public static bool operator ==(in Vector2D lhs, in Vector2D rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y;

        public static bool operator !=(in Vector2D lhs, in Vector2D rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y;

        public static Vector2D operator +(in Vector2D lhs, in Vector2D rhs) =>
            new Vector2D(lhs.x + rhs.x, lhs.y + rhs.y);
        public static Vector2D operator -(in Vector2D lhs, in Vector2D rhs) =>
            new Vector2D(lhs.x - rhs.x, lhs.y - rhs.y);
        public static Vector2D operator *(in Vector2D lhs, double rhs) =>
            new Vector2D(lhs.x * rhs, lhs.y * rhs);
        public static Vector2D operator *(double lhs, in Vector2D rhs) =>
            new Vector2D(lhs * rhs.x, lhs * rhs.y);
        public static Vector2D operator *(in Vector2D lhs, in Vector2D rhs) =>
            new Vector2D(lhs.x * rhs.x, lhs.y * rhs.y);
        public static Vector2D operator /(in Vector2D lhs, double rhs) =>
            new Vector2D(lhs.x / rhs, lhs.y / rhs);

        public double Length => Math.Sqrt(x * x + y * y);
    }
}
