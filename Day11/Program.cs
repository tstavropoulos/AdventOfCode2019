using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;
using AoCTools.IntCode;

namespace Day11
{
    class Program
    {
        private const string inputFile = @"../../../../input11.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 11 - Space Police");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string line = File.ReadAllText(inputFile);

            long[] regs = line.Split(",").Select(long.Parse).ToArray();
            {
                HullPaintingRobot robot = new HullPaintingRobot(1);

                IntCode machine = new IntCode(
                    name: "Star 1",
                    regs: regs,
                    fixedInputs: new long[0],
                    input: robot.GetLocationColor,
                    output: robot.HandleInput);



                machine.Run().Wait();


                Console.WriteLine($"The answer is: {robot.paintedLocations.Count}");
            }

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            {
                HullPaintingRobot robot = new HullPaintingRobot(2);

                IntCode machine = new IntCode(
                    name: "Star 2",
                    regs: regs,
                    fixedInputs: new long[0],
                    input: robot.GetLocationColor,
                    output: robot.HandleInput);

                machine.Run().Wait();

                HashSet<Point2D> finalLocations = new HashSet<Point2D>(
                    robot.paintedLocations.Where(x => x.Value == 1).Select(x => x.Key));

                int xMin = finalLocations.Select(x => x.x).Min();
                int yMin = finalLocations.Select(x => x.y).Min();
                int xMax = finalLocations.Select(x => x.x).Max();
                int yMax = finalLocations.Select(x => x.y).Max();

                Console.BackgroundColor = ConsoleColor.Black;
                for (int y = 0; y <= yMax; y++)
                {
                    for (int x = 0; x <= xMax; x++)
                    {
                        if (finalLocations.Contains((x, y)))
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
                }

            }

            Console.WriteLine();
            Console.ReadKey();
        }

        public class HullPaintingRobot
        {
            bool paintMode = true;

            public Dictionary<Point2D, long> paintedLocations = new Dictionary<Point2D, long>();

            public Point2D location = (0, 0);
            public Point2D heading = (0, -1);


            public HullPaintingRobot(int puzzle)
            {
                if (puzzle == 2)
                {
                    paintedLocations[location] = 1L;
                }
            }

            public void HandleInput(long value)
            {
                if (paintMode)
                {
                    //Paint Location {value}
                    paintedLocations[location] = value;
                }
                else
                {
                    //Rotate
                    if (value == 1)
                    {
                        heading = new Point2D(-heading.y, heading.x);

                    }
                    else if (value == 0)
                    {
                        heading = new Point2D(heading.y, -heading.x);
                    }
                    else
                    {
                        throw new Exception();
                    }

                    location += heading;
                }

                paintMode = !paintMode;
            }


            public long GetLocationColor() => paintedLocations.GetValueOrDefault(location, 0L);
        }
    }
}
