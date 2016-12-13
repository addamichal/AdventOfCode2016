using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstMaze = new Maze(40, 40, 1362, int.MaxValue);
            var shortestPath = firstMaze.GetShortestPath(new Position(1, 1), new Position(31, 39));
            Console.WriteLine(shortestPath.GetLength());

            var secondMaze = new Maze(40, 40, 1362, 50);
            Console.WriteLine(secondMaze.GetMaxNumberOfVisited());
        }

        public class Maze
        {
            private readonly int _xSize;
            private readonly int _ySize;
            private readonly int _magicNumber;
            private readonly int _maxLenghtSize;
            private char[,] _maze;
            private bool[,] _visited;

            public Maze(int xSize, int ySize, int magicNumber, int maxLenghtSize)
            {
                _xSize = xSize;
                _ySize = ySize;
                _magicNumber = magicNumber;
                _maxLenghtSize = maxLenghtSize;

                _visited = new bool[ySize, xSize];
                _maze = new char[ySize, xSize];

                for (int y = 0; y < ySize; y++)
                {
                    for (int x = 0; x < xSize; x++)
                    {
                        var formula = (x * x + 3 * x + 2 * x * y + y + y * y) + _magicNumber;
                        string binary = Convert.ToString(formula, 2);
                        var numOfOnes = binary.ToCharArray().Count(w => w == '1');

                        if (numOfOnes % 2 == 0)
                            _maze[y, x] = '.';
                        else
                            _maze[y, x] = '#';
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

            public Position GetShortestPath(Position startPosition, Position finishPosition)
            {
                var queue = new Queue<Position>();
                queue.Enqueue(startPosition);

                while (queue.Count != 0)
                {
                    var currentPosition = queue.Dequeue();

                    if (currentPosition.Equals(finishPosition))
                        return currentPosition;

                    var up = new Position(currentPosition.X, currentPosition.Y - 1, currentPosition);
                    var down = new Position(currentPosition.X, currentPosition.Y + 1, currentPosition);
                    var left = new Position(currentPosition.X - 1, currentPosition.Y, currentPosition);
                    var right = new Position(currentPosition.X + 1, currentPosition.Y, currentPosition);

                    CanGoToPosition(up, currentPosition, queue);
                    CanGoToPosition(down, currentPosition, queue);
                    CanGoToPosition(left, currentPosition, queue);
                    CanGoToPosition(right, currentPosition, queue);
                }

                return null;
            }

            public int GetMaxNumberOfVisited()
            {
                GetShortestPath(new Position(1, 1), new Position(99, 99));

                int visitedCounter = 0;
                for (int y = 0; y < _ySize; y++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        if (_visited[y, x])
                            visitedCounter++;
                    }
                }

                return visitedCounter;
            }

            private void CanGoToPosition(Position nextPosition, Position currentPosition, Queue<Position> queue)
            {
                if (IsAccessible(nextPosition) && !currentPosition.ContainsPosition(nextPosition) && currentPosition.GetLength() <= _maxLenghtSize)
                {
                    _visited[nextPosition.Y, nextPosition.X] = true;
                    queue.Enqueue(nextPosition);
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

            public bool ContainsPosition(Position position)
            {
                var currentPosition = this;
                while (currentPosition != null)
                {
                    if (currentPosition.Equals(position))
                        return true;

                    currentPosition = currentPosition.PreviousPosition;
                }

                return false;
            }

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
}
