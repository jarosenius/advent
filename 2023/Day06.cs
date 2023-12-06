using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2023;

[AoC(2023)]
public class Day06 : Day
{
    public Day06() : base(6, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var infos = GetInfo(input);
        return infos.Aggregate(1L, (res, i) => res *= CountNumberOfWays(i));
    }

    public override object Part2(List<string> input)
    {
        var time = int.Parse(input[0][5..].Replace(" ", ""));
        var distance = long.Parse(input[1][9..].Replace(" ", ""));
        return CountNumberOfWays(new Info(time, distance));
    }

    private static int CountNumberOfWays(Info info) => Enumerable.Range(0, info.Time + 1).Count(l => IsRecord(info, l));
    private static bool IsRecord(Info info, long hold) => (info.Time - hold) * hold > info.Distance;

    private static IEnumerable<Info> GetInfo(List<string> input)
    {
        var time = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value));
        var distance = Regex.Matches(input[1], @"\d+").Select(m => long.Parse(m.Value));
        return time.Zip(distance).Select(z => new Info(z.First, z.Second));
    }

    private record Info(int Time, long Distance);
}
