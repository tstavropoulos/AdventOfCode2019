using System;
using System.Collections.Generic;
using System.Text;

namespace AoCTools.Maze
{
    class MazeRoom
    {
        public readonly Point2D pos;

        //Can set this for the puzzle
        public static Dictionary<Point2D, MazeRoom> rooms = new Dictionary<Point2D, MazeRoom>();

        public MazeRoom N = null;
        public MazeRoom S = null;
        public MazeRoom E = null;
        public MazeRoom W = null;

        public enum Direction
        {
            North = 0,
            South,
            East,
            West,
            MAX
        }

        public MazeRoom(int x, int y)
        {
            pos = (x, y);

            if (rooms.ContainsKey((x, y)))
            {
                throw new Exception();
            }

            rooms.Add(pos, this);
        }

        public MazeRoom(int x, int y, Direction source)
            : this(x, y)
        {
            switch (source)
            {
                case Direction.North:
                    S = rooms[(x, y - 1)];
                    break;

                case Direction.South:
                    N = rooms[(x, y + 1)];
                    break;

                case Direction.East:
                    W = rooms[(x - 1, y)];
                    break;

                case Direction.West:
                    E = rooms[(x + 1, y)];
                    break;

                default: throw new Exception();
            }
        }

        public void PopulateExisting()
        {
            if (S == null && rooms.ContainsKey((pos.x, pos.y - 1)))
            {
                S = rooms[(pos.x, pos.y - 1)];
            }

            if (N == null && rooms.ContainsKey((pos.x, pos.y + 1)))
            {
                N = rooms[(pos.x, pos.y + 1)];
            }

            if (W == null && rooms.ContainsKey((pos.x - 1, pos.y)))
            {
                W = rooms[(pos.x - 1, pos.y)];
            }

            if (E == null && rooms.ContainsKey((pos.x + 1, pos.y)))
            {
                E = rooms[(pos.x + 1, pos.y)];
            }
        }

        public MazeRoom TravelAndCreate(Direction dir)
        {
            switch (dir)
            {
                case Direction.North: return N ?? (rooms.TryGetValue((pos.x, pos.y + 1), out N) ? N : (N = new MazeRoom(pos.x, pos.y + 1, dir)));
                case Direction.South: return S ?? (rooms.TryGetValue((pos.x, pos.y - 1), out S) ? S : (S = new MazeRoom(pos.x, pos.y - 1, dir)));
                case Direction.East: return E ?? (rooms.TryGetValue((pos.x + 1, pos.y), out E) ? E : (E = new MazeRoom(pos.x + 1, pos.y, dir)));
                case Direction.West: return W ?? (rooms.TryGetValue((pos.x - 1, pos.y), out W) ? W : (W = new MazeRoom(pos.x - 1, pos.y, dir)));

                default: throw new Exception();
            }
        }

        public IEnumerable<MazeRoom> GetConnectedRooms(MazeRoom source = null)
        {
            if (N != null && N != source)
            {
                yield return N;
            }

            if (S != null && S != source)
            {
                yield return S;
            }

            if (E != null && E != source)
            {
                yield return E;
            }

            if (W != null && W != source)
            {
                yield return W;
            }
        }
    }
}
