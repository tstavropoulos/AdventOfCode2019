using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    class Program
    {
        private const string inputFile = @"../../../../input14.txt";

        static List<string> reagents = new List<string>();
        static Dictionary<string, Reaction> production = new Dictionary<string, Reaction>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 14 - Space Stoichiometry");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            Reaction[] reactions = File.ReadAllLines(inputFile).Select(x => new Reaction(x)).ToArray();

            foreach (Reaction reaction in reactions)
            {
                production.Add(reaction.output.reagent, reaction);
                reagents.Add(reaction.output.reagent);
            }

            long singleFuelReq = GetOreRequirementForFuel(1);

            Console.WriteLine($"The answer is: {singleFuelReq}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            long fuelLB = 1_000_000_000_000 / singleFuelReq;
            long fuelUB = 100 * fuelLB;

            Console.WriteLine($"The answer is: {FindTrillionOreFuel(fuelLB, fuelUB)}");


            Console.WriteLine();
            Console.ReadKey();
        }

        static long FindTrillionOreFuel(long lowerBound, long upperBound)
        {
            while (upperBound - lowerBound > 1)
            {
                long nextGuess = (lowerBound + upperBound) / 2;
                long nextResult = GetOreRequirementForFuel(nextGuess);
                if (nextResult > 1_000_000_000_000)
                {
                    upperBound = nextGuess;
                }
                else
                {
                    lowerBound = nextGuess;
                }
            }

            return lowerBound;
        }

        static long GetOreRequirementForFuel(long fuel)
        {
            Dictionary<string, long> req = new Dictionary<string, long>();
            Dictionary<string, long> prod = new Dictionary<string, long>();

            foreach (string reagent in reagents)
            {
                req.Add(reagent, 0);
                prod.Add(reagent, 0);
            }

            req["ORE"] = 0;
            req["FUEL"] = fuel;

            bool modified = true;

            while (modified)
            {
                modified = false;

                foreach (string reagent in reagents)
                {
                    if (reagent == "ORE")
                    {
                        continue;
                    }

                    if (req[reagent] > prod[reagent])
                    {
                        modified = true;

                        long diff = req[reagent] - prod[reagent];
                        Reaction reaction = production[reagent];
                        long count = 1 + (diff - 1) / reaction.output.count;
                        prod[reagent] += count * reaction.output.count;

                        foreach ((int inputCount, string inputReagent) in reaction.input)
                        {
                            req[inputReagent] += inputCount * count;
                        }
                    }
                }
            }

            return req["ORE"];
        }

        class Reaction
        {
            public IEnumerable<(int count, string reagent)> input;
            public (int count, string reagent) output;

            public Reaction(string line)
            {
                input = line.Split(" => ")[0].Split(", ").Select(Parse);
                output = Parse(line.Split(" => ")[1]);
            }
        }

        static (int count, string reagent) Parse(string fragment)
        {
            string[] split = fragment.Split(' ');

            return (int.Parse(split[0]), split[1]);
        }
    }
}
