using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var visitedLocations = new HashSet<Location>();

            Location firstTwiceVisitedLocation = null;

            var txt = File.ReadAllText("input.txt");
            var inputs = txt.Split(new string[] { ", " }, StringSplitOptions.None);

            var currentDirection = Directions.North;
            Location currentLocation = new Location(0, 0);
            visitedLocations.Add(currentLocation);

            foreach (var input in inputs)
            {
                var direction = input[0];
                var steps = int.Parse(input.Substring(1));

                if (direction == 'R')
                    currentDirection = currentDirection == Directions.West ? Directions.North : currentDirection + 1;
                if (direction == 'L')
                    currentDirection = currentDirection == Directions.North ? Directions.West : currentDirection - 1;

                for (int i = 0; i < steps; i++)
                {
                    currentLocation = new Location(currentLocation.X, currentLocation.Y);

                    if (currentDirection == Directions.North)
                        currentLocation.Y++;
                    if (currentDirection == Directions.South)
                        currentLocation.Y--;
                    if (currentDirection == Directions.East)
                        currentLocation.X++;
                    if (currentDirection == Directions.West)
                        currentLocation.X--;

                    if (visitedLocations.Contains(currentLocation) && firstTwiceVisitedLocation == null)
                        firstTwiceVisitedLocation = currentLocation;

                    visitedLocations.Add(currentLocation);
                }
            }

            Console.WriteLine(GetDistanceFromStart(currentLocation));
            Console.WriteLine(GetDistanceFromStart(firstTwiceVisitedLocation));
        }

        private static int GetDistanceFromStart(Location currentLocation)
        {
            return Math.Abs(currentLocation.X) + Math.Abs(currentLocation.Y);
        }
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj.ToString() == this.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public enum Directions
    {
        North,
        East,
        South,
        West
    }
}
