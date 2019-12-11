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

            HullPaintingRobot firstRobot = new HullPaintingRobot(1);

            IntCode firstMachine = new IntCode(
                name: "Star 1",
                regs: regs,
                fixedInputs: Array.Empty<long>(),
                input: firstRobot.GetLocationColor,
                output: firstRobot.HandleInput);

            firstMachine.SyncRun();

            Console.WriteLine($"The answer is: {firstRobot.paintedLocations.Count}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            HullPaintingRobot secondRobot = new HullPaintingRobot(2);

            IntCode secondMachine = new IntCode(
                name: "Star 2",
                regs: regs,
                fixedInputs: Array.Empty<long>(),
                input: secondRobot.GetLocationColor,
                output: secondRobot.HandleInput);

            secondMachine.SyncRun();

            HashSet<Point2D> finalLocations = new HashSet<Point2D>(
                secondRobot.paintedLocations.Where(x => x.Value == 1).Select(x => x.Key));

            Point2D min = finalLocations.MinCoordinate();
            Point2D max = finalLocations.MaxCoordinate() + (1, 1);

            Console.BackgroundColor = ConsoleColor.Black;
            for (int y = min.y; y < max.y; y++)
            {
                for (int x = min.x; x < max.x; x++)
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

            Console.WriteLine();
            Console.ReadKey();
        }
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
                //Paint
                paintedLocations[location] = value;
            }
            else
            {
                //Rotate
                //Left/Right reversed because of the coordinate system
                heading = heading.Rotate(value == 0);
                location += heading;
            }

            paintMode = !paintMode;
        }

        public long GetLocationColor() => paintedLocations.GetValueOrDefault(location, 0L);
    }
}
