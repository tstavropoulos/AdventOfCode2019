using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;

namespace Day18
{
    class Program
    {
        private const string inputFile = @"../../../../input18.txt";

        static Square[,] grid;
        static Dictionary<Point2D, int> movementGrid;
        static Dictionary<Point2D, char> keyDict = new Dictionary<Point2D, char>();
        static Dictionary<Point2D, char> doorDict = new Dictionary<Point2D, char>();
        static Dictionary<char, Point2D> reverseDoorDict = new Dictionary<char, Point2D>();
        static Dictionary<char, Point2D> reverseKeyDict = new Dictionary<char, Point2D>();
        static Dictionary<char, int> keyReq = new Dictionary<char, int>();
        static Dictionary<(char, char), int> keyDistances = new Dictionary<(char, char), int>();
        static char[] allKeys;
        static int allKeysMask = 0;
        static Dictionary<(char lastKey, int keyState), int> bestKeyRemainder = new Dictionary<(char lastKey, int keyState), int>();
        static Point2D startingPoint = Point2D.Zero;
        static Point2D[] robotStartingPoints = new Point2D[4];

        enum Square
        {
            Open = 0,
            Wall,
            Key,
            Door
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Day 18 - Many-Worlds Interpretation");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            string[] lines = File.ReadAllLines(inputFile);

            grid = new Square[lines[0].Length, lines.Length];
            movementGrid = new Dictionary<Point2D, int>(grid.Length);

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    char c = lines[y][x];
                    if (c == '.')
                    {
                        grid[x, y] = Square.Open;
                    }
                    else if (c == '#')
                    {
                        grid[x, y] = Square.Wall;
                    }
                    else if (c >= 'a' && c <= 'z')
                    {
                        grid[x, y] = Square.Key;
                        //Write caps version to Key
                        keyDict[(x, y)] = (char)(c - 'a' + 'A');
                        reverseKeyDict[(char)(c - 'a' + 'A')] = (x, y);
                    }
                    else if (c >= 'A' && c <= 'Z')
                    {
                        grid[x, y] = Square.Door;
                        doorDict[(x, y)] = c;
                        reverseDoorDict[c] = (x, y);
                    }
                    else if (c == '@')
                    {
                        grid[x, y] = Square.Open;
                        startingPoint = (x, y);
                    }
                    else
                    {
                        throw new Exception($"Grid space unimplemented: {c}");
                    }
                }
            }

            allKeys = reverseKeyDict.Keys.ToArray();
            allKeysMask = (1 << allKeys.Length) - 1;

            //Print Central Room Colored
            //PrintRoom(startingPoint, new HashSet<char>());


            BuildKeyReqs();
            BuildKeyDistances();
            int shortest = GetShortestPath2('0', 0);
            Console.WriteLine($"The answer is: {shortest}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            robotStartingPoints[0] = startingPoint + Point2D.XAxis + Point2D.YAxis;
            robotStartingPoints[1] = startingPoint + Point2D.XAxis - Point2D.YAxis;
            robotStartingPoints[2] = startingPoint - Point2D.XAxis - Point2D.YAxis;
            robotStartingPoints[3] = startingPoint - Point2D.XAxis + Point2D.YAxis;

            grid[startingPoint.x, startingPoint.y] = Square.Wall;
            grid[startingPoint.x + 1, startingPoint.y] = Square.Wall;
            grid[startingPoint.x - 1, startingPoint.y] = Square.Wall;
            grid[startingPoint.x, startingPoint.y + 1] = Square.Wall;
            grid[startingPoint.x, startingPoint.y - 1] = Square.Wall;

            int output2 = 0;



            Console.WriteLine($"The answer is: {output2}");


            Console.WriteLine();
            Console.ReadKey();
        }

        static void BuildKeyReqs()
        {
            movementGrid.Clear();

            Queue<Point2D> pendingPoints = new Queue<Point2D>();
            pendingPoints.Enqueue(startingPoint);
            movementGrid.Add(startingPoint, 0);

            while (pendingPoints.Count > 0)
            {
                Point2D next = pendingPoints.Dequeue();
                int distance = movementGrid[next] + 1;

                foreach (Point2D adj in next.GetAdjacent())
                {
                    Square value = grid[adj.x, adj.y];
                    if (value == Square.Open || value == Square.Key || value == Square.Door)
                    {
                        //Can enter
                        if (movementGrid.GetValueOrDefault(adj, int.MaxValue) > distance)
                        {
                            //Update
                            movementGrid[adj] = distance;
                            if (!pendingPoints.Contains(adj))
                            {
                                pendingPoints.Enqueue(adj);
                            }
                        }
                    }
                }
            }

            int requiredKeys;
            foreach (char key in allKeys)
            {
                requiredKeys = 0;

                Point2D current = reverseKeyDict[key];

                while (current != startingPoint)
                {
                    int currentDistance = movementGrid[current];
                    foreach (Point2D adj in current.GetAdjacent())
                    {
                        if (movementGrid.GetValueOrDefault(adj, int.MaxValue) < currentDistance)
                        {
                            current = adj;
                            break;
                        }
                    }

                    if (doorDict.ContainsKey(current))
                    {
                        requiredKeys |= 1 << (doorDict[current] - 'A');
                    }
                }

                keyReq[key] = requiredKeys;
            }
        }

