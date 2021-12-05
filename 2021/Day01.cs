using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day01 : Day
    {
        public Day01() : base(1, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            return GetIncreases(input.ReadLinesAsInt());
        }
        public override long Part2(List<string> input)
        {
            var nums = input.Select(n => int.Parse(n)).ToList();
            var numbersToUse = (nums.Count / 3) * 3;
            var threeMeasure = nums.Take(numbersToUse).Select((n, idx) => nums.Skip(idx).Take(3).Sum()).ToList();
            
            return GetIncreases(threeMeasure);
        }

        private long GetIncreases(IEnumerable<int> nums)
        {
            return nums.Zip(nums.Skip(1), (previous, current) => current > previous ? 1 : 0).Sum();
        }
    }
}
