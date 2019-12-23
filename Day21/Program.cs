using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;
using AoCTools.IntCode;

namespace Day21
{
    class Program
    {
        private const string inputFile = @"../../../../input21.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 21 - Springdroid Adventure");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            //First test program:

            // IF (!A or !B) && C -> Jump

            //if !(A and B) && C -> Jump


            string input =
                "OR A T\n" +
                "AND B T\n" +
                "AND C T\n" +
                "NOT T J\n" +
                "AND D J\n" +
                "WALK\n";

            long[] inputArray = input.Select(x => (long)x).ToArray();


            long output1 = 0;

            IntCode machine = new IntCode(
                "Star 1",
                regs,
                fixedInputs: inputArray,
                output: x => output1 = x);


            machine.SyncRun();


            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            string input2 =
                "OR A T\n" +
                "AND B T\n" +
                "AND C T\n" +
                "NOT T T\n" +
                "AND D T\n" +
                "OR E J\n" +
                "OR H J\n" +
                "AND T J\n" +
                "RUN\n";

            long[] input2Array = input2.Select(x => (long)x).ToArray();

            long output2 = 0;

            IntCode machine2 = new IntCode(
                "Star 2",
                regs,
                fixedInputs: input2Array,
                output: x => output2 = x);


            machine2.SyncRun();



            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
