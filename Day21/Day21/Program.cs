using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            //var instructionSet = "testinput.txt";
            var instructionSet = "input.txt";

            //string input1 = "abcde";
            //string input2 = "decab";
            string input1 = "abcdefgh";
            string input2 = "fbgdceah";

            var instructions = File.ReadLines(instructionSet).ToList();

            var solution = new Solution();
            var answer1 = solution.Encrypt(input1, instructions);
            var answer2 = solution.Decrypt(input2, instructions);

            Console.WriteLine(answer1);
            Console.WriteLine(answer2);
        }
    }

    public class Solution
    {
        public string Encrypt(string input, List<string> instructions)
        {
            var solution = new Solution();
            foreach (var line in instructions)
            {
                input = solution.ExecuteInstruction(line, input);
            }
            return input;
        }

        public string Decrypt(string input, List<string> instructions)
        {
            var solution = new Solution();
            instructions.Reverse();
            int step = 0;
            foreach (var line in instructions)
            {
                input = solution.ExecuteInstruction(line, input, true);
                step++;
            }
            return input;
        }

        public string ExecuteInstruction(string instruction, string input, bool decrypt = false)
        {
            var inputChars = input.ToCharArray();

            var regexSwapPosition = Regex.Match(instruction, @"swap position (\d*) with position (\d*)");
            var regexSwapLetter = Regex.Match(instruction, @"swap letter (\w) with letter (\w)");
            var regexReversePositions = Regex.Match(instruction, @"reverse positions (\d*) through (\d*)");
            var regexRotateLeft = Regex.Match(instruction, @"rotate left (\d*) step");
            var regexRotateRight = Regex.Match(instruction, @"rotate right (\d*) step");
            var regexMovePosition = Regex.Match(instruction, @"move position (\d*) to position (\d*)");
            var regexRotateBased = Regex.Match(instruction, @"rotate based on position of letter (\w)");
            if (regexSwapPosition.Success)
            {
                var swapPosition = int.Parse(regexSwapPosition.Groups[1].Value);
                var withPosition = int.Parse(regexSwapPosition.Groups[2].Value);

                if (!decrypt)
                    SwapPosition(inputChars, swapPosition, withPosition);
                else
                    SwapPosition(inputChars, withPosition, swapPosition);
            }
            if (regexSwapLetter.Success)
            {
                var swapPosition = regexSwapLetter.Groups[1].Value[0];
                var withPosition = regexSwapLetter.Groups[2].Value[0];

                if (!decrypt)
                    SwapLetter(inputChars, swapPosition, withPosition);
                else
                    SwapLetter(inputChars, withPosition, swapPosition);
            }
            if (regexReversePositions.Success)
            {
                var fromIndex = int.Parse(regexReversePositions.Groups[1].Value);
                var toIndex = int.Parse(regexReversePositions.Groups[2].Value);

                //the same?
                Array.Reverse(inputChars, fromIndex, (toIndex - fromIndex) + 1);
            }
            if (regexRotateLeft.Success)
            {
                var times = int.Parse(regexRotateLeft.Groups[1].Value);
                if (!decrypt)
                    RotateLeft(times, inputChars);
                else
                    RotateRight(times, inputChars);
            }
            if (regexRotateRight.Success)
            {
                var times = int.Parse(regexRotateRight.Groups[1].Value);
                if (!decrypt)
                    RotateRight(times, inputChars);
                else
                    RotateLeft(times, inputChars);
            }
            if (regexMovePosition.Success)
            {
                int from = int.Parse(regexMovePosition.Groups[1].Value);
                int to = int.Parse(regexMovePosition.Groups[2].Value);

                if (!decrypt)
                    inputChars = MovePosition(inputChars, from, to);
                else
                    inputChars = MovePosition(inputChars, to, from);
            }
            if (regexRotateBased.Success)
            {
                var letter = regexRotateBased.Groups[1].Value[0];

                if (!decrypt)
                {
                    DecryptRotateBased(inputChars, letter);
                }
                else
                {
                    var decryptTimes = GetTimes(inputChars, letter);
                    RotateLeft(decryptTimes, inputChars);
                }
            }

            return new string(inputChars);
        }

        private static void DecryptRotateBased(char[] inputChars, char letter)
        {
            var lst = inputChars.ToList();
            var indexOf = lst.IndexOf(letter);
            var encryptTimes = 1 + indexOf + (indexOf >= 4 ? 1 : 0);
            RotateRight(encryptTimes, inputChars);
        }

        private static int GetTimes(char[] inputChars, char letter)
        {
            int times = 1;
            while (true)
            {
                var arrayCpy = inputChars.ToList().ToArray();
                RotateLeft(times, arrayCpy);

                DecryptRotateBased(arrayCpy, letter);

                var result2 = new string(arrayCpy);
                if (result2 == new string(inputChars))
                    return times;

                times++;
            }
        }

        private static char[] MovePosition(char[] inputChars, int @from, int to)
        {
            var lst = inputChars.ToList();
            var item = lst.ElementAt(@from);
            lst.RemoveAt(@from);
            lst.Insert(to, item);
            inputChars = lst.ToArray();
            return inputChars;
        }

        private static void RotateLeft(int times, char[] inputChars)
        {
            for (int time = 0; time < times; time++)
            {
                var tmp = inputChars[0];
                for (int i = 0; i < inputChars.Length - 1; i++)
                {
                    inputChars[i] = inputChars[i + 1];
                }
                inputChars[inputChars.Length - 1] = tmp;
            }
        }

        private static void SwapLetter(char[] inputChars, char swapPosition, char withPosition)
        {
            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] == swapPosition)
                    inputChars[i] = '@';
                if (inputChars[i] == withPosition)
                    inputChars[i] = swapPosition;
            }

            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] == '@')
                    inputChars[i] = withPosition;
            }
        }

        private static void SwapPosition(char[] inputChars, int swapPosition, int withPosition)
        {
            var tmp = inputChars[swapPosition];
            inputChars[swapPosition] = inputChars[withPosition];
            inputChars[withPosition] = tmp;
        }

        private static void RotateRight(int times, char[] inputChars)
        {
            for (int time = 0; time < times; time++)
            {
                var tmp = inputChars[inputChars.Length - 1];
                for (int i = inputChars.Length - 1; i >= 1; i--)
                {
                    inputChars[i] = inputChars[i - 1];
                }
                inputChars[0] = tmp;
            }
        }
    }

    public class SolutionTests
    {
        [Test]
        public void Test1()
        {
            string expected = "ebcda";
            string input = "abcde";
            string instruction = "swap position 4 with position 0";

            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test2()
        {
            string expected = "ebcda";
            string input = "edcba";
            string instruction = "swap letter d with letter b";

            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test3()
        {
            string expected = "abcde";
            string input = "edcba";
            string instruction = "reverse positions 0 through 4";

            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test4()
        {
            string expected = "bcdea";
            string input = "abcde";
            string instruction = "rotate left 1 step";

            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test5()
        {
            string expected = "eabcd";
            string input = "abcde";
            string instruction = "rotate right 1 step";

            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("bcdea", "move position 1 to position 4", "bdeac")]
        [TestCase("bdeac", "move position 3 to position 0", "abdec")]
        public void Test6(string input, string instruction, string expected)
        {
            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("abdec", "rotate based on position of letter b", "ecabd")]
        [TestCase("ecabd", "rotate based on position of letter d", "decab")]
        public void Test7(string input, string instruction, string expected)
        {
            var solution = new Solution();
            var actual = solution.ExecuteInstruction(instruction, input);

            Assert.AreEqual(expected, actual);
        }
    }
}
