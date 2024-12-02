using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day02 : Day
{
    public Day02() : base(2, 2024)
    {

    }

    public override object Part1(List<string> input)
    {
        var reports = GetReports(input);
        return reports.Count(r => r.Item1.Zip(r.Item1.Skip(1)).All(p => IsValid(p.First, p.Second, r.Item2)));
    }

    public override object Part2(List<string> input)
    {
        return null;
    }

    private static List<(int[], bool)> GetReports(List<string> input)
    {
        return input.Select(r =>
        {
            var report = r.SplitByAndParseToInt(" ");
            var increase = report[1] > report[0];
            return (report, increase);
        }).ToList();
    }

    private static bool IsValid(int a, int b, bool increase)
    {
        return increase ? b > a && b-a <=3 : a > b && a-b <=3;
    }
}

