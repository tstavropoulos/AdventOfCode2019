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
            Console.WriteLine("Day 9");
            Console.WriteLine("Star 1");
            Console.WriteLine();


            string line = File.ReadAllText(inputFile);
            //string line = "104,1125899906842624,99";

            long[] regs = line.Split(",").Select(long.Parse).ToArray();

            IntCode machine = new IntCode("Star 1", regs, new long[] { 1 }, output: Console.WriteLine);

            machine.Run().Wait();

            Console.WriteLine($"The answer is: {machine.lastOutput}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            int output2 = 0;
            

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            IntCode machine2 = new IntCode("Star 2", regs, new long[] { 2 }, output: Console.WriteLine);
            machine2.Run().Wait();
            sw.Stop();

            Console.WriteLine($"Took: {sw.Elapsed.TotalMilliseconds} ms");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
