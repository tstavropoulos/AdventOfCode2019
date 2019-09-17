using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        private const string inputFile = @"../../../../input25.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 25");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);



            int output = 0;



            Console.WriteLine($"The answer is: {output}");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
