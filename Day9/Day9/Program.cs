using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");
            var solution = new Solution();
            var result = solution.Solve(input);
            Console.WriteLine(result.Length);

            var result2 = solution.Solve2(input);
            Console.WriteLine(result2);
        }
    }

    public class Solution
    {
        public string Solve(string input)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    var indexOfClosingBracket = input.IndexOf(')', i);
                    var xyString = input.Substring(i + 1, indexOfClosingBracket - i - 1);
                    var xyParts = xyString.Split('x');
                    int x = int.Parse(xyParts[0]);
                    int y = int.Parse(xyParts[1]);

                    var str = input.Substring(indexOfClosingBracket + 1, x);
                    for (int j = 0; j < y; j++)
                        stringBuilder.Append(str);

                    i = indexOfClosingBracket + str.Length;
                }
                else
                    stringBuilder.Append(input[i]);
            }
            return stringBuilder.ToString();
        }

        public long Solve2(string input)
        {
            long result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    var indexOfClosingBracket = input.IndexOf(')', i);
                    var xyString = input.Substring(i + 1, indexOfClosingBracket - i - 1);
                    var xyParts = xyString.Split('x');
                    int x = int.Parse(xyParts[0]);
                    int y = int.Parse(xyParts[1]);

                    var decodedStringBuilder = new StringBuilder();
                    var str = input.Substring(indexOfClosingBracket + 1, x);
                    for (int j = 0; j < y; j++)
                        decodedStringBuilder.Append(str);

                    var decodedString = decodedStringBuilder.ToString();
                    var currentResult = Solve2(decodedString);
                    result += currentResult;

                    i = indexOfClosingBracket + str.Length;
                }
                else
                    result++;
            }
            return result;
        }
    }

    public class SolutionTests
    {
        [Test]
        [TestCase("ADVENT", "ADVENT")]
        [TestCase("A(1x5)BC", "ABBBBBC")]
        [TestCase("(3x3)XYZ", "XYZXYZXYZ")]
        [TestCase("A(2x2)BCD(2x2)EFG", "ABCBCDEFEFG")]
        [TestCase("(6x1)(1x3)A", "(1x3)A")]
        [TestCase("X(8x2)(3x3)ABCY", "X(3x3)ABC(3x3)ABCY")]
        public void Test1(string input, string expected)
        {
            var solution = new Solution();
            var actual = solution.Solve(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("ADVENT", 6)]
        [TestCase("(3x3)XYZ", 9)]
        [TestCase("X(8x2)(3x3)ABCY", 20)]
        [TestCase("(27x12)(20x12)(13x14)(7x10)(1x12)A", 241920)]
        public void Test2(string input, int expected)
        {
            var solution = new Solution();
            var actual = solution.Solve2(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
