using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        private const string inputFile = @"../../../../input02.txt";

        private enum State
        {
            Continue = 0,
            Terminate
        }

        private enum Instr
        {
            Add = 1,
            Multiply = 2,
            Terminate = 99
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Day 2");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            int[] initialRegs = File.ReadAllText(inputFile).Split(',').Select(int.Parse).ToArray();
            int[] regs = (int[])initialRegs.Clone();

            int instr = 0;

            regs[1] = 12;
            regs[2] = 2;

            while (Execute(ref instr, regs) == State.Continue) ;

            Console.WriteLine($"The answer is: {regs[0]}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int matchingInput = FindMatch(initialRegs);

            Console.WriteLine($"The answer is: {matchingInput}");

            Console.WriteLine();
            Console.ReadKey();
        }


        private static State Execute(ref int instr, int[] regs)
        {
            switch ((Instr)regs[instr])
            {
                case Instr.Add:
                    regs[regs[instr + 3]] = regs[regs[instr + 1]] + regs[regs[instr + 2]];
                    instr += 4;
                    return State.Continue;

                case Instr.Multiply:
                    regs[regs[instr + 3]] = regs[regs[instr + 1]] * regs[regs[instr + 2]];
                    instr += 4;
                    return State.Continue;

                case Instr.Terminate:
                    return State.Terminate;

                default: throw new Exception($"Unsupported instruction: {regs[instr]}");
            }
        }

        private static int FindMatch(int[] initialRegs)
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    int[] regs = (int[])initialRegs.Clone();

                    regs[1] = noun;
                    regs[2] = verb;

                    int instr = 0;

                    while (Execute(ref instr, regs) == State.Continue) ;

                    if (regs[0] == 19690720)
                    {
                        return 100 * noun + verb;
                    }
                }
            }

            throw new Exception("Result not found");
        }
    }
}
