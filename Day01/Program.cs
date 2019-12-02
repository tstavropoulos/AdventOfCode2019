using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        private const string inputFile = @"../../../../input01.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 1");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            int[] masses = lines.Select(int.Parse).ToArray();

            int fuel = masses.Select(GetFuel).Sum();

            Console.WriteLine($"The answer is: {fuel}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int totalFuel = masses
                .Select(GetFinalFuel)
                .Sum();

            Console.WriteLine($"The answer is: {totalFuel}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static int GetFuel(int mass)
        {
            return (mass / 3) - 2;
        }

        static int GetFinalFuel(int mass)
        {
            int fuel = GetFuel(mass);
            int totalFuel = 0;
            while (fuel > 0)
            {
                totalFuel += fuel;
                fuel = GetFuel(fuel);
            }

            return totalFuel;
        }
    }
}
