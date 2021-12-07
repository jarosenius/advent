using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace Advent.y2021
{
    public class Day07 : Day
    {
        public Day07() : base(7, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var numbers = input.First().SplitByAndParseToInt(",");
            return Enumerable.Range(0, numbers.Max())
                .Select(x => numbers.Sum(n => Math.Abs(n - x)))
                .Min();
        }
        public override long Part2(List<string> input)
        {
            var numbers = input.First().SplitByAndParseToInt(",");
            return Enumerable.Range(0, numbers.Max())
                .Select(x => numbers.Sum(n => 
                {
                    var r = Math.Abs(n - x);
                    return r * (r+1)/2;
                }))
                .Min();
        }
    }
}
