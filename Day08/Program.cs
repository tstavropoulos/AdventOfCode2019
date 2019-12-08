using System;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        private const string inputFile = @"../../../../input08.txt";

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Day 8 - Space Image Format");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            //25 x 6
            int layerSize = 25 * 6;
            int layerCount = line.Length / layerSize;
            string[] lines = new string[layerCount];

            for (int i = 0; i < layerCount; i++)
            {
                lines[i] = line.Substring(layerSize * i, layerSize);
            }

            int zeros = int.MaxValue;
            int ones = 0;
            int twos = 0;

            for (int i = 0; i < layerCount; i++)
            {
                int temp_zeros = 0;

                temp_zeros = lines[i].Count(x => x == '0');

                if (temp_zeros < zeros)
                {
                    zeros = temp_zeros;
                    ones = lines[i].Count(x => x == '1');
                    twos = lines[i].Count(x => x == '2');
                }
            }

            Console.WriteLine($"The answer is: {ones * twos}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            char[] finalImage = new char[layerSize];

            for (int pixel = 0; pixel < layerSize; pixel++)
            {
                for (int layer = 0; layer < layerCount; layer++)
                {
                    if (lines[layer][pixel] == '2')
                    {
                        continue;
                    }
                    else
                    {
                        finalImage[pixel] = lines[layer][pixel];
                        break;
                    }
                }
            }

            for (int i = 0; i < layerSize; i++)
            {
                if (i % 25 == 0)
                {
                    Console.WriteLine();
                }

                if (finalImage[i] == '1')
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write(' ');

            }

            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
