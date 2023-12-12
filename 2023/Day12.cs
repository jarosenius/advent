using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Advent.y2023;

[AoC(2023)]
public class Day12 : Day
{
    public Day12() : base(12, 2023)
    {

    }

    public override object Part1(List<string> input) => GetResult(input, 1);

    public override object Part2(List<string> input) => GetResult(input, 5);
    
    private static long GetResult(List<string> input, int unfolds)
    {
        var data = input.Select(i => ParseRow(i, unfolds)).ToList();
        return data.Sum(d => CalculateArrangements(d.Springs, d.Pattern, d.Springs.Length, []));
    }

    private static long CalculateArrangements(string springs, int[] pattern, int length, Dictionary<(string, string, int), long> cache)
    {
        long result = 0L;
        var key = (springs, string.Join(",", pattern), length);
        if(cache.TryGetValue(key, out var res))
        {
            return res;
        }

        if(pattern.Length == 0)
            result = springs.All(c => c is '.' or '?') ? 1 : 0;
        else
            result = CalculateSubArrangements(springs, pattern, length, cache);

        cache[key] = result;
        return result;

        static long CalculateSubArrangements(string springs, int[] pattern, int length, Dictionary<(string, string, int), long> cache)
        {
            var current = pattern[0];
            var rest = pattern[1..];
            var remaining = rest.Sum() + rest.Length;
            var max = length - remaining - current + 1;
            return Enumerable.Range(0, max).Sum(b =>
            {
                var p = Enumerable.Repeat('.', b).Concat(Enumerable.Repeat('#', current)).Concat(new[] { '.' }).ToArray();
                var (Springs, Pattern, Length) = (p.Length > springs.Length ? string.Empty : springs[p.Length..], rest, length - current - b - 1);
                if (springs.Zip(p).All((c) => c.First == c.Second || c.First == '?'))
                    return CalculateArrangements(Springs, Pattern, Length, cache);

                return 0;
            });
        }
    }

    private static (string Springs, int[] Pattern) ParseRow(string row, int unfolds)
    {
        var parts = row.Split(" ");
        var springs = string.Join("?", Enumerable.Repeat(parts[0], unfolds));
        var pattern = Enumerable.Repeat(parts[1].SplitByAndParseToInt(","), unfolds).SelectMany(p => p).ToArray();
        return (springs, pattern);
    }
}

