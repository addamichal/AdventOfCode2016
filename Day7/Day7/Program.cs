using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("input.txt");
            var solution = new Solution();

            int result1 = inputs.Count(w => solution.SupportsTls(w));
            Console.WriteLine(result1);

            int result2 = inputs.Count(w => solution.SupportsSsl(w));
            Console.WriteLine(result2);
        }
    }

    public class Solution
    {
        public bool SupportsTls(string ipAddress)
        {
            var hypernetSequences = GetHypernetSequences(ipAddress).ToArray();
            if (hypernetSequences.Any(ContainsAbba))
                return false;

            var nonHypernetSequences = ipAddress.Split(hypernetSequences, StringSplitOptions.None);
            if (nonHypernetSequences.Any(ContainsAbba))
                return true;

            return false;
        }

        private List<string> GetHypernetSequences(string ipAddress)
        {
            var results = new List<string>();
            var matches = Regex.Matches(ipAddress, @"\[[a-z]+\]");
            foreach (var match in matches)
            {
                var groupCapture = match.ToString();
                results.Add(groupCapture);
            }
            return results;
        }

        public bool SupportsSsl(string input)
        {
            var hypernetSequences = GetHypernetSequences(input).ToArray();
            var babs = hypernetSequences.SelectMany(w => GetAbas(w)).ToList();

            var nonHypernetSequences = input.Split(hypernetSequences, StringSplitOptions.None);
            var abas = nonHypernetSequences.SelectMany(w => GetAbas(w)).ToList();

            return AnyMatch(abas, babs);
        }

        private bool AnyMatch(List<string> abas, List<string> babs)
        {
            foreach (var aba in abas)
            {
                foreach (var bab in babs)
                {
                    if (aba[0] == bab[1] && aba[1] == bab[0])
                        return true;
                }
            }
            return false;
        }

        private bool ContainsAbba(string sequesce)
        {
            if (sequesce.Length < 4)
                return false;
            for (int i = 0; i < sequesce.Length - 3; i++)
            {
                if (sequesce[i] == sequesce[i + 3] && sequesce[i + 1] == sequesce[i + 2] && sequesce[i] != sequesce[i + 1])
                    return true;
            }
            return false;
        }

        private List<string> GetAbas(string sequence)
        {
            var results = new List<string>();
            if (sequence.Length < 3)
                return results;

            for (int i = 0; i < sequence.Length - 2; i++)
            {
                if (sequence[i] == sequence[i + 2] && sequence[i] != sequence[i + 1])
                {
                    string aba = new string(new[] { sequence[i], sequence[i + 1], sequence[i + 2] });
                    results.Add(aba);
                }
            }
            return results;
        }


    }

    public class SolutionTests
    {
        [Test]
        [TestCase("abba[mnop]qrst", true)]
        [TestCase("abcd[bddb]xyyx", false)]
        [TestCase("aaaa[qwer]tyui", false)]
        [TestCase("ioxxoj[asdfgh]zxcvbn", true)]
        [TestCase("rxpusykufgqujfe[rypwoorxdemxffui]cvvcufcqmxoxcphp[witynplrfvquduiot]vcysdcsowcxhphp[gctucefriclxaonpwe]jdprpdvpeumrhokrcjt", true)]
        [TestCase("lpmrkesaqdojkqrp[dcgsayoowgpiwam]arphrffqpcdntlxsza[ogneaqyckrvdcvqxbm]xlogrojsovrzfzjtjbd[qjoiyyatxkwsrvldp]gffgmieinxlfzpiej", true)]
        public void SupportsTlsTest(string input, bool expected)
        {
            var solution = new Solution();
            var actual = solution.SupportsTls(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("aba[bab]xyz", true)]
        [TestCase("xyx[xyx]xyx", false)]
        [TestCase("aaa[kek]eke", true)]
        [TestCase("zazbz[bzb]cdb", true)]
        public void SupportsSsl(string input, bool expected)
        {
            var solution = new Solution();
            var actual = solution.SupportsSsl(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
