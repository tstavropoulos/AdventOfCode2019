using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day10
{
    class Program
    {
        private const string inputFile = @"../../../../input10.txt";
        static readonly HashSet<Point2D> asteroids = new HashSet<Point2D>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 10 - Monitoring Station");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            Dictionary<Point2D, int> hitCount = new Dictionary<Point2D, int>();
            //Get all prime factors of DX and DY

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        asteroids.Add((x, y));
                    }
                }
            }

            foreach (Point2D origin in asteroids)
            {
                int hit = 0;
                foreach (Point2D destination in asteroids)
                {
                    if (origin == destination)
                    {
                        continue;
                    }

                    if (CanSee(origin, destination))
                    {
                        hit++;
                    }
                }

                hitCount.Add(origin, hit);
            }

            int max = hitCount.Values.Max();

            Console.WriteLine();

            Console.WriteLine($"The answer is: {max}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            var sw = System.Diagnostics.Stopwatch.StartNew();

            Point2D basePoint = hitCount.Where(x => x.Value == max).First().Key;

            int vaporizedCount = 0;
            Vector3D lastVaporized = (0, 0, 0);
            Point2D lastFull = (0, 0);

            var input = asteroids.Where(x => x != basePoint).ToArray();
            var output = input.Select(x => x - basePoint).Select(x => x.ToVector3D.Normalized).ToArray();

            Dictionary<Point2D, Vector3D> map = new Dictionary<Point2D, Vector3D>();

            for (int i = 0; i < input.Length; i++)
            {
                map.Add(input[i], output[i]);
            }

            if (map.ContainsValue((0, 1, 0)))
            {
                Point2D eliminated = map.Where(x => x.Value == (0, -1, 0)).OrderBy(x => (basePoint - x.Key).Length).First().Key;
                lastVaporized = map[eliminated];
                map.Remove(eliminated);

                vaporizedCount++;
            }

            while (vaporizedCount < 200)
            {
                Vector3D next = map.Values
                    .Select(value => (value - lastVaporized, value))
                    .OrderBy(x => x.Item1.Length)
                    .Where(x => x.Item1.Length > 0.001)
                    .Where(x => x.Item1.Cross(lastVaporized).z < 0.0).First().value;

                lastFull = map.Where(x => x.Value == next).OrderBy(x => (basePoint - x.Key).Length).First().Key;

                lastVaporized = map[lastFull];
                map.Remove(lastFull);
                vaporizedCount++;
            }

            sw.Stop();

            Console.WriteLine($"That took {sw.Elapsed.TotalMilliseconds} ms");

            Console.WriteLine($"The answer is: {lastFull.x * 100 + lastFull.y}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static bool CanSee(Point2D start, Point2D end)
        {
            Point2D slope = end - start;

            int deltaY = slope.y;
            int deltaX = slope.x;

            int gcd = GCD(Math.Abs(deltaX), Math.Abs(deltaY));

            Point2D reducedSlope = new Point2D(deltaX / gcd, deltaY / gcd);

            Point2D point = start + reducedSlope;

            while (point != end)
            {
                if (asteroids.Contains(point))
                {
                    return false;
                }

                point += reducedSlope;
            }

            return true;
        }

        //Euclid's Algorith
        public static int GCD(int a, int b)
        {
            //Short cut to handling 0 case
            if (a == 0 || b == 0)
            {
                return a == 0 ? b : a;
            }

            if (b > a)
            {
                (a, b) = (b, a);
            }

            while (b != 0)
            {
                (a, b) = (b, a % b);
            }

            return a;
        }
    }
}
