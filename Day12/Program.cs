using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day12
{
    class Program
    {
        private const string inputFile = @"../../../../input12.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 12 - The N-Body Problem");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            Point3D[] initialpositions = File.ReadAllLines(inputFile).Select(Parse).ToArray();
            int moons = initialpositions.Length;

            Point3D[] initialvelocities = Enumerable.Repeat(Point3D.Zero, moons).ToArray();

            Point3D[] positions = (Point3D[])initialpositions.Clone();
            Point3D[] velocities = (Point3D[])initialvelocities.Clone();

            //Handle Gravity
            for (int step = 0; step < 1000; step++)
            {
                for (int i = 0; i < moons - 1; i++)
                {
                    for (int j = i + 1; j < moons; j++)
                    {
                        if (positions[i].x > positions[j].x)
                        {
                            velocities[i] -= Point3D.XAxis;
                            velocities[j] += Point3D.XAxis;
                        }
                        else if (positions[i].x < positions[j].x)
                        {
                            velocities[i] += Point3D.XAxis;
                            velocities[j] -= Point3D.XAxis;
                        }

                        if (positions[i].y > positions[j].y)
                        {
                            velocities[i] -= Point3D.YAxis;
                            velocities[j] += Point3D.YAxis;
                        }
                        else if (positions[i].y < positions[j].y)
                        {
                            velocities[i] += Point3D.YAxis;
                            velocities[j] -= Point3D.YAxis;
                        }

                        if (positions[i].z > positions[j].z)
                        {
                            velocities[i] -= Point3D.ZAxis;
                            velocities[j] += Point3D.ZAxis;
                        }
                        else if (positions[i].z < positions[j].z)
                        {
                            velocities[i] += Point3D.ZAxis;
                            velocities[j] -= Point3D.ZAxis;
                        }
                    }
                }

                for (int i = 0; i < moons; i++)
                {
                    positions[i] += velocities[i];
                }
            }

            int totalEnergy = 0;

            for (int i = 0; i < moons; i++)
            {
                totalEnergy += CalculateEnergy(positions[i], velocities[i]);
            }

            Console.WriteLine($"The answer is: {totalEnergy}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //Axes are completely independent.  Thus the solution is the LCM of the XPeriod, YPeriod, and ZPeriod

            long xPeriod = GetPeriod(initialpositions.Select(x => x.x).ToArray(), initialvelocities.Select(x => x.x).ToArray());
            long yPeriod = GetPeriod(initialpositions.Select(x => x.y).ToArray(), initialvelocities.Select(x => x.y).ToArray());
            long zPeriod = GetPeriod(initialpositions.Select(x => x.z).ToArray(), initialvelocities.Select(x => x.z).ToArray());

            long gcd = GetGCD(xPeriod, yPeriod);
            gcd = GetGCD(gcd, zPeriod);

            long lcmXY = (xPeriod / GetGCD(xPeriod, yPeriod)) * yPeriod;
            long total = (lcmXY / GetGCD(lcmXY, zPeriod)) * zPeriod;

            //Find the period of each dimension

            Console.WriteLine($"The answer is: {total}");


            Console.WriteLine();
            Console.ReadKey();
        }


        static char[] splitChars = new[] { '<', 'x', 'y', 'z', '=', '>', ' ', ',' };

        static Point3D Parse(string line)
        {
            string[] splitLine = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            return new Point3D(int.Parse(splitLine[0]), int.Parse(splitLine[1]), int.Parse(splitLine[2]));
        }

        static int CalculateEnergy(in Point3D position, in Point3D velocity) => CalculateAbsSum(position) * CalculateAbsSum(velocity);

        static int CalculateAbsSum(in Point3D vector) => Math.Abs(vector.x) + Math.Abs(vector.y) + Math.Abs(vector.z);

        static long GetPeriod(int[] positions, int[] velocities)
        {
            long stepCount = 0;

            int[] initialPositions = (int[])positions.Clone();
            int[] initialVelocities = (int[])velocities.Clone();

            bool continueSearch = true;

            while (continueSearch)
            {
                for (int i = 0; i < positions.Length - 1; i++)
                {
                    for (int j = i + 1; j < positions.Length; j++)
                    {
                        if (positions[i] > positions[j])
                        {
                            velocities[i] -= 1;
                            velocities[j] += 1;
                        }
                        else if (positions[i] < positions[j])
                        {
                            velocities[i] += 1;
                            velocities[j] -= 1;
                        }
                    }

                }

                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] += velocities[i];
                }

                stepCount++;

                continueSearch = false;
                for (int i = 0; i < positions.Length; i++)
                {
                    if (initialPositions[i] != positions[i])
                    {
                        continueSearch = true;
                        break;
                    }

                    if (initialVelocities[i] != velocities[i])
                    {
                        continueSearch = true;
                        break;
                    }
                }
            }

            return stepCount;
        }

        static long GetGCD(long a, long b)
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
