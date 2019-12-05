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

        public readonly double Length => Math.Sqrt(x * x + y * y);

        public readonly Point2D ToPoint2D => new Point2D((int)x, (int)y);
        public readonly Point3D ToPoint3D => new Point3D((int)x, (int)y, 0);
        public readonly LongPoint2D ToLongPoint2D => new LongPoint2D((long)x, (long)y);
        public readonly LongPoint3D ToLongPoint3D => new LongPoint3D((long)x, (long)y, 0);
        public readonly Vector3D ToVector3D => new Vector3D(x, y, 0.0);

        public readonly Vector2D Normalized => this / Length;

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public readonly double Dot(in Vector2D other) => x * other.x + y * other.y;

        public static implicit operator Vector2D((double x, double y) point) => new Vector2D(point.x, point.y);
        public static implicit operator (double x, double y)(in Vector2D point) => (point.x, point.y);

        public readonly override bool Equals(object obj)
        {
            if (!(obj is Vector2D other))
            {
                return false;
            }

            return x == other.x && y == other.y;
        }

        public readonly override int GetHashCode() => HashCode.Combine(x, y);
        public readonly override string ToString() => $"({x}, {y})";

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

        public static Vector2D operator -(in Vector2D value) =>
            new Vector2D(-value.x, -value.y);

        public readonly void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }
    }
}
