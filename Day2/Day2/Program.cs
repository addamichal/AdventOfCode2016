using System;
using System.IO;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Answer(
                new[,] {
                { '1', '2', '3' },
                { '4', '5', '6' },
                { '7', '8', '9' }
            }, 0, 0);

            Answer(
                new[,] {
                { '*', '*', '1', '*', '*' },
                { '*', '2', '3', '4', '*' },
                { '5', '6', '7', '8', '9' },
                { '*', 'A', 'B', 'C', '*' },
                { '*', '*', 'D', '*', '*' }
            }, 2, 0
            );
        }

        private static void Answer(char[,] keypad, int startRow, int startColumn)
        {
            var instructions = File.ReadAllLines("input.txt");

            string key = "";
            var column = startColumn;
            var row = startRow;

            var upperBound = keypad.GetLength(0) - 1;
            foreach (var instruction in instructions)
            {
                var directions = instruction.ToCharArray();
                foreach (var direction in directions)
                {
                    if (direction == 'U')
                    {
                        if (row != 0 && keypad[row - 1, column] != '*')
                            row--;
                    }
                    if (direction == 'D')
                    {
                        if (row != upperBound && keypad[row + 1, column] != '*')
                            row++;
                    }
                    if (direction == 'L')
                    {
                        if (column != 0 && keypad[row, column - 1] != '*')
                            column--;
                    }
                    if (direction == 'R')
                    {
                        if (column != upperBound && keypad[row, column + 1] != '*')
                            column++;
                    }
                }

                key += keypad[row, column];
            }

            Console.WriteLine(key);
        }
    }
}
