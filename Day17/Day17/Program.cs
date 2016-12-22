using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "ioramepc";
            var solution = new Solution();
            var answer1 = solution.Answer1(input);
            var answer2 = solution.Answer2(input);
            Console.WriteLine(answer1);
            Console.WriteLine(answer2);
        }
    }

    public class Solution
    {
        private MD5CryptoServiceProvider md5;

        public Solution()
        {
            md5 = new MD5CryptoServiceProvider();
        }

        public string Answer1(string input)
        {
            return GetPaths(input).OrderBy(w => w.GetLength()).FirstOrDefault().Path;
        }

        public int Answer2(string input)
        {
            var paths = GetPaths(input);
            if (!paths.Any())
                return 0;

            var lastPath = paths.Last();
            return lastPath.GetLength();
        }

        public List<Position> GetPaths(string input)
        {
            var paths = new List<Position>();
            var startPosition = new Position(0, 0);
            var queue = new Queue<Position>();
            queue.Enqueue(startPosition);

            while (queue.Count != 0)
            {
                var currentPosition = queue.Dequeue();
                if (currentPosition.Equals(new Position(3, 3)))
                {
                    paths.Add(currentPosition);
                    continue;
                }

                var hash = input + currentPosition.Path;
                var str = GetMd5Hash(hash);

                var up = new Position(currentPosition.X, currentPosition.Y - 1, Direction.U, currentPosition);
                var down = new Position(currentPosition.X, currentPosition.Y + 1, Direction.D, currentPosition);
                var left = new Position(currentPosition.X - 1, currentPosition.Y, Direction.L, currentPosition);
                var right = new Position(currentPosition.X + 1, currentPosition.Y, Direction.R, currentPosition);

                if (PositionAvailable(up) && CorrectKey(str[0]))
                {
                    queue.Enqueue(up);
                }
                if (PositionAvailable(down) && CorrectKey(str[1]))
                {
                    queue.Enqueue(down);
                }
                if (PositionAvailable(left) && CorrectKey(str[2]))
                {
                    queue.Enqueue(left);
                }
                if (PositionAvailable(right) && CorrectKey(str[3]))
                {
                    queue.Enqueue(right);
                }
            }

            return paths;
        }

        private bool PositionAvailable(Position position)
        {
            return position.X >= 0 && position.X < 4 && position.Y >= 0 && position.Y < 4;
        }

        private bool CorrectKey(char key)
        {
            var correctKeys = new char[] { 'b', 'c', 'd', 'e', 'f' };
            return correctKeys.Contains(key);
        }

        private string GetMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < 2; i++)
            {
                var value = data[i].ToString("x2");
                sBuilder.Append(value);
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }

    public class Position
    {
        public string Path { get; set; }
        public Direction? Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y, Direction? direction = null, Position previousPosition = null)
        {
            X = x;
            Y = y;
            this.Direction = direction;

            if (previousPosition != null && direction != null)
                Path = previousPosition.Path + direction;
        }

        public override string ToString()
        {
            return Path;
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
            var path = this.Path;
            if (path != null) return path.GetHashCode();
            return 0;
        }

        public int GetLength()
        {
            return Path.Length;
        }
    }

    public enum Direction
    {
        U,
        D,
        L,
        R
    }
}
