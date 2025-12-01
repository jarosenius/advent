using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2024;

[AoC(2024)]
public class Day19() : Day(19, 2024)
{
    public override object Part1(List<string> input)
    {
        var (towels, patterns) = ParseInput(input);
        return GetWaysToOrder(patterns, towels);
    }
    public override object Part2(List<string> input)
    {
        var (towels, patterns) = ParseInput(input);
        return GetWaysToOrder(patterns, towels, false);
    }

    private static (string[] towels, List<string>) ParseInput(List<string> input) => (input[0].Split(", "), input.Skip(2).ToList());

    private static long GetWaysToOrder(List<string> patterns, string[] towels, bool filterZero = true)
    {
        var ways = patterns.Select(p => GetWaysToOrder(p, towels, []));
        return filterZero ? ways.Count(w => w != 0) : ways.Sum();
    }

    private static long GetWaysToOrder(string pattern, string[] towels, ConcurrentDictionary<string, long> cache)
    {
        return cache.GetOrAdd(pattern, 
            p => !string.IsNullOrEmpty(p) 
                ? towels.Where(p.StartsWith)
                        .Sum(t => GetWaysToOrder(pattern[t.Length..], towels, cache)) 
                : 1);
    }
}
