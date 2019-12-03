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

        public int Length => Math.Abs(x) + Math.Abs(y);

        public Point3D ToPoint3D => new Point3D(x, y, 0);
        public LongPoint2D ToLongPoint2D => new LongPoint2D(x, y);
        public LongPoint3D ToLongPoint3D => new LongPoint3D(x, y, 0);
        public Vector2D ToVector2D => new Vector2D(x, y);
        public Vector3D ToVector3D => new Vector3D(x, y, 0);

        public Point2D AxisStep
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

                return y > 0 ? YAxis : -YAxis;
            }
        }

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

        public static Point2D operator -(in Point2D value) =>
            new Point2D(-value.x, -value.y);

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }
    }
}
