using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var answer1 = "";
            var answer2 = "";
            var lines = File.ReadAllLines("input.txt");
            for (int i = 0; i < lines[0].Length; i++)
            {
                var mostFrequentCharacter = GetMostFrequentCharacter(i, lines);
                var leastFrequentCharacter = GetLeastFrequentCharacter(i, lines);
                answer1 += mostFrequentCharacter;
                answer2 += leastFrequentCharacter;
            }
            Console.WriteLine(answer1);
            Console.WriteLine(answer2);
        }

        static char GetMostFrequentCharacter(int index, string[] lines)
        {
            return
                lines.Select(w => w.ToCharArray())
                    .Select(w => w.ElementAt(index))
                    .GroupBy(w => w)
                    .Select(w => new { Character = w.Key, Frequency = w.Count() })
                    .OrderByDescending(w => w.Frequency)
                    .FirstOrDefault()
                    .Character;
        }

        static char GetLeastFrequentCharacter(int index, string[] lines)
        {
            return
                lines.Select(w => w.ToCharArray())
                    .Select(w => w.ElementAt(index))
                    .GroupBy(w => w)
                    .Select(w => new { Character = w.Key, Frequency = w.Count() })
                    .OrderBy(w => w.Frequency)
                    .FirstOrDefault()
                    .Character;
        }
    }
}
