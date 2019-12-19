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
        static Dictionary<char, Point2D> reverseKeyDict = new Dictionary<char, Point2D>();
        static int allKeysMask;
        static char[] allKeys;

        static Dictionary<(char, char), int> keyDistances = new Dictionary<(char, char), int>();
        static Point2D startingPoint = Point2D.Zero;
        static Dictionary<(char lastKey, int keyState), int> bestKeyRemainder = new Dictionary<(char lastKey, int keyState), int>();


        static Dictionary<char, int> keyReq = new Dictionary<char, int>();
        static Dictionary<char, int> keyAssignment = new Dictionary<char, int>();
        static Point2D[] robotStartingPoints = new Point2D[4];
        static Dictionary<QuadState, int> bestQuadKeyRemainder = new Dictionary<QuadState, int>();

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

            BuildKeyReqs();
            BuildKeyDistances();
            int shortest = GetShortestPath('0', 0);

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

            BuildQuadKeyDistances();


            Console.WriteLine($"The robust answer is: {GetShortestPath("1234")}");

            int[] robotKeysMasks = new int[4];

            for (int i = 0; i < 4; i++)
            {
                robotKeysMasks[i] = allKeysMask;
            }

            foreach (char key in allKeys)
            {
                robotKeysMasks[keyAssignment[key]] &= ~(1 << (key - 'A'));
            }

            int newShortest = 0;

            for (int i = 0; i < 4; i++)
            {
                newShortest += GetShortestPath((char)('1' + i), robotKeysMasks[i]);
            }

            Console.WriteLine($"The sum of naive segments answer is: {newShortest}");

            Console.WriteLine();
            //Console.ReadKey();
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

        static void BuildQuadKeyDistances()
        {
            keyDistances.Clear();
            bestKeyRemainder.Clear();

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

                for (int i = 0; i < 4; i++)
                {
                    if (movementGrid.ContainsKey(robotStartingPoints[i]))
                    {
                        keyDistances[((char)('1' + i), key)] = movementGrid[robotStartingPoints[i]];
                        keyAssignment[key] = i;
                        break;
                    }
                }

                foreach (char key2 in allKeys)
                {
                    if (movementGrid.ContainsKey(reverseKeyDict[key2]))
                    {
                        keyDistances[(key, key2)] = movementGrid[reverseKeyDict[key2]];
                    }
                }
            }
        }

        static int GetShortestPath(char lastKey, int keys)
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
                .Select(c => (c, keyDistances[(lastKey, c)]));

            int shortestDistance = int.MaxValue;

            foreach ((char key, int distance) in options)
            {
                keys |= 1 << (key - 'A');
                shortestDistance = Math.Min(shortestDistance, distance + GetShortestPath(key, keys));
                keys &= ~(1 << (key - 'A'));
            }

            bestKeyRemainder[(lastKey, keys)] = shortestDistance;

            return shortestDistance;
        }

        static int GetShortestPath(in QuadState lastKeys)
        {
            if (bestQuadKeyRemainder.ContainsKey(lastKeys))
            {
                return bestQuadKeyRemainder[lastKeys];
            }

            if (lastKeys.keys == allKeysMask)
            {
                return 0;
            }

            var options = allKeys
                .Where(lastKeys.IsUnused)
                .Where(lastKeys.HasRequiredKeys);

            int shortestDistance = int.MaxValue;

            foreach (char key in options)
            {
                shortestDistance = Math.Min(shortestDistance, keyDistances[(lastKeys[keyAssignment[key]], key)] + GetShortestPath(lastKeys.GetWith(key)));
            }

            bestQuadKeyRemainder[lastKeys] = shortestDistance;

            return shortestDistance;
        }

        readonly struct QuadState
        {
            public readonly char a;
            public readonly char b;
            public readonly char c;
            public readonly char d;
            public readonly int keys;

            public QuadState(char a, char b, char c, char d, int keys)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.keys = keys;
            }

            public static implicit operator QuadState((char a, char b, char c, char d) value) =>
                new QuadState(value.a, value.b, value.c, value.d, 0);

            public static implicit operator QuadState(string value) =>
                new QuadState(value[0], value[1], value[2], value[3], 0);

            public readonly char this[int i] => i switch
            {
                0 => a,
                1 => b,
                2 => c,
                3 => d,
                _ => throw new Exception(),
            };

            public readonly QuadState GetWith(char key) => keyAssignment[key] switch
            {
                0 => new QuadState(key, b, c, d, keys | (1 << (key - 'A'))),
                1 => new QuadState(a, key, c, d, keys | (1 << (key - 'A'))),
                2 => new QuadState(a, b, key, d, keys | (1 << (key - 'A'))),
                3 => new QuadState(a, b, c, key, keys | (1 << (key - 'A'))),
                _ => throw new Exception(),
            };

            public readonly override int GetHashCode() => HashCode.Combine(a, b, c, d, keys);
            public readonly override bool Equals(object obj)
            {
                if (!(obj is QuadState other))
                {
                    return false;
                }

                return a == other.a && b == other.b && c == other.c && d == other.d && keys == other.keys;
            }

            public readonly bool IsUnused(char key) => (keys & (1 << (key - 'A'))) == 0;
            public readonly bool HasRequiredKeys(char key) => (keys | keyReq[key]) == keys;
        }
    }

}
