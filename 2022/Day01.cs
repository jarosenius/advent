using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Advent.y2022;

[AoC(2022)]
public class Day01 : Day
{
    public Day01() : base(1, 2022)
    {
        
    }

    public override long Part1(List<string> input)
    {
        return GetCaloriesForElves(input).First();
    }
    public override long Part2(List<string> input)
    {
        return GetCaloriesForElves(input).Take(3).Sum();
    }

    private static IOrderedEnumerable<int> GetCaloriesForElves(List<string> input) 
    {
        return input.GroupWhile(row => row != string.Empty).Select(g => g.ReadLinesAsInt().Sum()).OrderByDescending(c => c);
    }
}

