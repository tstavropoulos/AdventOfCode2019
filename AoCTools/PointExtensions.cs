using System;
using System.Collections.Generic;
using System.Text;

namespace AoCTools
{
    public static class PointExtensions
    {
        #region Point2D Extensions

        public static Point2D MinCoordinate(this IEnumerable<Point2D> points)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;

            foreach (Point2D point in points)
            {
                if (point.x < minX)
                {
                    minX = point.x;
                }

                if (point.y < minY)
                {
                    minY = point.y;
                }
            }

            return new Point2D(minX, minY);
        }

        public static Point2D MaxCoordinate(this IEnumerable<Point2D> points)
        {
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (Point2D point in points)
            {
                if (point.x > maxX)
                {
                    maxX = point.x;
                }

                if (point.y > maxY)
                {
                    maxY = point.y;
                }
            }

            return new Point2D(maxX, maxY);
        }

        public static Point2D Rotate(in this Point2D point, bool right)
        {
            if (right)
            {
                return new Point2D(point.y, -point.x);
            }
            else
            {
                return new Point2D(-point.y, point.x);
            }
        }

        #endregion Point2D Extensions
        #region Point3D Extensions

        public static Point3D MinCoordinate(this IEnumerable<Point3D> points)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int minZ = int.MaxValue;

            foreach (Point3D point in points)
            {
                if (point.x < minX)
                {
                    minX = point.x;
                }

                if (point.y < minY)
                {
                    minY = point.y;
                }

                if (point.z < minZ)
                {
                    minZ = point.z;
                }
            }

            return new Point3D(minX, minY, minZ);
        }

        public static Point3D MaxCoordinate(this IEnumerable<Point3D> points)
        {
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            int maxZ = int.MinValue;

            foreach (Point3D point in points)
            {
                if (point.x > maxX)
                {
                    maxX = point.x;
                }

                if (point.y > maxY)
                {
                    maxY = point.y;
                }

                if (point.z > maxZ)
                {
                    maxZ = point.z;
                }
            }

            return new Point3D(maxX, maxY, maxZ);
        }

        #endregion Point3D Extensions
        #region LongPoint2D Extensions

        public static LongPoint2D MinCoordinate(this IEnumerable<LongPoint2D> points)
        {
            long minX = long.MaxValue;
            long minY = long.MaxValue;

            foreach (LongPoint2D point in points)
            {
                if (point.x < minX)
                {
                    minX = point.x;
                }

                if (point.y < minY)
                {
                    minY = point.y;
                }
            }

            return new LongPoint2D(minX, minY);
        }

        public static LongPoint2D MaxCoordinate(this IEnumerable<LongPoint2D> points)
        {
            long maxX = long.MinValue;
            long maxY = long.MinValue;

            foreach (LongPoint2D point in points)
            {
                if (point.x > maxX)
                {
                    maxX = point.x;
                }

                if (point.y > maxY)
                {
                    maxY = point.y;
                }
            }

            return new LongPoint2D(maxX, maxY);
        }

        public static LongPoint2D Rotate(in this LongPoint2D point, bool right)
        {
            if (right)
            {
                return new LongPoint2D(point.y, -point.x);
            }
            else
            {
                return new LongPoint2D(-point.y, point.x);
            }
        }

        #endregion LongPoint2D Extensions
        #region LongPoint3D Extensions

        public static LongPoint3D MinCoordinate(this IEnumerable<LongPoint3D> points)
        {
            long minX = long.MaxValue;
            long minY = long.MaxValue;
            long minZ = long.MaxValue;

            foreach (LongPoint3D point in points)
            {
                if (point.x < minX)
                {
                    minX = point.x;
                }

                if (point.y < minY)
                {
                    minY = point.y;
                }

                if (point.z < minZ)
                {
                    minZ = point.z;
                }
            }

            return new LongPoint3D(minX, minY, minZ);
        }

        public static LongPoint3D MaxCoordinate(this IEnumerable<LongPoint3D> points)
        {
            long maxX = long.MinValue;
            long maxY = long.MinValue;
            long maxZ = long.MinValue;

            foreach (LongPoint3D point in points)
            {
                if (point.x > maxX)
                {
                    maxX = point.x;
                }

                if (point.y > maxY)
                {
                    maxY = point.y;
                }

                if (point.z > maxZ)
                {
                    maxZ = point.z;
                }
            }

            return new LongPoint3D(maxX, maxY, maxZ);
        }

        #endregion LongPoint3D Extensions
        #region Vector2D Extensions

        public static Vector2D MinCoordinate(this IEnumerable<Vector2D> points)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            foreach (Vector2D point in points)
            {
                if (point.x < minX)
                {
                    minX = point.x;
                }

                if (point.y < minY)
                {
                    minY = point.y;
                }
            }

            return new Vector2D(minX, minY);
        }

        public static Vector2D MaxCoordinate(this IEnumerable<Vector2D> points)
        {
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (Vector2D point in points)
            {
                if (point.x > maxX)
                {
                    maxX = point.x;
                }

                if (point.y > maxY)
                {
                    maxY = point.y;
                }
            }

            return new Vector2D(maxX, maxY);
        }

        public static Vector2D Rotate(in this Vector2D point, bool right)
        {
            if (right)
            {
                return new Vector2D(point.y, -point.x);
            }
            else
            {
                return new Vector2D(-point.y, point.x);
            }
        }

        #endregion Vector2D Extensions
        #region Vector3D Extensions

        public static Vector3D MinCoordinate(this IEnumerable<Vector3D> points)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double minZ = double.MaxValue;

            foreach (Vector3D point in points)
            {
                if (point.x < minX)
                {
                    minX = point.x;
                }

                if (point.y < minY)
                {
                    minY = point.y;
                }

                if (point.z < minZ)
                {
                    minZ = point.z;
                }
            }

            return new Vector3D(minX, minY, minZ);
        }

        public static Vector3D MaxCoordinate(this IEnumerable<Vector3D> points)
        {
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double maxZ = double.MinValue;

            foreach (Vector3D point in points)
            {
                if (point.x > maxX)
                {
                    maxX = point.x;
                }

                if (point.y > maxY)
                {
                    maxY = point.y;
                }

                if (point.z > maxZ)
                {
                    maxZ = point.z;
                }
            }

            return new Vector3D(maxX, maxY, maxZ);
        }

        #endregion Vector2D Extensions
    }
}
