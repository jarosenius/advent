using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day15 : Day
    {
        private Dictionary<int, List<int>> previous = new Dictionary<int, List<int>>();

        public Day15() : base(15, 2020)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var target = 2020;
            List<int> numbers = GetAnswer(input, target);
            
            return numbers[target - 1];
        }
        public override long Part2(List<string> input)
        {
            var target = 30000000;
            var numbers = GetAnswer(input, target);
            
            return numbers[target - 1];
        }

        private List<int> GetAnswer(IEnumerable<string> input, int target)
        {
            var lastSeen = new List<int>();
            previous = new Dictionary<int, List<int>>();

            lastSeen.AddRange(input.First().Split(",").Select(int.Parse));
            for (var i = 0; i < lastSeen.Count; i++)
            {
                AddToPrevious(lastSeen[i], i);
            }

            for (var i = lastSeen.Count; i < target; i++)
            {
                var prevNumb = lastSeen[i - 1];
                if (!previous.ContainsKey(prevNumb) || previous[prevNumb].Count < 2)
                {
                    lastSeen.Add(0);
                }
                else if (previous[prevNumb].Count > 1)
                {
                    var allIndexesOf = previous[prevNumb];
                    var length = allIndexesOf.Count;
                    lastSeen.Add(allIndexesOf[length - 1] - allIndexesOf[length - 2]);
                }
                AddToPrevious(lastSeen[i], i);
            }
            return lastSeen;
        }

        void AddToPrevious(int n, int index)
        {
            if (!previous.ContainsKey(n))
                previous.Add(n, new List<int>() { index });
            else
                previous[n].Add(index);
        }
    }
}
