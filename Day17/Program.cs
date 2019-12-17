using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AoCTools;
using AoCTools.IntCode;

namespace Day17
{
    class Program
    {
        private const string inputFile = @"../../../../input17.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 17");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            StringBuilder builder = new StringBuilder();

            IntCode machine = new IntCode(
                "Star 1",
                regs,
                Array.Empty<long>(),
                output: x => builder.Append((char)x));



            machine.SyncRun();
            string text = builder.ToString();

            string[] lines = text.Split('\n').Where(x=>x.Length != 0).ToArray();

            int alignmentParam = 0;

            for (int y = 1; y < lines.Length - 1; y++)
            {
                for (int x = 1; x < lines[0].Length - 1; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        if (lines[y-1][x] == '#' && lines[y+1][x] == '#' && lines[y][x+1] == '#' && lines[y][x-1] == '#')
                        {
                            alignmentParam += x * y;
                        }
                    }
                }
            }

            Console.WriteLine($"The answer is: {alignmentParam}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();




            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
