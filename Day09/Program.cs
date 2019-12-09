using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools.IntCode;

namespace Day09
{
    class Program
    {
        private const string inputFile = @"../../../../input09.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 9 - Sensor Boost");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            long[] regs = line.Split(",").Select(long.Parse).ToArray();

            IntCode machine = new IntCode(
                name: "Star 1",
                regs: regs,
                fixedInputs: new long[] { 1 },
                output: x => Console.WriteLine($"Star 1 output: {x}"));

            machine.Run().Wait();

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            IntCode machine2 = new IntCode(
                name: "Star 2",
                regs: regs,
                fixedInputs: new long[] { 2 },
                output: x => Console.WriteLine($"Star 2 output: {x}"));

            machine2.Run().Wait();

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
