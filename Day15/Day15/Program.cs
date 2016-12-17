using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var discs = new List<Disc>();

            //a)
            discs.Add(new Disc(17, 15));
            discs.Add(new Disc(3, 2));
            discs.Add(new Disc(19, 4));
            discs.Add(new Disc(13, 2));
            discs.Add(new Disc(7, 2));
            discs.Add(new Disc(5, 0));

            GetAnswer(discs);
            
            //b)
            discs.Add(new Disc(11, 0));

            GetAnswer(discs);
        }

        private static void GetAnswer(List<Disc> discs)
        {
            var time = 0;
            while (true)
            {
                var result1 = AreDiscsAligned(discs, time);
                if (result1)
                {
                    Console.WriteLine(time);
                    break;
                }
                time++;
            }
        }

        private static bool AreDiscsAligned(List<Disc> discs, int time)
        {
            for (int i = 0; i < discs.Count; i++)
            {
                var position = discs[i].GetPosition(time + i + 1);
                if (position != 0)
                    return false;
            }

            return true;
        }
    }

    public class Disc
    {
        public int CurrentPosition { get; set; }
        public int NumberOfPositions { get; set; }

        public int GetPosition(int time)
        {
            return (CurrentPosition + time) % NumberOfPositions;
        }

        public Disc(int numberOfPositions, int currentPosition)
        {
            NumberOfPositions = numberOfPositions;
            CurrentPosition = currentPosition;
        }
    }
}
