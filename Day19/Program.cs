using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools.IntCode;

namespace Day19
{
    class Program
    {
        private const string inputFile = @"../../../../input19.txt";

        static IEnumerable<long> regs;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 19 - Tractor Beam");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            long totalAffected = 0;


            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    IntCode machine = new IntCode(
                        "Star 1",
                        regs,
                        fixedInputs: new long[] { x, y },
                        output: n => totalAffected += n);

                    machine.SyncRun();
                }
            }

            Console.WriteLine($"The answer is: {totalAffected}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            int x0 = 0;
            int y1 = 150;
            while (true)
            {
                //Find first x coordinate of beam.

                while (!TestCoordinate(x0, y1))
                {
                    x0++;
                }

                if (TestCoordinate(x0 + 99, y1 - 99))
                {
                    break;
                }

                y1++;
            }

            Console.WriteLine($"The answer is: {10000 * x0 + (y1 - 99)}");


            Console.WriteLine();
            Console.ReadKey();
        }

        static bool TestCoordinate(int x, int y)
        {
            IntCode machine = new IntCode(
                "Star 1",
                regs,
                fixedInputs: new long[] { x, y });

            machine.SyncRun();

            return machine.lastOutput == 1;
        }
    }
}
