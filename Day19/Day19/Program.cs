using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = new Solution();
            var answer = solution.GetAnswer2(3017957);
            Console.WriteLine(answer);
        }
    }

    public class Solution
    {
        public int GetAnswer(int input)
        {
            var elfs = new bool[input];
            for (int i = 0; i < input; i++)
            {
                elfs[i] = true;
            }

            var currentElfIndex = 0;
            while (true)
            {
                if (currentElfIndex == input)
                    currentElfIndex = 0;

                var currentElf = elfs[currentElfIndex];
                if (currentElf)
                {
                    var nextElfIndex = currentElfIndex + 1;
                    while (true)
                    {
                        if (nextElfIndex == input)
                            nextElfIndex = 0;

                        if (nextElfIndex == currentElfIndex)
                        {
                            return currentElfIndex; //we found an answer
                        }

                        if (elfs[nextElfIndex])
                        {
                            //Console.WriteLine("Elf {0} takes Elf {1}'s present.", currentElfIndex, nextElfIndex);
                            elfs[nextElfIndex] = false;
                            break;
                        }

                        nextElfIndex++;
                    }
                }
                else
                {
                    //Console.WriteLine("Elf {0} has no presents and is skipped", currentElfIndex);
                }
                currentElfIndex++;
            }
        }

        public int GetAnswer2(int input)
        {
            int startIndex = 0;
            var elfs = Enumerable.Range(1, input).ToList();

            while (elfs.Count != 1)
            {
                var currentElf = elfs[startIndex];

                //if (elfs.Count % 1000 == 0)
                //    Console.WriteLine(elfs.Count);

                var nextIndex = (startIndex + elfs.Count / 2) % elfs.Count;
                //Console.WriteLine("Elf {0} is taking from Elf {1}", currentElf, elfs[nextIndex]);
                elfs.RemoveAt(nextIndex);

                startIndex = (elfs.IndexOf(currentElf) + 1) % elfs.Count;
            }
            return elfs.SingleOrDefault();
        }
    }
}
