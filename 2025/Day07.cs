using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day07() : Day(7, 2025)
{
    public override object Part1(List<string> input) => CountBeamSplits(input, false);
    public override object Part2(List<string> input) => CountBeamSplits(input, true);

    private static long CountBeamSplits(List<string> input, bool countTimelines)
    {
        var start = input[0].IndexOf('S');
        var timelines = new Dictionary<int, long> { { start, 1 } };
        var next = new Dictionary<int, long>();
        var sum = 0L;

        for (var row = 1; row < input.Count; row++)
        {
            var line = input[row];
            next.Clear();

            foreach (var (col, count) in timelines)
            {
                if (col < 0 || col >= line.Length)
                    continue;

                if (line[col] == '^')
                {
                    sum += countTimelines ? count : 1;
                    AddTimeline(next, col - 1, count, 0, line.Length);
                    AddTimeline(next, col + 1, count, 0, line.Length);
                }
                else
                {
                    AddTimeline(next, col, count, 0, line.Length);
                }
            }

            if (next.Count == 0)
                break;

            (timelines, next) = (next, timelines);
        }

        return sum;
    }

    private static void AddTimeline(Dictionary<int, long> dict, int col, long count, int minInc, int maxExc)
    {
        if (col < minInc || col >= maxExc)
            return;

        dict[col] = dict.TryGetValue(col, out var existing) ? existing + count : count;
    }
}

