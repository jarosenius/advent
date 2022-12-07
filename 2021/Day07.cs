using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2021
{
    [AoC(2021)]
    public class Day07 : Day
    {
        public Day07() : base(7, 2021)
        {

        }

        public override long Part1(List<string> input)
        {
            var numbers = input.First().SplitByAndParseToInt(",");
            var max = numbers.Max();
            var min = numbers.Min();
            return Enumerable.Range(min, max - min + 1).Min(x => numbers.Sum(n => Math.Abs(n - x)));
        }
        public override long Part2(List<string> input)
        {
            var numbers = input.First().SplitByAndParseToInt(",");
            var max = numbers.Max();
            var min = numbers.Min();
            return Enumerable.Range(min, max - min + 1).Min(x => numbers.Sum(n =>
                {
                    var s = Math.Abs(n - x);
                    return s * (s + 1) / 2;
                }));
        }
    }
}
