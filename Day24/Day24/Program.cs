using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").ToList();
            var solution = new Solution(lines);
            var answer1 = solution.GetAnswer();
            Console.WriteLine(answer1);
            var answer2 = solution.GetAnswer(true);
            Console.WriteLine(answer2);
        }
    }

    public class Solution
    {
        private Dictionary<Path, int> cache = new Dictionary<Path, int>();
        private Maze _maze;

        public Solution(List<string> lines)
        {
            _maze = new Maze(lines);
        }

        public int GetAnswer(bool alsoReturn = false)
        {
            int min = int.MaxValue;
            var positions = _maze.PositionsToVisit.Keys.ToList();
            positions.Remove(0);
            var permutations = Permute(positions);
            foreach (var permutation in permutations)
            {
                permutation.Insert(0, 0);
                if (alsoReturn)
                    permutation.Add(0);
            }

            foreach (var permutation in permutations)
            {
                int currentDistance = 0;
                for (int i = 0; i < permutation.Count - 1; i++)
                {
                    var fromPosition = permutation[i];
                    var toPosition = permutation[i + 1];

                    var from = _maze.PositionsToVisit[fromPosition];
                    var to = _maze.PositionsToVisit[toPosition];

                    var path = new Path() { From = from, To = to };

                    int distance;
                    if (cache.ContainsKey(path))
                        distance = cache[path];
                    else
                    {
                        distance = _maze.GetShortestPath(from, to);
                        cache.Add(path, distance);
                    }

                    currentDistance += distance;
                }

                if (currentDistance < min)
                {
                    min = currentDistance;
                }
            }

            return min;
        }

        private static List<List<int>> Permute(List<int> set)
        {
            List<List<int>> result = new List<List<int>>();

            Action<int> permute = null;
            permute = start =>
            {
                if (start == set.Count)
                {
                    result.Add(set.ToList());
                }
                else
                {
                    List<int> swaps = new List<int>();
                    for (int i = start; i < set.Count; i++)
                    {
                        if (swaps.Contains(set[i])) continue; // skip if we already done swap with this item
                        swaps.Add(set[i]);

                        Swap(set, start, i);
                        permute(start + 1);
                        Swap(set, start, i);
                    }
                }
            };

            permute(0);

            return result;
        }

        private static void Swap(List<int> set, int index1, int index2)
        {
            int temp = set[index1];
            set[index1] = set[index2];
            set[index2] = temp;
        }
    }

    public class Path
    {
        public Position From { get; set; }
        public Position To { get; set; }

        public override string ToString()
        {
            return $"{nameof(From)}: {From}, {nameof(To)}: {To}";
        }

        protected bool Equals(Path other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Path)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From != null ? From.GetHashCode() : 0) * 397) ^ (To != null ? To.GetHashCode() : 0);
            }
        }
    }

    public class Maze
    {
        public HashSet<Position> AlreadyVisitedPositions = new HashSet<Position>();
        public Dictionary<int, Position> PositionsToVisit = new Dictionary<int, Position>();
        private readonly int _xSize;
        private readonly int _ySize;
        private char[,] _maze;

        public Maze(List<string> lines)
        {
            _xSize = lines[0].Length;
            _ySize = lines.Count;

            _maze = new char[_ySize, _xSize];

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                        _maze[y, x] = '#';
                    else if (lines[y][x] == '.')
                        _maze[y, x] = '.';
                    else
                    {
                        var number = int.Parse(lines[y][x].ToString());
                        PositionsToVisit.Add(number, new Position(x, y));
                        _maze[y, x] = '.';
                    }
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < _ySize; y++)
            {
                for (int x = 0; x < _xSize; x++)
                {
                    stringBuilder.Append(_maze[y, x]);
                }
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }

        public int GetShortestPath(Position startPosition, Position finishPosition)
        {
            AlreadyVisitedPositions = new HashSet<Position>();

            var queue = new Queue<Position>();
            queue.Enqueue(startPosition);
            AlreadyVisitedPositions.Add(startPosition);

            while (queue.Count != 0)
            {
                var currentPosition = queue.Dequeue();
                if (currentPosition.Equals(finishPosition))
                    return currentPosition.GetLength();

                var up = new Position(currentPosition.X, currentPosition.Y - 1, currentPosition);
                var down = new Position(currentPosition.X, currentPosition.Y + 1, currentPosition);
                var left = new Position(currentPosition.X - 1, currentPosition.Y, currentPosition);
                var right = new Position(currentPosition.X + 1, currentPosition.Y, currentPosition);

                CanGoToPosition(up, queue);
                CanGoToPosition(down, queue);
                CanGoToPosition(left, queue);
                CanGoToPosition(right, queue);
            }

            return -1;
        }

        private void CanGoToPosition(Position nextPosition, Queue<Position> queue)
        {
            if (IsAccessible(nextPosition) && !AlreadyVisitedPositions.Contains(nextPosition))
            {
                queue.Enqueue(nextPosition);
                AlreadyVisitedPositions.Add(nextPosition);
            }
        }

        private bool IsAccessible(Position position)
        {
            if (position.X >= 0 && position.Y >= 0 && position.X < _xSize && position.Y < _ySize)
            {
                return _maze[position.Y, position.X] == '.';
            }
            return false;
        }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", X, Y);
        }

        public Position(int x, int y, Position previousPosition = null)
        {
            this.PreviousPosition = previousPosition;
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj.ToString() == this.ToString();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public Position PreviousPosition { get; set; }

        public int GetLength()
        {
            var currentPosition = this;
            int counter = 0;
            while (true)
            {
                if (currentPosition == null)
                    return counter - 1;

                currentPosition = currentPosition.PreviousPosition;

                counter++;
            }
        }
    }
}
