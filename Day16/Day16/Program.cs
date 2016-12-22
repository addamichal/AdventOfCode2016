using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var solution = new Solution();
            var answer1 = solution.Answer(272, "10001001100000001");
            Console.WriteLine(answer1);

            var answer2 = solution.Answer(35651584, "10001001100000001");
            Console.WriteLine(answer2);
        }
    }

    public class Solution
    {
        public string Checksum(string str)
        {
            var result = new List<char>();
            var input = str.ToCharArray().ToList();
            while (true)
            {
                for (int i = 0; i < input.Count; i += 2)
                {
                    var c = input[i] == input[i + 1] ? '1' : '0';
                    result.Add(c);
                }

                if (result.Count % 2 == 1)
                    return new string(result.ToArray());

                input = result;
                result = new List<char>();
            }
        }

        public string Answer(int length, string initialState)
        {
            var input = GetDragonCurve(length, initialState);
            return Checksum(input.Substring(0, length));
        }

        public string GetDragonCurve(int length, string initialState)
        {
            var input = initialState.ToCharArray().ToList();
            var result = new List<char>();

            while (result.Count < length)
            {
                result = new List<char>();

                result.AddRange(input);
                result.Add('0');

                for (int i = input.Count - 1; i >= 0; i--)
                {
                    var c = input[i];
                    result.Add(c == '1' ? '0' : '1');
                }

                input = result;
            }

            return new string(result.ToArray());
        }
    }

    public class SolutionTests
    {
        [Test]
        [TestCase("1", "100")]
        [TestCase("0", "001")]
        [TestCase("11111", "11111000000")]
        [TestCase("111100001010", "1111000010100101011110000")]
        [TestCase("10000", "10000011110010000111110")]
        public void GetDragonCurve_Test(string input, string expected)
        {
            var solution = new Solution();
            var actual = solution.GetDragonCurve(expected.Length, input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Checksum_Test()
        {
            string expected = "100";
            var solution = new Solution();
            var actual = solution.Checksum("110010110100");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("10000", 20, "01100")]
        [TestCase("10001001100000001", 272, "10101001010100001")]
        public void Answer_Test(string input, int length, string expected)
        {
            var solution = new Solution();
            var actual = solution.Answer(length, input);

            Assert.AreEqual(expected, actual);
        }
    }
}
