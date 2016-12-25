using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt").ToList();
            var solution = new Solution(instructions);

            var input = 1;
            while (true)
            {
                var result = solution.Run(input);
                if (result)
                    break;

                input++;
            }
            Console.WriteLine(input);

        }
    }

    public class Solution
    {
        public List<int> Output = new List<int>();

        public List<string> instructions;
        public Dictionary<string, int> Registers;
        public int CurrentInstructionId;

        public Solution(List<string> instructions)
        {
            this.instructions = instructions;

            Registers = new Dictionary<string, int>();
            Registers.Add("a", 0);
            Registers.Add("b", 0);
            Registers.Add("c", 0);
            Registers.Add("d", 0);
        }

        public bool Run(int start)
        {
            CurrentInstructionId = 0;
            Output = new List<int>();

            Registers["a"] = start;


            while (true)
            {
                if (Output.Count == 8)
                    return IsOutputValid(Output);

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
                else if (instruction.StartsWith("out"))
                {
                    //we can improve this one...
                    string outInstruction = instruction;
                    var prematch = Regex.Match(instruction, @"([a-d])");
                    if (prematch.Success)
                    {
                        var register = prematch.Groups[1].Value;
                        var registerValue = Registers[register].ToString();
                        outInstruction = outInstruction.Replace(register, registerValue);
                    }
                    var match = Regex.Match(outInstruction, @"out (-?\d)*");
                    var value = int.Parse(match.Groups[1].Value);
                    Output.Add(value);
                }

                CurrentInstructionId++;
            }

            return false;
        }

        public bool IsOutputValid(List<int> output)
        {
            Console.WriteLine(string.Join(", ", output));
            for (int i = 0; i < output.Count - 3; i += 2)
            {
                if (!(output[i] == output[i + 2] && output[i + 1] == output[i + 3] && output[i] != output[i + 1] && output[i] == 0 && output[i + 1] == 1))
                    return false;
            }
            return true;
        }
    }

    public class SolutionTest
    {
        [Test]
        public void IsValid_Test()
        {
            var input = new List<int> { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 };
            var solution = new Solution(new List<string>());
            var actual = solution.IsOutputValid(input);
            Assert.IsTrue(actual);
        }
    }
}
