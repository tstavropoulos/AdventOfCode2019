using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        private const string inputFile = @"../../../../input06.txt";

        private static Dictionary<string, Planet> planets = new Dictionary<string, Planet>();

        static void Main(string[] args)
        {
            Console.WriteLine("Day 6 - Universal Orbit Map");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            foreach (string line in lines)
            {
                Planet parent = GetPlanet(line.Substring(0, 3));
                Planet child = GetPlanet(line.Substring(4, 3));

                parent.AddChild(child);
                child.SetParent(parent);
            }

            int output1 = planets.Values.Select(x => x.Tier).Sum();

            Console.WriteLine($"The cumulative depth is: {output1}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();


            Planet start = planets["YOU"].parent;
            start.Distance = 0;

            Planet end = planets["SAN"].parent;


            Queue<Planet> pendingEvals = new Queue<Planet>();

            pendingEvals.Enqueue(start);

            while (pendingEvals.Count > 0)
            {
                Planet current = pendingEvals.Dequeue();

                if (current == end)
                {
                    break;
                }

                int newDistance = current.Distance + 1;

                foreach (Planet next in current.Connections())
                {
                    if (next.Distance > newDistance)
                    {
                        next.Distance = newDistance;
                        pendingEvals.Enqueue(next);
                    }
                }
            }

            Console.WriteLine($"The distance from you to santa is: {end.Distance}");

            Console.WriteLine();
            Console.ReadKey();
        }

        public static Planet GetPlanet(string name)
        {
            if (!planets.ContainsKey(name))
            {
                planets.Add(name, new Planet(name));
            }

            return planets[name];
        }
    }
}
