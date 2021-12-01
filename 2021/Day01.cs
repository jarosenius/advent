using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2021
{
    public class Day01 : Day
    {
        public Day01() : base(1, 2021)
        {
            
        }

        public override async Task<long> Part1(IEnumerable<string> input)
        {
            await Task.CompletedTask;
            return GetIncreases(input.Select(n => int.Parse(n)).ToList());
        }
        public override async Task<long> Part2(IEnumerable<string> input)
        {
            var nums = input.Select(n => int.Parse(n)).ToList();
            var numbersToUse = (nums.Count / 3) * 3;
            var threeMeasure = nums.Take(numbersToUse).Select((n, idx) => nums.Skip(idx).Take(3).Sum()).ToList();
            await Task.CompletedTask;
            return GetIncreases(threeMeasure);
        }

        private long GetIncreases(List<int> nums)
        {
            return nums.Zip(nums.Skip(1), (previous, current) => current > previous ? 1 : 0).Sum();
        }
    }
}
