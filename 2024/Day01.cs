using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day01 : Day
{
    public Day01() : base(1, 2024)
    {

    }

    public override object Part1(List<string> input)
    {
        var (left, right) = GetLists(input);
        return Enumerable.Zip(left, right).Sum(p => Math.Abs(p.First - p.Second));
    }

    private static (List<int> Left, List<int> Right) GetLists(List<string> input)
    {
        const string reg = @"(\d+)\s+(\d+)";
        var regex = new Regex(reg, RegexOptions.Compiled);
        var lists = input.Select(r =>
        {
            var match = regex.Match(r);
            if (match.Success)
            {
                return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            }
            return (0, 0);
        });

        var left = lists.Select(e => e.Item1).OrderBy(e => e).ToList();
        var right = lists.Select(e => e.Item2).OrderBy(e => e).ToList();
        return (left, right);
    }

    public override object Part2(List<string> input)
    {
        return null;
    }
}

