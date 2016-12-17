using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {

        static void Main(string[] args)
        {
            var input = "ngcjuoqr";

            var solution1 = new Solution(false);
            var answer1 = solution1.GetAnswer(input);
            Console.WriteLine(answer1);

            var solution2 = new Solution(true);
            var answer2 = solution2.GetAnswer(input);
            Console.WriteLine(answer2);
        }
    }

    public class Solution
    {
        Dictionary<string, string> cache = new Dictionary<string, string>();
        MD5 md5 = MD5.Create();
        bool useStretched;

        public Solution(bool useStretched)
        {
            this.useStretched = useStretched;
        }

        public int GetAnswer(string salt)
        {
            int keys = 0;
            int index = 0;

            while (true)
            {
                if (IsFullKey(salt, index))
                    keys++;

                if (keys == 64)
                    return index;

                index++;
            }
        }

        private bool IsFullKey(string salt, int index)
        {
            char? isKey = IsThreeKey(salt, index);
            if (isKey != null)
            {
                for (int i = 1; i <= 1000; i++)
                {
                    var nextIndex = index + i;
                    if (IsFiveKey(salt, nextIndex, isKey.Value) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public char? IsThreeKey(string salt, int index)
        {
            var input = salt + index;
            var md5Hash = GetMd5Hash(input);
            var result = ContainsThreeInRow(md5Hash);
            return result;
        }

        public char? IsFiveKey(string salt, int index, char c)
        {
            var input = salt + index;
            var md5Hash = GetMd5Hash(input);
            var result = ContainsFiveInRow(md5Hash, c);
            return result;
        }

        private string GetMd5Hash(string input)
        {
            if (cache.ContainsKey(input))
                return cache[input];

            string md5Hash = useStretched ? GetStretchedHash(md5, input) : GetMd5Hash(md5, input);
            cache.Add(input, md5Hash);

            return md5Hash;
        }

        private char? ContainsThreeInRow(string input)
        {
            for (int i = 0; i < input.Length - 2; i++)
            {
                if (input[i + 0] == input[i + 1] && input[i + 1] == input[i + 2])
                    return input[i];
            }
            return null;
        }

        private char? ContainsFiveInRow(string input, char character)
        {
            for (int i = 0; i < input.Length - 4; i++)
            {
                if (input[i] == character &&
                    input[i] == input[i + 1] &&
                    input[i + 1] == input[i + 2] &&
                    input[i + 2] == input[i + 3] &&
                    input[i + 3] == input[i + 4])
                {
                    return character;
                }
            }
            return null;
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] data = md5Hash.ComputeHash(inputBytes);
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        private string GetStretchedHash(MD5 md5Hash, string input)
        {
            for (int i = 0; i < 2017; i++)
                input = GetMd5Hash(md5Hash, input);

            return input;
        }
    }
}
