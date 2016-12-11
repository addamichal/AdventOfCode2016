using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            //var firstStep = new Solution();
            //firstStep.Floors[1].Add(new Item("H", false));
            //firstStep.Floors[1].Add(new Item("L", false));
            //firstStep.Floors[2].Add(new Item("H", true));
            //firstStep.Floors[3].Add(new Item("L", true));

            var firstStep = new Solution();
            firstStep.Floors[1].Add(new Item("PO", true));
            firstStep.Floors[1].Add(new Item("TM", true));
            firstStep.Floors[1].Add(new Item("TM", false));
            firstStep.Floors[1].Add(new Item("PM", true));
            firstStep.Floors[1].Add(new Item("RU", true));
            firstStep.Floors[1].Add(new Item("RU", false));
            firstStep.Floors[1].Add(new Item("CO", true));
            firstStep.Floors[1].Add(new Item("CO", false));

            firstStep.Floors[2].Add(new Item("PO", false));
            firstStep.Floors[2].Add(new Item("PM", false));

            GetAnswer(firstStep);

            //b)
            firstStep.Floors[1].Add(new Item("EL", true));
            firstStep.Floors[1].Add(new Item("EL", false));
            firstStep.Floors[1].Add(new Item("DI", true));
            firstStep.Floors[1].Add(new Item("DI", false));

            Solution.DoneSolutions = new HashSet<Solution>();
            GetAnswer(firstStep);
        }

        private static void GetAnswer(Solution firstStep)
        {
            var currentQueue = new List<Solution>();
            currentQueue.Add(firstStep);

            while (currentQueue.Count != 0)
            {
                Console.WriteLine("DEBUG: {0}", currentQueue.Count);
                var nextSolutions = new List<Solution>();
                foreach (var solution in currentQueue)
                {
                    //Console.WriteLine(solution.Step);
                    //Console.WriteLine(solution);

                    if (solution.IsFinal())
                    {
                        Console.WriteLine(solution.Step);
                        return;
                    }

                    var nextSteps = solution.ExecuteStep();
                    nextSolutions.AddRange(nextSteps);
                }

                currentQueue = nextSolutions;
            }
        }
    }

    public class Solution : ICloneable
    {
        public static HashSet<Solution> DoneSolutions = new HashSet<Solution>();

        public int Step { get; set; }

        public int ElevatorPosition { get; set; }
        public Dictionary<int, List<Item>> Floors { get; set; }

        public Solution()
        {
            Step = 0;
            ElevatorPosition = 1;
            Floors = new Dictionary<int, List<Item>>();
            for (int i = 1; i <= 4; i++)
            {
                Floors.Add(i, new List<Item>());
            }
        }

        public List<Solution> ExecuteStep()
        {
            var solutions = new List<Solution>();

            if (ElevatorPosition != 4)
            {
                var validUpCombinations = GetValidCombinations(Floors[ElevatorPosition], Floors[ElevatorPosition + 1]).ToList();
                foreach (var validCombination in validUpCombinations)
                {
                    Solution newSolution = (Solution)this.Clone();

                    foreach (var item in validCombination)
                        newSolution.Floors[ElevatorPosition].Remove(item);

                    newSolution.Floors[ElevatorPosition + 1].AddRange(validCombination);
                    newSolution.ElevatorPosition++;
                    newSolution.Step++;

                    if (!DoneSolutions.Contains(newSolution))
                    {
                        solutions.Add(newSolution);
                        DoneSolutions.Add(newSolution);
                    }
                }
            }

            if (ElevatorPosition != 1)
            {
                var validUpCombinations = GetValidCombinations(Floors[ElevatorPosition], Floors[ElevatorPosition - 1]).ToList();
                foreach (var validCombination in validUpCombinations)
                {
                    Solution newSolution = (Solution)this.Clone();

                    foreach (var item in validCombination)
                        newSolution.Floors[ElevatorPosition].Remove(item);

                    newSolution.Floors[ElevatorPosition - 1].AddRange(validCombination);
                    newSolution.ElevatorPosition--;
                    newSolution.Step++;

                    if (!DoneSolutions.Contains(newSolution))
                    {
                        solutions.Add(newSolution);
                        DoneSolutions.Add(newSolution);
                    }
                }
            }

            return solutions;
        }

        private List<List<Item>> GetValidCombinations(List<Item> currentFloor, List<Item> upperFloor)
        {
            var validCombinations = new List<List<Item>>();
            var possibleCombinations = GetValidElevatorCombinations(currentFloor);
            foreach (var possibleCombination in possibleCombinations)
            {
                var newFloor = new List<Item>();
                newFloor.AddRange(upperFloor);
                newFloor.AddRange(possibleCombination);

                var oldFloor = new List<Item>();
                oldFloor.AddRange(currentFloor);
                foreach (var item in possibleCombination)
                {
                    oldFloor.Remove(item);
                }

                if (IsValid(newFloor) && IsValid(oldFloor))
                {
                    validCombinations.Add(possibleCombination);
                }
            }
            return validCombinations;
        }

        public bool IsValid(List<Item> items)
        {
            var anyGenerators = items.Any(w => w.IsGenerator);
            if (!anyGenerators)
                return true;

            var chips = items.Where(w => !w.IsGenerator);
            foreach (var chip in chips)
            {
                var generator = items.SingleOrDefault(w => w.Element == chip.Element && w.IsGenerator);
                if (generator == null)
                    return false;
            }
            return true;
        }

        public List<List<Item>> GetValidElevatorCombinations(List<Item> items)
        {
            var result = new List<List<Item>>();
            for (int i = 0; i < items.Count; i++)
            {
                var currentItem = items[i];
                result.Add(new List<Item>() { currentItem });
                for (int j = i + 1; j < items.Count; j++)
                {
                    var otherItem = items[j];

                    var combination = new List<Item>() { currentItem, otherItem };
                    if (IsValid(combination))
                        result.Add(combination);
                }
            }
            return result;
        }

        public bool IsFinal()
        {
            for (int i = 1; i <= 3; i++)
            {
                if (this.Floors[i].Count != 0)
                    return false;
            }
            return true;
        }

        public object Clone()
        {
            var newSolution = new Solution();
            newSolution.Step = this.Step;
            newSolution.ElevatorPosition = this.ElevatorPosition;

            foreach (var floor in Floors)
            {
                foreach (var item in floor.Value)
                {
                    newSolution.Floors[floor.Key].Add(item);
                }
            }
            return newSolution;
        }

        public override string ToString()
        {
            var result = "";
            foreach (var floor in Floors.Reverse())
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("F" + floor.Key + " ");
                if (ElevatorPosition == floor.Key)
                    stringBuilder.Append("E ");
                else
                    stringBuilder.Append(". ");

                foreach (var item in floor.Value.Select(w => w.ToString()).OrderBy(w => w))
                {
                    stringBuilder.Append(item + " ");
                }
                result += stringBuilder + Environment.NewLine;
            }

            return result;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherSolution = (Solution)obj;
            return this.ToString() == otherSolution.ToString();
        }
    }

    public class Item
    {
        public string Element { get; set; }
        public bool IsGenerator { get; set; }

        public Item(string element, bool isGenerator)
        {
            Element = element;
            IsGenerator = isGenerator;
        }

        public override bool Equals(object obj)
        {
            var otherItem = (Item)obj;
            return this.IsGenerator == otherItem.IsGenerator && this.Element == otherItem.Element;
        }

        public override string ToString()
        {
            return Element + (IsGenerator ? "G" : "M");
        }
    }

    public class SolutionTest
    {
        [Test]
        public void IsValid_Test1()
        {
            var expected = true;
            var input = new List<Item>()
            {
                new Item("H", false),
                new Item("L", false)
            };

            var solution = new Solution();

            var actual = solution.IsValid(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_Test2()
        {
            var expected = true;
            var input = new List<Item>()
            {
                new Item("H", false),
                new Item("H", true)
            };

            var solution = new Solution();

            var actual = solution.IsValid(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_Test3()
        {
            var expected = false;
            var input = new List<Item>()
            {
                new Item("H", false),
                new Item("L", true)
            };

            var solution = new Solution();

            var actual = solution.IsValid(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_Test4()
        {
            var expected = true;
            var input = new List<Item>()
            {
                new Item("H", false),
                new Item("H", true),
                new Item("L", false),
                new Item("L", true)
            };

            var solution = new Solution();

            var actual = solution.IsValid(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_Test5()
        {
            var expected = false;
            var input = new List<Item>()
            {
                new Item("H", false),
                new Item("L", true),
            };

            var solution = new Solution();

            var actual = solution.IsValid(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetValidElevatorCombinations_Test1()
        {
            var expected = new List<List<Item>>()
            {
                new List<Item>() { new Item("H", false) },
                new List<Item>() { new Item("H", true) },
                new List<Item>() { new Item("H", false), new Item("H", true) },
                new List<Item>() { new Item("L", false) },
                new List<Item>() { new Item("L", true) },
                new List<Item>() { new Item("H", false), new Item("L", false) },
                new List<Item>() { new Item("H", false), new Item("L", false) },
                new List<Item>() { new Item("H", true), new Item("L", true) }
            };

            var input = new List<Item>()
            {
                new Item("H", false),
                new Item("H", true),
                new Item("L", false),
                new Item("L", true)
            };

            var solution = new Solution();
            var actual = solution.GetValidElevatorCombinations(input);

            Assert.AreEqual(expected.Count, actual.Count);
        }
    }
}
