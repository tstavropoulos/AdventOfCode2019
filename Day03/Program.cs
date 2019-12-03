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
            Console.WriteLine("Day 3");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            string wire1 = lines[0];
            string wire2 = lines[1];


            LongPoint2D[] wire1Dirs = lines[0]
                .Split(",").Select(ParseLongPoint).ToArray();
            LongPoint2D[] wire2Dirs = lines[1]
                .Split(",").Select(ParseLongPoint).ToArray();

            HashSet<LongPoint2D> visitedPoints = new HashSet<LongPoint2D>();
            HashSet<LongPoint2D> collisionPoints = new HashSet<LongPoint2D>();

            Dictionary<LongPoint2D, long> wire1Delays = new Dictionary<LongPoint2D, long>();
            Dictionary<LongPoint2D, long> totalDelays = new Dictionary<LongPoint2D, long>();


            LongPoint2D currentPoint = LongPoint2D.Zero;
            long delay = 0;

            foreach(LongPoint2D dir in wire1Dirs)
            {
                long length = dir.Length;
                LongPoint2D direction;

                if (dir.x > 0)
                {
                    direction = LongPoint2D.XAxis;
                }
                else if (dir.x < 0)
                {
                    direction = -1 * LongPoint2D.XAxis;
                }
                else if (dir.y > 0)
                {
                    direction = LongPoint2D.YAxis;
                }
                else
                {
                    direction = -1 * LongPoint2D.YAxis;
                }

                for (int i = 0; i < length; i++)
                {
                    currentPoint += direction;
                    delay++;
                    visitedPoints.Add(currentPoint);
                    if (!wire1Delays.ContainsKey(currentPoint))
                    {
                        wire1Delays.Add(currentPoint, delay);
                    }
                }
            }

            currentPoint = LongPoint2D.Zero;
            delay = 0;

            foreach (LongPoint2D dir in wire2Dirs)
            {
                long length = dir.Length;
                LongPoint2D direction;

                if (dir.x > 0)
                {
                    direction = LongPoint2D.XAxis;
                }
                else if (dir.x < 0)
                {
                    direction = -1 * LongPoint2D.XAxis;
                }
                else if (dir.y > 0)
                {
                    direction = LongPoint2D.YAxis;
                }
                else
                {
                    direction = -1 * LongPoint2D.YAxis;
                }

                for (int i = 0; i < length; i++)
                {
                    currentPoint += direction;
                    delay++;
                    if (visitedPoints.Contains(currentPoint))
                    {
                        collisionPoints.Add(currentPoint);
                        totalDelays.Add(currentPoint, delay + wire1Delays[currentPoint]);
                    }
                }
            }

            LongPoint2D intersection = collisionPoints.OrderBy(x => Math.Abs(x.x) + Math.Abs(x.y)).First();



            Console.WriteLine($"The answer is: {Math.Abs(intersection.x) + Math.Abs(intersection.y)}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            LongPoint2D newInt = totalDelays.OrderBy(x => x.Value).First().Key;


            Console.WriteLine($"The answer is: {totalDelays.OrderBy(x => x.Value).First().Value}");


            Console.WriteLine();
            Console.ReadKey();
        }

        public static LongPoint2D ParseLongPoint(string segment)
        {
            char dir = segment[0];
            long number = long.Parse(segment.Substring(1));

            switch (dir)
            {
                case 'R': return number * LongPoint2D.XAxis;
                case 'L': return -number * LongPoint2D.XAxis;
                case 'U': return number * LongPoint2D.YAxis;
                case 'D': return -number * LongPoint2D.YAxis;

                default: throw new Exception();
            }
        }
    }
}
