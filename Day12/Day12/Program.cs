using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt");

            var registers1 = new Dictionary<string, int>();
            registers1.Add("a", 0);
            registers1.Add("b", 0);
            registers1.Add("c", 0);
            registers1.Add("d", 0);

            var answer1 = Run(instructions, registers1);
            Console.WriteLine(answer1);

            var registers2 = new Dictionary<string, int>();
            registers2.Add("a", 0);
            registers2.Add("b", 0);
            registers2.Add("c", 1);
            registers2.Add("d", 0);

            var answer2 = Run(instructions, registers2);
            Console.WriteLine(answer2);
        }

        private static int Run(string[] instructions, Dictionary<string, int> registers)
        {
            var currentInstructionId = 0;
            while (true)
            {
                if (currentInstructionId >= instructions.Length)
                    break;

                var instruction = instructions[currentInstructionId];

                if (instruction.StartsWith("cpy"))
                {
                    var match = Regex.Match(instruction, @"cpy (-?\d*[a-d]?) ([a-d])");
                    var register = match.Groups[2].Value;

                    var strValue = match.Groups[1].Value;

                    int value;
                    if (int.TryParse(strValue, out value))
                        registers[register] = value;
                    else
                        registers[register] = registers[strValue];
                }
                else if (instruction.StartsWith("inc"))
                {
                    var register = instruction.Replace("inc ", "");
                    registers[register]++;
                }
                else if (instruction.StartsWith("dec"))
                {
                    var register = instruction.Replace("dec ", "");
                    registers[register]--;
                }
                else if (instruction.StartsWith("jnz"))
                {
                    var match = Regex.Match(instruction, @"jnz ([a-d]*\d*) (-?\d+)");
                    var xStr = match.Groups[1].Value;
                    int x;
                    var value = int.Parse(match.Groups[2].Value);

                    if (!int.TryParse(xStr, out x))
                        x = registers[xStr];

                    if (x != 0)
                    {
                        currentInstructionId += value;
                        continue;
                    }
                }

                currentInstructionId++;
            }

            return registers["a"];
        }
    }
}
