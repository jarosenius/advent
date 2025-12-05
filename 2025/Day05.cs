using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day05() : Day(5, 2025)
{
    public override object Part1(List<string> input)
    {
        var (ranges, ingredients) = ParseInput(input);
        return ingredients.Count(id => ranges.Any(range => id >= range.Start && id <= range.End));
    }

    public override object Part2(List<string> input) =>
        MergeRanges(ParseRanges(input)).Sum(range => range.End - range.Start + 1);

    private static (List<Range> Ranges, List<long> Ingredients) ParseInput(List<string> input)
    {
        var blank = input.IndexOf("");

        return (ParseRanges(input, blank),
            Ingredients:
            [.. input.Skip(blank + 1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(long.Parse)]);
    }
    private static List<Range> ParseRanges(List<string> input, int? end = null)
    {
        var take = end ?? input.IndexOf("");

        return [.. input.Take(take)
            .Select(line => line.SplitByAndParseToLong("-"))
            .Select(parts => new Range(parts[0], parts[1]))];
    }

    private static List<Range> MergeRanges(List<Range> ranges)
    {
        if (ranges.Count == 0)
            return ranges;

        var sorted = ranges.OrderBy(r => r.Start).ToList();
        var merged = new List<Range> { sorted[0] };
        sorted.Skip(1).ForEach(current =>
        {
            var last = merged[^1];
            var shouldExtend = current.Start <= last.End + 1;
            if (shouldExtend)
                merged[^1] = last with { End = Math.Max(last.End, current.End) };
            else
                merged.Add(current);
        });

        return merged;
    }

    private record Range(long Start, long End);
}

