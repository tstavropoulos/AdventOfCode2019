using System;

namespace AoCTools
{
    public readonly struct Point3D
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public static readonly Point3D Zero = new Point3D(0, 0, 0);
        public static readonly Point3D XAxis = new Point3D(1, 0, 0);
        public static readonly Point3D YAxis = new Point3D(0, 1, 0);
        public static readonly Point3D ZAxis = new Point3D(0, 0, 1);

        public int Length => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public Point2D ToPoint2D => new Point2D(x, y);
        public LongPoint2D ToLongPoint2D => new LongPoint2D(x, y);
        public LongPoint3D ToLongPoint3D => new LongPoint3D(x, y, z);
        public Vector2D ToVector2D => new Vector2D(x, y);
        public Vector3D ToVector3D => new Vector3D(x, y, z);

        public Point3D AxisStep
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

        public Point3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Point3D((int x, int y, int z) point) =>
            new Point3D(point.x, point.y, point.z);

        public static implicit operator (int x, int y, int z)(Point3D point) =>
            (point.x, point.y, point.z);

        public override bool Equals(object obj)
        {
            if (!(obj is Point3D other))
            {
                return false;
            }

            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode() => HashCode.Combine(x, y, z);
        public override string ToString() => $"({x}, {y}, {z})";

        public static bool operator ==(in Point3D lhs, in Point3D rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;

        public static bool operator !=(in Point3D lhs, in Point3D rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;

        public static Point3D operator +(in Point3D lhs, in Point3D rhs) =>
            new Point3D(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        public static Point3D operator -(in Point3D lhs, in Point3D rhs) =>
            new Point3D(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        public static Point3D operator *(in Point3D lhs, in Point3D rhs) =>
            new Point3D(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        public static Point3D operator *(in Point3D lhs, int rhs) =>
            new Point3D(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        public static Point3D operator *(int lhs, in Point3D rhs) =>
            new Point3D(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);

        public static Point3D operator -(in Point3D value) =>
            new Point3D(-value.x, -value.y, -value.z);

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }
    }
}
