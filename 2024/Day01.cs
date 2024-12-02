using System;
using System.Collections.Concurrent;
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

    public override object Part2(List<string> input)
    {
        var (left, right) = GetLists(input);
        var lookup = new ConcurrentDictionary<int, int>();
        return left.Sum(e => lookup.GetOrAdd(e, right.Count(r => r == e) * e));
    }

    private static (List<int> Left, List<int> Right) GetLists(List<string> input)
    {
        var lists = input.Select(r => {
            var parts = r.Split("   ");
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        });

        var left = lists.Select(e => e.Item1).OrderBy(e => e).ToList();
        var right = lists.Select(e => e.Item2).OrderBy(e => e).ToList();
        return (left, right);
    }
}

