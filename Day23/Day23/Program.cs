using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt").ToList();
            var solution = new Solution(instructions);
            solution.Registers["a"] = 7;
            var result = solution.Run();
            Console.WriteLine(result);

            solution.Registers["a"] = Factor(12);
            solution.Registers["b"] = 1;
            solution.Registers["c"] = 1;
            solution.Registers["d"] = 0;

            solution.CurrentInstructionId = 19;
            var result2 = solution.Run();
            Console.WriteLine(result2);
        }

        public static int Factor(int x)
        {
            var result = 1;
            while (x != 1)
            {
                result *= x;
                x--;
            }
            return result;
        }
    }

    public class Solution
    {
        public List<string> instructions;
        public Dictionary<string, int> Registers;
        public int CurrentInstructionId;

        public Solution(List<string> instructions)
        {
            CurrentInstructionId = 0;
            this.instructions = instructions;

            Registers = new Dictionary<string, int>();
            Registers.Add("a", 7);
            Registers.Add("b", 0);
            Registers.Add("c", 0);
            Registers.Add("d", 0);
        }

        public int Run()
        {
            while (true)
            {
                if (CurrentInstructionId >= instructions.Count)
                    break;

                var instruction = instructions[CurrentInstructionId];
                if (instruction.StartsWith("cpy"))
                {
                    var match = Regex.Match(instruction, @"cpy (-?\d*[a-d]?) ([a-d])");
                    var register = match.Groups[2].Value;

                    var strValue = match.Groups[1].Value;

                    int value;
                    if (int.TryParse(strValue, out value))
                        Registers[register] = value;
                    else
                        Registers[register] = Registers[strValue];
                }
                else if (instruction.StartsWith("inc"))
                {
                    var register = instruction.Replace("inc ", "");
                    Registers[register]++;
                }
                else if (instruction.StartsWith("dec"))
                {
                    var register = instruction.Replace("dec ", "");
                    Registers[register]--;
                }
                else if (instruction.StartsWith("jnz"))
                {
                    //we can improve this one...
                    string jnzInstruction = instruction;
                    var prematch = Regex.Match(instruction, @"([a-d])");
                    if (prematch.Success)
                    {
                        var register = prematch.Groups[1].Value;
                        var registerValue = Registers[register].ToString();
                        jnzInstruction = jnzInstruction.Replace(register, registerValue);
                    }

                    var match = Regex.Match(jnzInstruction, @"jnz ([a-d]*-?\d*) (-?\d+)");
                    var xStr = match.Groups[1].Value;
                    int x;
                    var value = int.Parse(match.Groups[2].Value);

                    if (!int.TryParse(xStr, out x))
                        x = Registers[xStr];

                    if (x != 0)
                    {
                        CurrentInstructionId += value;
                        continue;
                    }
                }
                else if (instruction.StartsWith("tgl"))
                {
                    var match = Regex.Match(instruction, @"tgl ([a-d])");
                    var register = match.Groups[1].Value;

                    var value = Registers[register];
                    var toggledInstructionId = CurrentInstructionId + value;
                    if (toggledInstructionId < instructions.Count)
                    {
                        var toggledInstruction = instructions[toggledInstructionId];

                        string newInstruction = null;
                        if (toggledInstruction.StartsWith("inc"))
                            newInstruction = toggledInstruction.Replace("inc", "dec");
                        else if (toggledInstruction.StartsWith("dec"))
                            newInstruction = toggledInstruction.Replace("dec", "inc");
                        else if (toggledInstruction.StartsWith("tgl"))
                            newInstruction = toggledInstruction.Replace("tgl", "inc");
                        else if (toggledInstruction.StartsWith("jnz"))
                            newInstruction = toggledInstruction.Replace("jnz", "cpy");
                        else if (toggledInstruction.StartsWith("cpy"))
                            newInstruction = toggledInstruction.Replace("cpy", "jnz");
                        else
                            throw new Exception("Unknown toggle" + toggledInstruction);

                        instructions[toggledInstructionId] = newInstruction;
                    }
                }

                CurrentInstructionId++;
            }

            return Registers["a"];
        }

    }
}