        static void BuildKeyDistances()
        {
            foreach (char key in allKeys)
            {
                movementGrid.Clear();

                Queue<Point2D> pendingPoints = new Queue<Point2D>();
                pendingPoints.Enqueue(reverseKeyDict[key]);
                movementGrid.Add(reverseKeyDict[key], 0);

                while (pendingPoints.Count > 0)
                {
                    Point2D next = pendingPoints.Dequeue();
                    int distance = movementGrid[next] + 1;

                    foreach (Point2D adj in next.GetAdjacent())
                    {
                        Square value = grid[adj.x, adj.y];
                        if (value == Square.Open || value == Square.Key || value == Square.Door)
                        {
                            //Can enter
                            if (movementGrid.GetValueOrDefault(adj, int.MaxValue) > distance)
                            {
                                //Update
                                movementGrid[adj] = distance;
                                if (!pendingPoints.Contains(adj))
                                {
                                    pendingPoints.Enqueue(adj);
                                }
                            }
                        }
                    }
                }

                keyDistances[('0', key)] = movementGrid[startingPoint];

                foreach (char key2 in allKeys)
                {
                    keyDistances[(key, key2)] = movementGrid[reverseKeyDict[key2]];
                }
            }
        }

        static int GetShortestPath2(char lastKey, int keys)
        {
            if (bestKeyRemainder.ContainsKey((lastKey, keys)))
            {
                return bestKeyRemainder[(lastKey, keys)];
            }

            if (keys == allKeysMask)
            {
                return 0;
            }

            var options = allKeys
                .Where(c => (keys & (1 << (c - 'A'))) == 0)
                .Where(c => (keys | keyReq[c]) == keys)
                .Select(c => (c, keyDistances[(lastKey, c)]))
                .OrderBy(x => x.Item2);

            int shortestDistance = int.MaxValue;

            foreach ((char key, int distance) in options)
            {
                keys |= 1 << (key - 'A');
                shortestDistance = Math.Min(shortestDistance, distance + GetShortestPath2(key, keys));
                keys &= ~(1 << (key - 'A'));
            }

            bestKeyRemainder[(lastKey, keys)] = shortestDistance;

            return shortestDistance;
        }

        static void PrintRoom(Point2D start, HashSet<char> obtainedKeys)
        {
            Console.ReadLine();
            movementGrid.Clear();

            Queue<Point2D> pendingPoints = new Queue<Point2D>();
            pendingPoints.Enqueue(start);
            movementGrid.Add(start, 0);

            while (pendingPoints.Count > 0)
            {
                Point2D next = pendingPoints.Dequeue();
                int distance = movementGrid[next] + 1;

                foreach (Point2D adj in next.GetAdjacent())
                {
                    Square value = grid[adj.x, adj.y];
                    if (value == Square.Open || value == Square.Key || (value == Square.Door && obtainedKeys.Contains(doorDict[adj])))
                    {
                        //Can enter
                        if (movementGrid.GetValueOrDefault(adj, int.MaxValue) > distance)
                        {
                            //Update
                            movementGrid[adj] = distance;
                            if (!pendingPoints.Contains(adj))
                            {
                                pendingPoints.Enqueue(adj);
                            }
                        }
                    }
                }
            }

            Console.WriteLine();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    switch (grid[x, y])
                    {
                        case Square.Open:
                            Console.BackgroundColor = movementGrid.ContainsKey((x, y)) ? ConsoleColor.Red : ConsoleColor.Black;
                            Console.Write(' ');
                            break;

                        case Square.Wall:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(' ');
                            break;

                        case Square.Key:
                            Console.BackgroundColor = movementGrid.ContainsKey((x, y)) ? ConsoleColor.Red : ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(keyDict[(x, y)]);
                            break;

                        case Square.Door:
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(doorDict[(x, y)]);
                            break;

                        default:
                            throw new Exception();
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }
    }
}
