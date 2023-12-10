using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day09 : Day
{
    public Day09() : base(9, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var list = input.Select(i => i.SplitByAndParse(" ", long.Parse));
        return list.Select(i => CalculateRightSide(i)).Sum();
    }
    public override object Part2(List<string> input)
    {
        var list = input.Select(i => i.SplitByAndParse(" ", long.Parse));
        return list.Select(i => CalculateLeftSide(i)).Sum();
    }

    static long CalculateRightSide(IEnumerable<long> list) =>
        list.Any() ? CalculateRightSide(list.DiffEveryOther()) + list.Last() : 0;

    long CalculateLeftSide(IEnumerable<long> list) =>
       CalculateRightSide(list.Reverse().ToArray());
}

