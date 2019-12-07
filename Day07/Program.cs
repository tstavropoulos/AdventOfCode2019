using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoCTools.IntCode;

namespace Day07
{
    class Program
    {
        private const string inputFile = @"../../../../input07.txt";
        static bool inputToggle = false;
        static int lastOutput = 0;
        static int phaseValue = 0;
        static IntCode[] amplifiers = new IntCode[5];
        static int[] inputRequestCount = new int[5];

        static void Main(string[] args)
        {
            Console.WriteLine("Day 7 - Amplification Circuit");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            int[] values = line.Split(',').Select(int.Parse).ToArray();

            int bestOutput = int.MinValue;

            foreach(int[] ordering in GetOrderings(new int[5], 0, new bool[5], 0))
            {
                lastOutput = 0;
                for (int i = 0; i < 5; i++)
                {
                    inputToggle = false;
                    phaseValue = ordering[i];
                    IntCode amplifier = new IntCode(
                        values,
                        output: x => lastOutput = x,
                        input: GetInput);

                    while (amplifier.Execute() != IntCode.State.Terminate) { }
                }

                bestOutput = Math.Max(bestOutput, lastOutput);
            }

            Console.WriteLine($"The answer is: {bestOutput}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            int bestOutput2 = int.MinValue;

            foreach (int[] ordering in GetOrderings(new int[5], 0, new bool[5], 5))
            {
                Array.Clear(inputRequestCount, 0, 5);
                for (int i = 0; i < 5; i++)
                {
                    int machineIndex = i;
                    int priorMachine = (i + 5 - 1) % 5;
                    inputToggle = false;
                    amplifiers[i] = new IntCode(
                        values,
                        input: () =>
                        {
                            int request = inputRequestCount[machineIndex]++;

                            switch (request)
                            {
                                case 0:
                                    return ordering[machineIndex];

                                case 1:
                                    if (machineIndex == 0)
                                    {
                                        return 0;
                                    }
                                    goto default;

                                default:
                                    return amplifiers[priorMachine].lastOutput;
                            }
                        });

                }

                while (!amplifiers[4].done)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        while (amplifiers[i].Execute() == IntCode.State.Continue) { }
                    }
                }

                bestOutput2 = Math.Max(bestOutput2, amplifiers[4].lastOutput);
            }


            Console.WriteLine($"The answer is: {bestOutput2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        static int GetInput()
        {
            if (!inputToggle)
            {
                inputToggle = true;
                return phaseValue;
            }

            return lastOutput;
        }

        static IEnumerable<int[]> GetOrderings(int[] order, int index, bool[] used, int offset)
        {
            for (int i = 0; i < 5; i++)
            {
                if (!used[i])
                {
                    order[index] = offset + i;
                    used[i] = true;

                    if (index == 4)
                    {
                        yield return order;
                    }
                    else
                    {
                        foreach (int[] ordering in GetOrderings(order, index + 1, used, offset))
                        {
                            yield return ordering;
                        }
                    }

                    used[i] = false;
                }
            }
        }
    }
}
