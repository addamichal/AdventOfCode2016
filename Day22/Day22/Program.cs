using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Skip(2).ToList();
            var allNodes = lines.Select(w => Node.Parse(w)).ToList();

            var solution = new Solution(allNodes, 35, 29);

            var pairs = solution.GetAnswer1();
            Console.WriteLine(pairs);

            var result = solution.GetAnswer2();
            Console.WriteLine(result);
        }
    }

    public class Solution
    {
        private readonly int _width;
        private readonly int _height;
        private HashSet<State> ExistingStates = new HashSet<State>();
        private Dictionary<Position, Node> _nodes;

        public Solution(List<Node> nodes, int width, int height)
        {
            _nodes = new Dictionary<Position, Node>();
            foreach (var node in nodes)
            {
                var position = node.GetPosition();
                _nodes.Add(position, node);
            }
            _width = _nodes.Last().Key.X + 1;
            _height = _nodes.Last().Key.Y + 1;
        }

        public int GetAnswer1()
        {
            var allNodes = _nodes.Values.ToList();

            int pairs = 0;
            for (int i = 0; i < allNodes.Count; i++)
            {
                var currentNode = allNodes[i];
                if (currentNode.Use == 0)
                    continue;

                for (int j = 0; j < allNodes.Count; j++)
                {
                    var nextNode = allNodes[j];
                    if (currentNode.Name == nextNode.Name)
                        continue;

                    if (currentNode.Used < nextNode.Avail)
                    {
                        pairs++;
                    }
                }
            }
            return pairs;
        }

        public int GetAnswer2()
        {
            var queue = new Queue<State>();
            var startPosition = _nodes.SingleOrDefault(w => w.Value.Used == 0).Key;

            var endDataPosition = new Position(0, 0);

            var startDataPosition = new Position(_width - 1, 0);
            var state = new State(startPosition, startDataPosition);
            queue.Enqueue(state);

            this.ExistingStates.Add(state);

            while (queue.Count != 0)
            {
                var currentState = queue.Dequeue();
                var currentPosition = currentState.CurrentPosition;
                var currentDataPosition = currentState.DataPosition;

                if (currentDataPosition.Equals(endDataPosition))
                {
                    return currentPosition.GetLength() - 1;
                }

                var up = new Position(currentPosition.X, currentPosition.Y - 1, currentPosition);
                var down = new Position(currentPosition.X, currentPosition.Y + 1, currentPosition);
                var left = new Position(currentPosition.X - 1, currentPosition.Y, currentPosition);
                var right = new Position(currentPosition.X + 1, currentPosition.Y, currentPosition);

                EnqueueIfPossible(up, currentPosition, currentDataPosition, queue);
                EnqueueIfPossible(down, currentPosition, currentDataPosition, queue);
                EnqueueIfPossible(left, currentPosition, currentDataPosition, queue);
                EnqueueIfPossible(right, currentPosition, currentDataPosition, queue);
            }

            return 0;
        }

        private void EnqueueIfPossible(Position nextPosition, Position currentPosition, Position currentDataPosition, Queue<State> queue)
        {
            if (CanVisit(nextPosition))
            {
                //we need to switch data here...
                //var nextNode = _nodes[up];
                //var previousNode = _nodes[up.PreviousPosition];
                //
                ////move data
                //previousNode.Used = nextNode.Used;
                //previousNode.Avail = nextNode.Avail;

                Position newDataPosition;
                if (nextPosition.Equals(currentDataPosition))
                    newDataPosition = new Position(currentPosition.X, currentPosition.Y, currentDataPosition);
                else
                    newDataPosition = new Position(currentDataPosition.X, currentDataPosition.Y, currentDataPosition.PreviousPosition);

                var newState = new State(nextPosition, newDataPosition);
                if (!this.ExistingStates.Contains(newState))
                {
                    this.ExistingStates.Add(newState);
                    queue.Enqueue(newState);
                }
            }
        }

        public bool CanVisit(Position nextPosition)
        {
            if (nextPosition.X < _width && nextPosition.X >= 0 && nextPosition.Y < _height && nextPosition.Y >= 0)
            {
                var currentNode = _nodes[nextPosition.PreviousPosition];
                if (!_nodes.ContainsKey(nextPosition))
                    return false;

                var nextNode = _nodes[nextPosition];

                if (nextNode.Used <= currentNode.Size) //not sure if this is ok...
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class State
    {
        public Position CurrentPosition { get; set; }
        public Position DataPosition { get; set; }

        public State(Position currentPosition, Position dataPosition)
        {
            CurrentPosition = currentPosition;
            DataPosition = dataPosition;
        }

        public override string ToString()
        {
            return $"{CurrentPosition}, {DataPosition}";
        }

        protected bool Equals(State other)
        {
            return Equals(CurrentPosition, other.CurrentPosition) && Equals(DataPosition, other.DataPosition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((State)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CurrentPosition != null ? CurrentPosition.GetHashCode() : 0) * 397) ^ (DataPosition != null ? DataPosition.GetHashCode() : 0);
            }
        }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position PreviousPosition { get; set; }

        public Position(int x, int y, Position previousPosition = null)
        {
            this.X = x;
            this.Y = y;
            this.PreviousPosition = previousPosition;
        }

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }

        protected bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public int GetLength()
        {
            int size = 0;
            var currentPosition = this;
            while (currentPosition != null)
            {
                currentPosition = currentPosition.PreviousPosition;
                size++;
            }
            return size;
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Used { get; set; }
        public int Avail { get; set; }
        public int Use { get; set; }

        public Node(string name, int size, int used, int avail, int use)
        {
            this.Name = name;
            this.Size = size;
            this.Used = used;
            this.Use = use;
            this.Avail = avail;
        }

        ///dev/grid/node-x0-y0     89T   67T    22T   75%
        public static Node Parse(string input)
        {
            var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new Node(parts[0], int.Parse(parts[1].Replace("T", "")), int.Parse(parts[2].Replace("T", "")), int.Parse(parts[3].Replace("T", "")), int.Parse(parts[4].Replace("%", "")));
        }

        // /dev/grid/node-x0-y0
        public Position GetPosition()
        {
            var node = this.Name.Replace("/dev/grid/node-", "");
            var parts = node.Split('-');
            return new Position(int.Parse(parts[0].Replace("x", "")), int.Parse(parts[1].Replace("y", "")));
        }
    }
}
