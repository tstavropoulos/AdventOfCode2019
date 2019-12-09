using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AoCTools.IntCode;

namespace Day07
{
    class Program
    {
        private const string inputFile = @"../../../../input07.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 7 - Amplification Circuit");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            long[] values = line.Split(',').Select(long.Parse).ToArray();
            long bestOutput = long.MinValue;
            IntCode[] amplifiers = new IntCode[5];

            foreach (int[] ordering in GetOrderings(new int[5], 0, new bool[5], 0))
            {
                for (int i = 0; i < 5; i++)
                {
                    long[] input;

                    if (i == 0)
                    {
                        input = new long[] { ordering[i], 0 };
                    }
                    else
                    {
                        input = new long[] { ordering[i] };
                    }

                    int nextMachine = (i + 1) % 5;
                    amplifiers[i] = new IntCode(
                        name: $"Machine {i}",
                        regs: values,
                        fixedInputs: input,
                        output: x => amplifiers[nextMachine].WriteValue(x));
                }

                amplifiers.Select(x => x.Run()).ToArray()[4].Wait();

                bestOutput = Math.Max(bestOutput, amplifiers[4].lastOutput);
            }

            Console.WriteLine($"The answer is: {bestOutput}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            long bestOutput2 = int.MinValue;

            foreach (int[] ordering in GetOrderings(new int[5], 0, new bool[5], 5))
            {
                for (int i = 0; i < 5; i++)
                {
                    int nextMachine = (i + 1) % 5;
                    long[] input;

                    if (i == 0)
                    {
                        input = new long[] { ordering[i], 0 };
                    }
                    else
                    {
                        input = new long[] { ordering[i] };
                    }

                    amplifiers[i] = new IntCode(
                        name: $"Machine {i}",
                        regs: values,
                        fixedInputs: input,
                        output: x => amplifiers[nextMachine].WriteValue(x));

                }

                amplifiers.Select(x => x.Run()).ToArray()[4].Wait();

                bestOutput2 = Math.Max(bestOutput2, amplifiers[4].lastOutput);
            }

            Console.WriteLine($"The answer is: {bestOutput2}");

            Console.WriteLine();
            Console.ReadKey();
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
