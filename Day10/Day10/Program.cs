using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            int high = 61;
            int low = 17;

            var instructions = File.ReadAllLines("input.txt");

            var maxBot = GetMaxBotId(instructions);
            var maxOutputId = GetMaxOutputId(instructions);

            List<Output> outputs = new List<Output>();
            for (int i = 0; i <= maxOutputId; i++)
            {
                outputs.Add(new Output(i));
            }

            List<Bot> bots = new List<Bot>();
            for (int i = 0; i <= maxBot; i++)
            {
                bots.Add(new Bot(i));
            }

            //bot 5 gives low to output 19 and high to bot 14
            var valueRegex = @"value (\d+) goes to bot (\d+)";
            var goesToRegex = @"bot (\d+) gives low to ([a-z]+) (\d+) and high to ([a-z]+) (\d+)";
            foreach (var input in instructions)
            {
                var valueMatch = Regex.Match(input, valueRegex);
                if (valueMatch.Success)
                {
                    var bot = int.Parse(valueMatch.Groups[2].Value);
                    var value = int.Parse(valueMatch.Groups[1].Value);
                    bots[bot].AddNumber(value);
                }

                var goesToMatch = Regex.Match(input, goesToRegex);
                if (goesToMatch.Success)
                {
                    int botId = int.Parse(goesToMatch.Groups[1].Value);
                    var bot = bots[botId];
                    string lowGoesTo = goesToMatch.Groups[2].Value;
                    int lowGoesToId = int.Parse(goesToMatch.Groups[3].Value);
                    if (lowGoesTo == "bot")
                        bot.Low = bots[lowGoesToId];
                    else
                        bot.Low = outputs[lowGoesToId];

                    string highGoesTo = goesToMatch.Groups[4].Value;
                    int highGoesToId = int.Parse(goesToMatch.Groups[5].Value);
                    if (highGoesTo == "bot")
                        bot.High = bots[highGoesToId];
                    else
                        bot.High = outputs[highGoesToId];
                }
            }

            int answer = -1;
            while (true)
            {
                var answerBot = bots.SingleOrDefault(w => w.HasNumbers(low, high));
                if (answerBot != null)
                {
                    answer = answerBot.BotNumber;
                }

                var readyBots = bots.Where(bot => bot.IsReady()).ToList();
                if (readyBots.Any() == false)
                    break;

                foreach (var bot in readyBots)
                {
                    bot.ExecuteInstructions();
                }
            }

            Console.WriteLine(answer);
            Console.WriteLine(outputs[0].Number * outputs[1].Number * outputs[2].Number);
        }

        private static int GetMaxOutputId(string[] inputs)
        {
            List<int> botIds = new List<int>();
            var maxBotRegex = @"output (\d+)";
            foreach (var input in inputs)
            {
                var match = Regex.Match(input, maxBotRegex);
                if (match.Success)
                {
                    var bot = int.Parse(match.Groups[1].Value);
                    botIds.Add(bot);
                }
            }
            var maxBot = botIds.Max();
            return maxBot;
        }

        private static int GetMaxBotId(string[] inputs)
        {
            List<int> botIds = new List<int>();
            var maxBotRegex = @"bot (\d+)";
            foreach (var input in inputs)
            {
                var match = Regex.Match(input, maxBotRegex);
                if (match.Success)
                {
                    var bot = int.Parse(match.Groups[1].Value);
                    botIds.Add(bot);
                }
            }
            var maxBot = botIds.Max();
            return maxBot;
        }
    }

    public class Bot : Base
    {
        public int BotNumber { get; set; }
        public List<int> Numbers { get; set; }

        public Base Low { get; set; }
        public Base High { get; set; }

        public Bot(int botNumber)
        {
            this.BotNumber = botNumber;
            Numbers = new List<int>();
        }

        public override void AddNumber(int number)
        {
            Numbers.Add(number);
        }

        protected int GetLow()
        {
            return Math.Min(Numbers[0], Numbers[1]);
        }

        protected int GetHigh()
        {
            return Math.Max(Numbers[0], Numbers[1]);
        }

        public bool IsReady()
        {
            return Numbers.Count == 2;
        }

        public bool HasNumbers(int low, int high)
        {
            return this.Numbers.Contains(low) && this.Numbers.Contains(high);
        }

        public override bool Equals(object obj)
        {
            return this.BotNumber == ((Bot)obj).BotNumber;
        }

        public override int GetHashCode()
        {
            return this.BotNumber;
        }

        protected void Clear()
        {
            this.Numbers.Clear();
        }

        public void ExecuteInstructions()
        {
            this.Low.AddNumber(GetLow());
            this.High.AddNumber(GetHigh());
            this.Clear();
        }

        public override string ToString()
        {
            return $"BotNumber: {BotNumber}";
        }
    }

    public class Output : Base
    {
        public int OutputId { get; set; }
        public int Number { get; set; }

        public Output(int outputId)
        {
            this.OutputId = outputId;
        }

        public override void AddNumber(int number)
        {
            this.Number = number;
        }

        public override string ToString()
        {
            return $"OutputId: {OutputId}";
        }
    }

    public abstract class Base
    {
        public abstract void AddNumber(int number);
    }

    public class Solution
    {

    }
}
