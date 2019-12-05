using System;

namespace AoCTools
{
    public readonly struct Vector3D
    {
        public readonly double x;
        public readonly double y;
        public readonly double z;

        public static readonly Vector3D Zero = new Vector3D(0.0, 0.0, 0.0);
        public static readonly Vector3D XAxis = new Vector3D(1.0, 0.0, 0.0);
        public static readonly Vector3D YAxis = new Vector3D(0.0, 1.0, 0.0);
        public static readonly Vector3D ZAxis = new Vector3D(0.0, 0.0, 1.0);

        public readonly double Length => Math.Sqrt(x * x + y * y + z * z);

        public readonly Point2D ToPoint2D => new Point2D((int)x, (int)y);
        public readonly Point3D ToPoint3D => new Point3D((int)x, (int)y, (int)z);
        public readonly LongPoint2D ToLongPoint2D => new LongPoint2D((long)x, (long)y);
        public readonly LongPoint3D ToLongPoint3D => new LongPoint3D((long)x, (long)y, (long)z);
        public readonly Vector2D ToVector2D => new Vector2D(x, y);

        public readonly Vector3D Normalized => this / Length;

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public readonly double Dot(in Vector3D other) => x * other.x + y * other.y + z * other.z;
        public readonly Vector3D Cross(in Vector3D other) =>
            new Vector3D(
                x: y * other.z - z * other.y,
                y: z * other.x - x * other.z,
                z: x * other.y - y * other.x);

        public static implicit operator Vector3D((double x, double y, double z) point) =>
            new Vector3D(point.x, point.y, point.z);

        public static implicit operator (double x, double y, double z)(in Vector3D point) =>
            (point.x, point.y, point.z);

        public readonly override bool Equals(object obj)
        {
            if (!(obj is Vector3D other))
            {
                return false;
            }

            return x == other.x && y == other.y && z == other.z;
        }

        public readonly override int GetHashCode() => HashCode.Combine(x, y, z);
        public readonly override string ToString() => $"({x}, {y}, {z})";

        public static bool operator ==(in Vector3D lhs, in Vector3D rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;

        public static bool operator !=(in Vector3D lhs, in Vector3D rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;

        public static Vector3D operator +(in Vector3D lhs, in Vector3D rhs) =>
            new Vector3D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        public static Vector3D operator -(in Vector3D lhs, in Vector3D rhs) =>
            new Vector3D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        public static Vector3D operator *(in Vector3D lhs, double rhs) =>
            new Vector3D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        public static Vector3D operator *(double lhs, in Vector3D rhs) =>
            new Vector3D(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        public static Vector3D operator *(in Vector3D lhs, in Vector3D rhs) =>
            new Vector3D(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        public static Vector3D operator /(in Vector3D lhs, double rhs) =>
            new Vector3D(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);

        public readonly void Deconstruct(out double x, out double y, out double z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }
}
