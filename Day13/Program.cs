using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCTools;
using AoCTools.IntCode;

namespace Day13
{
    class Program
    {
        private const string inputFile = @"../../../../input13.txt";

        static Dictionary<Point2D, int> tileMap = new Dictionary<Point2D, int>();

        static Point2D ballPos;
        static Point2D paddlePos;
        static int inputStage = 0;
        static int cachedX = 0;
        static Point2D cachedPoint = Point2D.Zero;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 13");
            Console.WriteLine("Star 1");
            Console.WriteLine();

            long[] regs = File.ReadAllText(inputFile).Split(',').Select(long.Parse).ToArray();

            IntCode machine = new IntCode(
                name: "Star 1",
                regs: regs,
                fixedInputs: new long[0],
                input: null,
                output: RenderOutput1);


            machine.SyncRun();

            Console.WriteLine($"The answer is: {tileMap.Values.Where(x => x == 2).Count()}");

            Console.WriteLine();
            Console.WriteLine("Star 2");
            Console.WriteLine();

            //Reset
            tileMap.Clear();
            inputStage = 0;

            //// Ineffective Tree solution left for Posterity's sake

            //MoveTree moveTree = new MoveTree()
            //while (true)
            //{
            //    tileMap.Clear();
            //    inputStage = 0;

            //    IntCode machine2 = new IntCode(
            //        name: "Star 2",
            //        regs: regs,
            //        fixedInputs: new long[0],
            //        input: moveTree.NextMove,
            //        output: RenderOutput1);

            //    machine2[0] = 2;

            //    machine2.SyncRun();

            //    if (tileMap.Values.Where(x => x == 2).Count() == 0)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        moveTree.MarkFailure();
            //    }
            //}

            IntCode machine2 = new IntCode(
                name: "Star 2",
                regs: regs,
                fixedInputs: new long[0],
                input: Input2,
                output: RenderOutput2);

            machine2[0] = 2;

            machine2.SyncRun();

            Console.WriteLine($"The answer is: {score}");

            Console.WriteLine();
            Console.ReadKey();
        }

        static int score = 0;

        static void RenderOutput1(long value)
        {
            switch (inputStage)
            {
                case 0:
                    cachedX = (int)value;
                    inputStage = 1;
                    break;

                case 1:
                    if (cachedX == -1 && value == 0)
                    {
                        inputStage = 3;
                    }
                    else
                    {
                        cachedPoint = new Point2D(cachedX, (int)value);
                        inputStage = 2;
                    }
                    break;

                case 2:
                    tileMap[cachedPoint] = (int)value;
                    inputStage = 0;
                    break;

                case 3:
                    //Score
                    score = (int)value;
                    inputStage = 0;
                    break;


                default:
                    throw new Exception();
            }
        }

        static void RenderOutput2(long value)
        {
            switch (inputStage)
            {
                case 0:
                    cachedX = (int)value;
                    inputStage = 1;
                    break;

                case 1:
                    if (cachedX == -1 && value == 0)
                    {
                        inputStage = 3;
                    }
                    else
                    {
                        cachedPoint = new Point2D(cachedX, (int)value);
                        inputStage = 2;
                    }
                    break;

                case 2:
                    if (value == 3)
                    {
                        paddlePos = cachedPoint;
                    }
                    else if (value == 4)
                    {
                        ballPos = cachedPoint;
                    }

                    tileMap[cachedPoint] = (int)value;
                    inputStage = 0;
                    break;

                case 3:
                    //Score
                    score = (int)value;
                    inputStage = 0;
                    break;

                default:
                    throw new Exception();
            }
        }

        static long Input2()
        {
            if (ballPos.x == paddlePos.x)
            {
                return 0;
            }

            return ballPos.x < paddlePos.x ? -1 : 1;
        }
    }

    class MoveTree
    {
        private readonly List<MoveNode> children = new List<MoveNode>();

        private MoveNode currentNode = null;

        public MoveTree()
        {
            for (int move = -1; move <= 1; move++)
            {
                children.Add(new MoveNode(null, move));
            }
        }

        public void MarkFailure()
        {
            currentNode.MarkDead();
            currentNode = null;
        }

        public long NextMove()
        {
            if (currentNode == null)
            {
                foreach (MoveNode node in children)
                {
                    if (node.alive)
                    {
                        currentNode = node;
                        break;
                    }
                }
            }
            else
            {
                currentNode = currentNode.NextAvailableChild();
            }

            return currentNode.direction;
        }
    }

    class MoveNode
    {
        public readonly int direction;

        public bool alive = true;
        public readonly MoveNode parent;
        public readonly List<MoveNode> children = new List<MoveNode>();

        public MoveNode(MoveNode parent, int direction)
        {
            this.parent = parent;
            this.direction = direction;
        }

        public MoveNode NextAvailableChild()
        {
            if (!alive)
            {
                throw new Exception();
            }

            if (children.Count == 0)
            {
                PopulateChildren();
            }

            foreach (MoveNode child in children)
            {
                if (child.alive)
                {
                    return child;
                }
            }

            throw new Exception();
        }

        public void PopulateChildren()
        {
            for (int i = -1; i <= 1; i++)
            {
                children.Add(new MoveNode(this, i));
            }
        }

        public void MarkDead()
        {
            alive = false;
            children.Clear();
            parent?.CheckDead();
        }

        protected void CheckDead()
        {
            if (children.All(x => !x.alive))
            {
                MarkDead();
            }
        }
    }
}
