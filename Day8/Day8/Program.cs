using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var solution = new Solution(50, 6);
            foreach (var line in lines)
            {
                solution.ProcessInput(line);
            }

            Console.WriteLine(solution.GetLitCount());
            Console.WriteLine(solution);
        }
    }

    public class Solution
    {
        private readonly int _rows;
        private readonly int _columns;
        private int[,] _board;

        public Solution(int columns, int rows)
        {
            _rows = rows;
            _columns = columns;

            _board = new int[rows, columns];
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    if (_board[row, column] == 1)
                        stringBuilder.Append('#');
                    else
                        stringBuilder.Append('.');
                }
                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        public void ProcessInput(string input)
        {
            var command = BaseCommand.Parse(input);
            if (command is RectCommand)
            {
                var rectCommand = (RectCommand)command;
                for (int row = 0; row < rectCommand.Rows; row++)
                {
                    for (int column = 0; column < rectCommand.Columns; column++)
                    {
                        _board[row, column] = 1;
                    }
                }
            }
            if (command is RotateColumnCommand)
            {
                var rotateColumnCommand = (RotateColumnCommand)command;
                int column = rotateColumnCommand.X;
                for (int currentBy = 0; currentBy < rotateColumnCommand.By; currentBy++)
                {
                    var lastRow = _board[_rows - 1, column];
                    for (int i = _rows - 1; i >= 1; i--)
                    {
                        _board[i, column] = _board[i - 1, column];
                    }
                    _board[0, column] = lastRow;
                }
            }
            if (command is RotateRowCommand)
            {
                var rotateRowCommand = (RotateRowCommand)command;
                int row = rotateRowCommand.Y;
                for (int currentBy = 0; currentBy < rotateRowCommand.By; currentBy++)
                {
                    var lastColumn = _board[row, _columns - 1];
                    for (int i = _columns - 1; i >= 1; i--)
                    {
                        _board[row, i] = _board[row, i - 1];
                    }
                    _board[row, 0] = lastColumn;
                }
            }
        }

        public int GetLitCount()
        {
            int counter = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_board[i, j] == 1)
                        counter++;
                }
            }
            return counter;
        }
    }

    public class BaseCommand
    {
        public static BaseCommand Parse(string input)
        {
            if (input.StartsWith("rect"))
            {
                input = input.Replace("rect ", "");
                var strings = input.Split('x');
                return new RectCommand() { Columns = int.Parse(strings[0]), Rows = int.Parse(strings[1]) };
            }
            if (input.StartsWith("rotate column x="))
            {
                input = input.Replace("rotate column x=", "");
                var strings = input.Split(new[] { " by " }, StringSplitOptions.None);
                return new RotateColumnCommand() { X = int.Parse(strings[0]), By = int.Parse(strings[1]) };
            }
            if (input.StartsWith("rotate row y="))
            {
                input = input.Replace("rotate row y=", "");
                var strings = input.Split(new[] { " by " }, StringSplitOptions.None);
                return new RotateRowCommand() { Y = int.Parse(strings[0]), By = int.Parse(strings[1]) };
            }

            throw new Exception("Unknown command: " + input);
        }
    }

    public class RectCommand : BaseCommand
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
    }

    public class RotateColumnCommand : BaseCommand
    {
        public int X { get; set; }
        public int By { get; set; }
    }

    public class RotateRowCommand : BaseCommand
    {
        public int Y { get; set; }
        public int By { get; set; }
    }

    public class SolutionTests
    {
        [Test]
        public void Test_step_0()
        {
            var expected =
@".......
.......
.......
";

            var solution = new Solution(7, 3);

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void Test_step_1()
        {
            var expected =
@"###....
###....
.......
";

            var solution = new Solution(7, 3);
            solution.ProcessInput("rect 3x2");

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void Test_step_2()
        {
            var expected =
@"#.#....
###....
.#.....
";

            var solution = new Solution(7, 3);
            solution.ProcessInput("rect 3x2");
            solution.ProcessInput("rotate column x=1 by 1");

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void Test_step_3()
        {
            var expected =
@"....#.#
###....
.#.....
";

            var solution = new Solution(7, 3);
            solution.ProcessInput("rect 3x2");
            solution.ProcessInput("rotate column x=1 by 1");
            solution.ProcessInput("rotate row y=0 by 4");

            Assert.AreEqual(expected, solution.ToString());
        }

        [Test]
        public void Test_step_4()
        {
            var expected =
@".#..#.#
#.#....
.#.....
";

            var solution = new Solution(7, 3);
            solution.ProcessInput("rect 3x2");
            solution.ProcessInput("rotate column x=1 by 1");
            solution.ProcessInput("rotate row y=0 by 4");
            solution.ProcessInput("rotate column x=1 by 1");

            Assert.AreEqual(expected, solution.ToString());
        }
    }
}
