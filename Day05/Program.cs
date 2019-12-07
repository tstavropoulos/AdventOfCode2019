using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoCTools.IntCode;

namespace Day05
{
    class Program
    {
        private const string inputFile = @"../../../../input05.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 5 - Sunny with a Chance of Asteroids");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            int[] values = line.Split(',').Select(int.Parse).ToArray();

            IntCode machine = new IntCode(
                name: $"Star 1 Machine",
                regs: values,
                fixedInputs: new[] { 1 });

            machine.Run().Wait();

            Console.WriteLine($"The answer is: {machine.lastOutput}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            IntCode machine2 = new IntCode(
                name: $"Star 2 Machine",
                regs: values,
                fixedInputs: new[] { 5 });

            machine2.Run().Wait();

            Console.WriteLine($"The answer is: {machine2.lastOutput}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
