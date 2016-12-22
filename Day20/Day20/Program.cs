using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            long to = 4294967295;
            var lines = File.ReadAllLines("input.txt");
            var ranges = lines.Select(Range.Parse).OrderBy(w => w.From).ToList();

            long answer1 = -1;
            var answer2 = 0;
            long number = 0;


            while (number < to)
            {
                var containingRanges = ranges.Where(w => w.InRange(number)).ToList();
                if (containingRanges.Count == 0)
                {
                    if (answer1 == -1)
                        answer1 = number;
                    answer2++;
                    number++;
                }
                else
                {
                    var lastRange = containingRanges.OrderByDescending(w => w.To).First();
                    number = lastRange.To + 1;
                }
            }

            Console.WriteLine(answer1);
            Console.WriteLine(answer2);
        }
    }

    public class Range
    {
        public long From { get; set; }
        public long To { get; set; }

        public Range(long @from, long to)
        {
            From = @from;
            To = to;
        }

        public bool InRange(long number)
        {
            return number >= this.From && number <= To;
        }

        public static Range Parse(string line)
        {
            var parts = line.Split('-');
            return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
        }
    }
}
