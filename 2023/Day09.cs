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
        return list.Select(i => Calculate(i)).Sum();
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }

    static long Calculate(IEnumerable<long> list) =>
        list.Any() ? Calculate(list.DiffEveryOther()) + list.Last() : 0;
}

