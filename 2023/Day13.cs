using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq.Extensions;

namespace Advent.y2023;

[AoC(2023)]
public class Day13 : Day
{
    public Day13() : base(13, 2023) { }

    public override object Part1(List<string> input) => GetResult(input, 0);
    public override object Part2(List<string> input) => GetResult(input, 1);
    private static int GetResult(List<string> input, int smudges) => input.Split("")
        .Select(p => p.ToList())
        .Sum(p => CountRows(p, smudges) + CountCols(p, smudges));
    private static int CountRows(List<string> map, int smudges) => Count(smudges, i => map[i].ToArray(), i => i * 100, map.Count);
    private static int CountCols(List<string> map, int smudges) => Count(smudges, i => map.SliceColumn(i), i => i, map[0].Length);

    private static int Count<T>(int smudges, Func<int, IEnumerable<T>> input, Func<int, int> sumValue, int count)
    {
        for (var reflect = 0; reflect < count - 1; reflect++)
        {
            var diff = 
                Enumerable.Range(0, count)
                .Where(delta => reflect - delta >= 0 && reflect + delta + 1 < count)
                .Sum(delta => 
                        input(reflect - delta)
                        .Zip(input(reflect + delta + 1))
                        .Count(d => !d.First.Equals(d.Second)));

            if (diff == smudges)
                return sumValue(reflect + 1);
        }
        return 0;
    }
}

