using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AoCTools;
using AoCTools.IntCode;

namespace Day17
{
    class Program
    {
        private const string inputFile = @"../../../../input17.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 17 - Set and Forget");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            StringBuilder builder = new StringBuilder();

            IntCode machine = new IntCode(
                name: "Star 1",
                regs: regs,
                fixedInputs: Array.Empty<long>(),
                output: x => builder.Append((char)x));

            machine.SyncRun();
            string text = builder.ToString();

            string[] lines = text.Split('\n').Where(x => x.Length != 0).ToArray();

            int alignmentParam = 0;

            for (int y = 1; y < lines.Length - 1; y++)
            {
                for (int x = 1; x < lines[0].Length - 1; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        if (lines[y - 1][x] == '#' && lines[y + 1][x] == '#' && lines[y][x + 1] == '#' && lines[y][x - 1] == '#')
                        {
                            alignmentParam += x * y;
                        }
                    }
                }
            }

            Console.WriteLine($"The answer is: {alignmentParam}");


            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            HashSet<Point2D> scaffold = new HashSet<Point2D>();
            Point2D startingPoint = Point2D.Zero;

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        scaffold.Add((x, y));
                    }
                    else if (lines[y][x] == '^')
                    {
                        startingPoint = (x, y);
                    }
                }
            }

            List<string> path = GetPath(startingPoint, scaffold);

            var compressed = Compress(path);

            IEnumerable<long> input =
                compressed.Seq.Select(x => (long)x).Append(10)
                .Concat(compressed.A.Select(x => (long)x)).Append(10)
                .Concat(compressed.B.Select(x => (long)x)).Append(10)
                .Concat(compressed.C.Select(x => (long)x)).Append(10)
                .Append((long)'n').Append(10);

            regs[0] = 2;

            long dust = 0;

            IntCode machine2 = new IntCode(
                name: "Star 2",
                regs: regs,
                fixedInputs: input,
                output: x => dust = x);

            machine2.SyncRun();

            Console.WriteLine($"Dust collected: {dust}");

            Console.WriteLine();
            Console.ReadKey();
        }

        /// <summary>
        /// Get the compressed sequence representation
        /// </summary>
        static (string A, string B, string C, string Seq) Compress(List<string> sequence)
        {
            string fullSequence = string.Join(",", sequence);

            for (int aLen = 10; aLen >= 2; aLen -= 2)
            {
                string aSeq = string.Join(",", sequence.GetRange(0, aLen));

                if (aSeq.Length > 20)
                {
                    continue;
                }

                int nextIndexA = aSeq.Length + 1;
                int aCount = 1;
                while (fullSequence.Substring(nextIndexA, aSeq.Length) == aSeq)
                {
                    nextIndexA += aSeq.Length + 1;
                    aCount++;
                }

                for (int bLen = 10; bLen >= 2; bLen -= 2)
                {
                    string bSeq = string.Join(",", sequence.GetRange(aLen * aCount, bLen));

                    if (bSeq.Length > 20)
                    {
                        continue;
                    }

                    int nextIndexB = nextIndexA + bSeq.Length + 1;
                    int bCount = 1;
                    int abCount = 0;
                    bool match = true;
                    while (match)
                    {
                        match = false;
                        if (fullSequence.Substring(nextIndexB, aSeq.Length) == aSeq)
                        {
                            match = true;
                            nextIndexB += aSeq.Length + 1;
                            abCount++;
                        }

                        if (fullSequence.Substring(nextIndexB, bSeq.Length) == bSeq)
                        {
                            match = true;
                            nextIndexB += bSeq.Length + 1;
                            bCount++;
                        }
                    }

                    for (int cLen = 10; cLen >= 2; cLen -= 2)
                    {
                        string cSeq = string.Join(",", sequence.GetRange(aLen * (aCount + abCount) + bLen * bCount, cLen));

                        if (cSeq.Length > 20)
                        {
                            continue;
                        }

                        string output = Try(
                            A: aSeq,
                            B: bSeq,
                            C: cSeq,
                            sequence: fullSequence);

                        if (output != null)
                        {
                            return (aSeq, bSeq, cSeq, output);
                        }
                    }
                }
            }

            throw new Exception();
        }

        /// <summary>
        /// Test if we can compose the sequence out of the segements
        /// </summary>
        static string Try(string A, string B, string C, string sequence)
        {
            int index = 0;

            List<string> output = new List<string>();

            while(index < sequence.Length)
            {
                if (index + A.Length <= sequence.Length && sequence.Substring(index, A.Length) == A)
                {
                    index += A.Length + 1;
                    output.Add("A");
                    continue;
                }
                else if (index + B.Length <= sequence.Length && sequence.Substring(index, B.Length) == B)
                {
                    index += B.Length + 1;
                    output.Add("B");
                    continue;
                }
                else if (index + C.Length <= sequence.Length && sequence.Substring(index, C.Length) == C)
                {
                    index += C.Length + 1;
                    output.Add("C");
                    continue;
                }
                else
                {
                    return null;
                }
            }

            string outputLine = string.Join(",", output);

            return outputLine.Length <= 20 ? outputLine : null;
        }


        /// <summary>
        /// Greedy path algorithm.  I assume at every crossing that it's meant to keep going.
        /// This appears to be the proper intended means of getting the complete path.
        /// At least, it seems to work with the examples and my input
        /// </summary>
        static List<string> GetPath(Point2D start, HashSet<Point2D> scaffold)
        {
            Point2D heading = -Point2D.YAxis;
            Point2D current = start;

            HashSet<Point2D> remainingPoints = new HashSet<Point2D>(scaffold);

            List<string> output = new List<string>();

            while (remainingPoints.Count > 0)
            {
                if (remainingPoints.Contains(current + heading.Rotate(false)))
                {
                    output.Add("R");
                    heading = heading.Rotate(false);
                }
                else if (remainingPoints.Contains(current + heading.Rotate(true)))
                {
                    output.Add("L");
                    heading = heading.Rotate(true);
                }
                else
                {
                    throw new Exception();
                }

                int count = 0;

                while (scaffold.Contains(current + heading))
                {
                    current += heading;
                    count++;
                    remainingPoints.Remove(current);
                }

                output.Add(count.ToString());
            }

            return output;
        }
    }
}
