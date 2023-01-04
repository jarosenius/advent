using System.Collections.Generic;
using System.Linq;

namespace Advent.y2022;

[AoC(2022)]
public class Day06 : Day
{
    public Day06() : base(6, 2022)
    {

    }

    public override object Part1(List<string> input)
    {
        return GetAnswer(input[0], 4);
    }

    public override object Part2(List<string> input)
    {
        return GetAnswer(input[0], 14);
    }

    private static int GetAnswer(string line, int window)
    {
        return Enumerable.Range(window, line.Length).First(i => line.Substring(i - window, window).ToHashSet().Count == window);
    }
}

