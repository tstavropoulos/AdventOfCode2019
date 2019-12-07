using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoCTools.IntCode;

namespace Day05
{
    class Program
    {
        private const string inputFile = @"../../../../input05.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 5 - Sunny with a Chance of Asteroids");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            int[] values = line.Split(',').Select(int.Parse).ToArray();

            int output1 = 0;

            IntCode machine = new IntCode(values,
                output: x => output1 = x,
                input: () => 1);

            while (machine.Execute() != IntCode.State.Terminate) { }

            Console.WriteLine($"The answer is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            int output2 = 0;

            IntCode machine2 = new IntCode(values,
                output: x => output2 = x,
                input: () => 5);

            while (machine2.Execute() != IntCode.State.Terminate) { }


            Console.WriteLine($"The answer is: {output2}");

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
