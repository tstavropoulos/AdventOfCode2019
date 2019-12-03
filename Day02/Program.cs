using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools.IntCode;
using State = AoCTools.IntCode.IntCode.State;

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

            int[] initialRegs = File.ReadAllText(inputFile).Split(',').Select(int.Parse).ToArray();
            IntCode initialIntCode = new IntCode(initialRegs);

            IntCode intCode = initialIntCode.Clone();

            intCode[1] = 12;
            intCode[2] = 2;

            while (intCode.Execute() == State.Continue) ;

            Console.WriteLine($"The answer is: {intCode[0]}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int matchingInput = FindMatch(initialIntCode);
            Console.WriteLine($"The answer is: {matchingInput}");

            Console.WriteLine();
            Console.ReadKey();
        }

        private static int FindMatch(IntCode initialIntCode)
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    IntCode intCode = initialIntCode.Clone();

                    intCode[1] = noun;
                    intCode[2] = verb;

                    while (intCode.Execute() == State.Continue) ;

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
