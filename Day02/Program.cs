using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        private const string inputFile = @"../../../../input02.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 2");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            {
                int[] values = File.ReadAllText(inputFile).Split(',').Select(int.Parse).ToArray();

                int currentIndex = 0;
                bool cont = true;

                values[1] = 12;
                values[2] = 2;
                while (cont)
                {
                    switch (values[currentIndex])
                    {

                        case 1:
                            //Add
                            values[values[currentIndex + 3]] = values[values[currentIndex + 1]] + values[values[currentIndex + 2]];
                            currentIndex += 4;
                            break;

                        case 2:
                            //Multiply
                            values[values[currentIndex + 3]] = values[values[currentIndex + 1]] * values[values[currentIndex + 2]];
                            currentIndex += 4;
                            break;

                        case 99:
                            cont = false;
                            break;

                        default: throw new Exception();
                    }
                }

                Console.WriteLine($"The answer is: {values[0]}");
            }

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            int finalNoun = 0;
            int finalVerb = 0;
            bool done = false;
            {
                int[] initialValues = File.ReadAllText(inputFile).Split(',').Select(int.Parse).ToArray();

                for (int noun = 0; noun < 100; noun++)
                {
                    for (int verb = 0; verb < 100; verb++)
                    {

                        int[] values = (int[])initialValues.Clone();

                        values[1] = noun;
                        values[2] = verb;

                        int currentIndex = 0;
                        bool cont = true;

                        while (cont)
                        {
                            switch (values[currentIndex])
                            {
                                case 1:
                                    //Add
                                    values[values[currentIndex + 3]] = values[values[currentIndex + 1]] + values[values[currentIndex + 2]];
                                    currentIndex += 4;
                                    break;

                                case 2:
                                    //Multiply
                                    values[values[currentIndex + 3]] = values[values[currentIndex + 1]] * values[values[currentIndex + 2]];
                                    currentIndex += 4;
                                    break;

                                case 99:
                                    cont = false;
                                    break;

                                default: throw new Exception();
                            }
                        }

                        if (values[0] == 19690720)
                        {
                            done = true;
                            finalNoun = noun;
                            finalVerb = verb;
                        }

                        if(done)
                        {
                            break;
                        }

                    }

                    if (done)
                    {
                        break;
                    }
                }
            }


            Console.WriteLine($"The answer is: {100 * finalNoun + finalVerb}");


            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
