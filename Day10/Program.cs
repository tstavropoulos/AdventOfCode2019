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
        static bool[,] grid;
        static List<Point2D> asteroids;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 10");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);


            asteroids = new List<Point2D>();
            Dictionary<Point2D, int> hitCount = new Dictionary<Point2D, int>();
            //Get all prime factors of DX and DY

            grid = new bool[lines[0].Length, lines.Length];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        grid[x, y] = true;
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
                    .Select(x => (x - lastVaporized, x))
                    .OrderBy(x => x.Item1.Length)
                    .Where(x => x.Item1.Length > 0.001)
                    .Where(x => x.Item1.Cross(lastVaporized).z < 0.0).First().Item2;

                lastFull = map.Where(x => x.Value == next).OrderBy(x => (basePoint - x.Key).Length).First().Key;

                lastVaporized = map[lastFull];
                map.Remove(lastFull);
                vaporizedCount++;
            }

            Console.WriteLine($"The answer is: {lastFull.x * 100 + lastFull.y}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static bool CanSee(Point2D start, Point2D end)
        {
            Point2D slope = end - start;

            int deltaY = slope.y;
            int deltaX = slope.x;


            List<int> xFactors = new List<int>(Factorize(Math.Abs(deltaX)));
            List<int> yFactors = new List<int>(Factorize(Math.Abs(deltaY)));

            for (int i = 0; i < xFactors.Count; i++)
            {
                if (yFactors.Contains(xFactors[i]))
                {
                    yFactors.Remove(xFactors[i]);
                    xFactors.RemoveAt(i);
                    i--;
                }
            }

            int reducedX = Math.Sign(deltaX) * xFactors.Aggregate(1, (x, y) => x * y);
            int reducedY = Math.Sign(deltaY) * yFactors.Aggregate(1, (x, y) => x * y);

            Point2D reducedSlope = new Point2D(reducedX, reducedY);

            if (deltaY == 0)
            {
                reducedSlope = new Point2D(Math.Sign(deltaX), 0);
            }
            else if (deltaX == 0)
            {
                reducedSlope = new Point2D(0, Math.Sign(deltaY));
            }

            Point2D point = start + reducedSlope;

            while (point != end)
            {
                if (asteroids.Contains(point))
                {
                    if (start == (5, 8))
                    {
                        Console.WriteLine($"Missed {end} because of {point}");
                    }

                    return false;
                }

                point += reducedSlope;
            }

            if (start == (5, 8))
            {
                Console.WriteLine($"Hit {end}");
            }

            return true;
        }


        public static IEnumerable<int> Factorize(int number)
        {
            if (number < 1)
            {
                yield break;
            }

            //Strip out factors of 2.
            //Many numbers requested will have many (or only) factors of 2.
            //This is an optimization
            while (number % 2 == 0)
            {
                yield return 2;
                number /= 2;
            }

            foreach (int prime in PrimesUpTo((int)Math.Sqrt(number)))
            {
                while (number % prime == 0)
                {
                    yield return prime;
                    number /= prime;
                }

                if (number == 1)
                {
                    break;
                }
            }

            if (number > 1)
            {
                yield return number;
            }
        }

        public static IEnumerable<int> PrimesUpTo(int number)
        {
            if (number < 2)
            {
                yield break;
            }

            //Include the boundary
            number++;

            BitArray primeField = new BitArray(number, true);
            primeField.Set(0, false);
            primeField.Set(1, false);
            yield return 2;

            //We don't bother setting the multiples of 2 because we don't bother checking them.

            int i;
            for (i = 3; i * i < number; i += 2)
            {
                if (primeField.Get(i))
                {
                    //i Is Prime
                    yield return i;

                    //Clear new odd factors
                    //All our primes are now odd, as are our primes Squared.
                    //This maens the numbers we need to clear start at i*i, and advance by 2*i
                    //For example j=3:  9 is the first odd composite, 15 is the next odd composite 
                    //  that's a factor of 3
                    for (int j = i * i; j < number; j += 2 * i)
                    {
                        primeField.Set(j, false);
                    }
                }
            }

            //Grab remainder of identified primes
            for (; i < number; i += 2)
            {
                if (primeField.Get(i))
                {
                    yield return i;
                }
            }
        }
    }
}
