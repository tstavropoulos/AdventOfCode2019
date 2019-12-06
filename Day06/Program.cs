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
            Console.WriteLine("Day 6");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            foreach(string line in lines)
            {
                Planet parent = GetPlanet(line.Substring(0, 3));
                Planet child = GetPlanet(line.Substring(4, 3));

                parent.AddChild(child);
                child.SetParent(parent);
            }



            int output1 = planets.Values.Select(x=>x.Tier).Sum();



            Console.WriteLine($"The answer is: {output1}");

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

                foreach(Planet next in current.Connections())
                {
                    if (next.Distance > newDistance)
                    {
                        next.Distance = newDistance;
                        pendingEvals.Enqueue(next);
                    }
                }
            }



            Console.WriteLine($"The answer is: {end.Distance}");


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

    public class Planet
    {
        public string Name { get; }

        public Planet parent = null;
        public List<Planet> children = new List<Planet>();

        public int Distance { get; set; } = int.MaxValue;

        public int Tier
        {
            get
            {
                if (parent == null)
                {
                    return 0;
                }

                return parent.Tier + 1;
            }
        }

        public Planet(string name)
        {
            Name = name;
        }

        public void SetParent(Planet parent)
        {
            if (this.parent != null)
            {
                throw new Exception();
            }

            this.parent = parent;
        }

        public void AddChild(Planet child)
        {
            children.Add(child);
        }

        public IEnumerable<Planet> Connections()
        {
            if (parent != null)
            {
                yield return parent;
            }

            foreach (Planet child in children)
            {
                yield return child;
            }
        }
    }
}
