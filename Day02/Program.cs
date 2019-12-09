using System;
using System.IO;
using System.Linq;
using AoCTools.IntCode;

namespace Day02
{
    class Program
    {
        private const string inputFile = @"../../../../input02.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 2 - 1202 Program Alarm");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] initialRegs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            IntCode intCode = new IntCode(
                name: "Star 1 Machine",
                regs: initialRegs);

            intCode[1] = 12;
            intCode[2] = 2;

            intCode.Run().Wait();

            Console.WriteLine($"The answer is: {intCode[0]}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int matchingInput = FindMatch(initialRegs);
            Console.WriteLine($"The answer is: {matchingInput}");

            Console.WriteLine();
            Console.ReadKey();
        }

        private static int FindMatch(long[] initialRegs)
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    IntCode intCode = new IntCode(
                        name: $"Star 2 Machine ({noun},{verb})",
                        regs: initialRegs);

                    intCode[1] = noun;
                    intCode[2] = verb;

                    intCode.Run().Wait();

                    if (intCode[0] == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            throw new Exception("Result not found");
        }
    }
}
