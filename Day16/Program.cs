using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        private const string inputFile = @"../../../../input16.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 16 - Flawed Frequency Transmission");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string input = File.ReadAllText(inputFile);

            IEnumerable<int> values = input.Select(x => (int)char.GetNumericValue(x));
            IEnumerable<int> next = values;

            for (int phase = 0; phase < 100; phase++)
            {
                next = FFT(next).ToArray();
            }

            Console.WriteLine($"The answer is: {string.Join("", next.Take(8).Select(x => x.ToString()))}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int offset = int.Parse(string.Join("", values.Take(7).Select(x => x.ToString())));

            int remainingValues = values.Count() * 10000 - offset;

            int totalSums = 1 + (remainingValues - 1) / values.Count();

            IEnumerable<int> longValues = Repeat(values, totalSums).Skip(offset % values.Count());
            int[] nextLong = longValues.ToArray();

            for (int phase = 0; phase < 100; phase++)
            {
                FFT2(nextLong);
            }

            Console.WriteLine($"The answer is: {string.Join("", nextLong.Take(8).Select(x => x.ToString()))}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static IEnumerable<int> Repeat(IEnumerable<int> input, int count)
        {
            for (int i = 0; i < count; i++)
            {
                foreach (int value in input)
                {
                    yield return value;
                }
            }
        }

        static IEnumerable<int> FFT(IEnumerable<int> input)
        {
            for (int i = 0; i < input.Count(); i++)
            {
                int cumulative = input.Zip(GetPattern(i).Skip(1), (x, y) => x * y).Sum();

                yield return Math.Abs(cumulative) % 10;
            }
        }

        static void FFT2(int[] input)
        {
            for (int i = input.Length - 2; i >= 0; i--)
            {
                input[i] = (input[i] + input[i + 1]) % 10;
            }
        }

        static IEnumerable<int> GetPattern(int depth)
        {
            while (true)
            {
                foreach (int i in Enumerable.Repeat(0, depth + 1))
                {
                    yield return i;
                }

                foreach (int i in Enumerable.Repeat(1, depth + 1))
                {
                    yield return i;
                }

                foreach (int i in Enumerable.Repeat(0, depth + 1))
                {
                    yield return i;
                }

                foreach (int i in Enumerable.Repeat(-1, depth + 1))
                {
                    yield return i;
                }
            }

        }
    }
}
