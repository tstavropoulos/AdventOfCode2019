using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day03
{
    class Program
    {
        private const string inputFile = @"../../../../input03.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 3 - Crossed Wires");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            Dictionary<Point2D, int> wire1Delays = new Dictionary<Point2D, int>();
            List<(Point2D point, int delay)> totalDelays = new List<(Point2D point, int delay)>();

            string[] lines = File.ReadAllLines(inputFile);

            Point2D[] wire1Dirs = lines[0].Split(",").Select(ParsePoint).ToArray();
            Point2D[] wire2Dirs = lines[1].Split(",").Select(ParsePoint).ToArray();

            Point2D currentPoint = Point2D.Zero;
            int delay = 0;

            foreach (Point2D dir in wire1Dirs)
            {
                int length = dir.Length;
                Point2D step = dir.AxisStep;

                for (int i = 0; i < length; i++)
                {
                    currentPoint += step;
                    delay++;
                    if (!wire1Delays.ContainsKey(currentPoint))
                    {
                        wire1Delays.Add(currentPoint, delay);
                    }
                }
            }

            currentPoint = Point2D.Zero;
            delay = 0;

            foreach (Point2D dir in wire2Dirs)
            {
                int length = dir.Length;
                Point2D step = dir.AxisStep;

                for (int i = 0; i < length; i++)
                {
                    currentPoint += step;
                    delay++;
                    if (wire1Delays.ContainsKey(currentPoint))
                    {
                        totalDelays.Add((currentPoint, delay + wire1Delays[currentPoint]));
                    }
                }
            }

            int closestIntersection = totalDelays.Select(x => x.point.Length).OrderBy(x => x).First();

            Console.WriteLine($"The closest intersection is at distance: {closestIntersection}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int earliestIntersection = totalDelays.Select(x => x.delay).OrderBy(x => x).First();

            Console.WriteLine($"The earliest intersection occurs at: {earliestIntersection}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public static Point2D ParsePoint(string segment)
        {
            char dir = segment[0];
            int number = int.Parse(segment.Substring(1));

            switch (dir)
            {
                case 'R': return number * Point2D.XAxis;
                case 'L': return -number * Point2D.XAxis;
                case 'U': return number * Point2D.YAxis;
                case 'D': return -number * Point2D.YAxis;

                default: throw new Exception();
            }
        }
    }
}
