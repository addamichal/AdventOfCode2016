using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = new Solution();
            var line = ".^.^..^......^^^^^...^^^...^...^....^^.^...^.^^^^....^...^^.^^^...^^^^.^^.^.^^..^.^^^..^^^^^^.^^^..^";
            var answer1 = solution.GetAnswer(line, 40);
            Console.WriteLine(answer1);
            var answer2 = solution.GetAnswer(line, 400000);
            Console.WriteLine(answer2);
        }
    }

    public class Solution
    {
        public List<bool> GetNextRow(List<bool> currentRow)
        {
            var result = new List<bool>();

            currentRow.Insert(0, false);
            currentRow.Add(false);

            for (int i = 0; i < currentRow.Count - 2; i++)
            {
                if (IsTrap(currentRow[i], currentRow[i + 1], currentRow[i + 2]))
                    result.Add(true);
                else
                    result.Add(false);
            }

            return result;
        }

        public bool IsTrap(bool left, bool center, bool right)
        {
            if (left && center && !right)
                return true;
            if (center && right && !left)
                return true;
            if (left && !center && !right)
                return true;
            if (right && !center && !left)
                return true;

            return false;
        }

        public string ToString(List<bool> row)
        {
            var result = "";
            foreach (var el in row)
            {
                if (el)
                    result += "^";
                else
                    result += ".";
            }
            return result;
        }

        public List<bool> Parse(string line)
        {
            var result = new List<bool>();
            var inputs = line.ToCharArray();
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == '^')
                    result.Add(true);
                else
                    result.Add(false);
            }

            return result;
        }

        public int GetAnswer(string input, int rows)
        {
            Console.WriteLine(input);
            int safeTiles = 0;
            var row = Parse(input);
            safeTiles += row.Count(w => !w);

            for (int i = 0; i < rows - 1; i++)
            {
                row = GetNextRow(row);
                //Console.WriteLine(ToString(row));
                safeTiles += row.Count(w => !w);
            }

            return safeTiles;
        }
    }
}
