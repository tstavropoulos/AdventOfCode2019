using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;
using AoCTools.IntCode;

namespace Day15
{
    class Program
    {
        private const string inputFile = @"../../../../input15.txt";

        static Dictionary<Point2D, bool> grid = new Dictionary<Point2D, bool>();

        static Point2D currentPosition = new Point2D(0, 0);
        static Point2D targetPosition = new Point2D(0, 0);

        static IEnumerator<int> movement;

        static int lastOutput = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 15 - Oxygen System");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            grid[(0, 0)] = true;

            IntCode machine = new IntCode(
                name: "Star 1",
                regs,
                Array.Empty<long>(),
                input: GetInput,
                output: x => lastOutput = (int)x);

            movement = ExploreMap().GetEnumerator();


            machine.SyncRun();

            currentPosition = (0, 0);

            Console.WriteLine($"The answer is: {TravelTo(targetPosition).Count()}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            Console.WriteLine($"The answer is: {FillGridFrom(targetPosition)}");


            Console.WriteLine();
            Console.ReadKey();
        }

        static long GetInput()
        {
            if (movement.MoveNext())
            {
                return movement.Current;
            }

            return 99;
        }

        static IEnumerable<int> ExploreMap()
        {
            Queue<Point2D> pointsToExplore = new Queue<Point2D>();

            pointsToExplore.Enqueue(currentPosition);

            while (pointsToExplore.Count > 0)
            {
                Point2D nextPoint = pointsToExplore.Dequeue();

                IEnumerable<int> travel = TravelTo(nextPoint);

                foreach (int dir in travel)
                {
                    yield return dir;
                }

                //Now at target position

                int direction = 0;
                foreach (Point2D next in GetAdjacents(currentPosition))
                {
                    direction++;

                    if (!grid.ContainsKey(next))
                    {
                        yield return direction;

                        if (lastOutput == 0)
                        {
                            grid.Add(next, false);
                            continue;
                        }

                        grid.Add(next, true);

                        if (lastOutput == 2)
                        {
                            targetPosition = next;
                        }

                        if (!pointsToExplore.Contains(next))
                        {
                            pointsToExplore.Enqueue(next);
                        }

                        yield return InvertDirection(direction);
                    }
                }
            }

        }

        static IEnumerable<int> TravelTo(Point2D destination)
        {
            if (currentPosition == destination)
            {
                yield break;
            }

            Dictionary<Point2D, int> travelMap = new Dictionary<Point2D, int>();
            Queue<Point2D> travelPoints = new Queue<Point2D>();
            travelPoints.Enqueue(destination);
            travelMap.Add(destination, 0);

            while (travelPoints.Count > 0)
            {
                Point2D current = travelPoints.Dequeue();

                if (current == currentPosition)
                {
                    break;
                }

                int distance = travelMap[current] + 1;

                foreach (Point2D next in GetAdjacents(current))
                {
                    if (!grid.GetValueOrDefault(next))
                    {
                        continue;
                    }

                    if (travelMap.GetValueOrDefault(next, int.MaxValue) > distance)
                    {
                        travelMap[next] = distance;
                        if (!travelPoints.Contains(next))
                        {
                            travelPoints.Enqueue(next);
                        }
                    }
                }
            }

            while (currentPosition != destination)
            {
                int distance = travelMap[currentPosition];

                int direction = 0;

                foreach (Point2D next in GetAdjacents(currentPosition))
                {
                    direction++;
                    if (travelMap.GetValueOrDefault(next, int.MaxValue) == distance - 1)
                    {
                        currentPosition = next;
                        yield return direction;
                        break;
                    }
                }
            }
        }

        static int FillGridFrom(Point2D startPoint)
        {
            Dictionary<Point2D, int> distanceMap = new Dictionary<Point2D, int>();
            Queue<Point2D> distancePoints = new Queue<Point2D>();
            distancePoints.Enqueue(startPoint);
            distanceMap.Add(startPoint, 0);

            while (distancePoints.Count > 0)
            {
                Point2D current = distancePoints.Dequeue();

                int distance = distanceMap[current] + 1;

                foreach (Point2D next in GetAdjacents(current))
                {
                    if (!grid.GetValueOrDefault(next))
                    {
                        continue;
                    }

                    if (distanceMap.GetValueOrDefault(next, int.MaxValue) > distance)
                    {
                        distanceMap[next] = distance;
                        if (!distancePoints.Contains(next))
                        {
                            distancePoints.Enqueue(next);
                        }
                    }
                }
            }

            return distanceMap.Values.Max();
        }

        static IEnumerable<Point2D> GetAdjacents(Point2D center)
        {
            yield return center + Point2D.XAxis;
            yield return center - Point2D.XAxis;
            yield return center - Point2D.YAxis;
            yield return center + Point2D.YAxis;
        }

        static int InvertDirection(int direction)
        {
            switch (direction)
            {
                case 1: return 2;
                case 2: return 1;
                case 3: return 4;
                case 4: return 3;
                default: throw new Exception();
            }
        }
    }
}
