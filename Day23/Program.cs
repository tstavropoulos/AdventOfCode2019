using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;
using AoCTools.IntCode;

namespace Day23
{
    class Program
    {
        private const string inputFile = @"../../../../input23.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 23");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            IntCode machine = new IntCode(
                name: "Star 1",
                regs: regs,
                fixedInputs: Array.Empty<long>(),
                output: x => Console.Write(x));

            machine.SyncRun();

            int output1 = 0;



            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            int output2 = 0;



            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
