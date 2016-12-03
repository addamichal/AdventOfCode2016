using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var result1 = 0;
            var result2 = 0;
            var lines = File.ReadAllLines("input.txt").Select(w => w.Trim());

            List<List<int>> inputs = new List<List<int>>();
            foreach (var line in lines)
            {
                var currentLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(w => int.Parse(w)).ToList();
                inputs.Add(currentLine);
            }

            foreach (var input in inputs)
            {
                if (input[0] + input[1] > input[2] && input[1] + input[2] > input[0] && input[0] + input[2] > input[1])
                    result1++;
            }

            for (int i = 0; i < inputs.Count; i += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (inputs[i][j] + inputs[i + 1][j] > inputs[i + 2][j] &&
                        inputs[i + 1][j] + inputs[i + 2][j] > inputs[i][j] &&
                        inputs[i][j] + inputs[i + 2][j] > inputs[i + 1][j])
                        result2++;
                }
            }

            Console.WriteLine(result1);
            Console.WriteLine(result2);
        }
    }
}
