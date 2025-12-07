using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2025;

[AoC(2025)]
public class Day07() : Day(7, 2025)
{
    public override object Part1(List<string> input) => CountBeamSplits(input);
    public override object Part2(List<string> input)
    {
        return null;
    }

    private static int CountBeamSplits(List<string> input)
    {
        var start = input[0].IndexOf('S');
        var active = new HashSet<int> { start };
        var sum = 0;

        for (var row = 1; row < input.Count; row++)
        {
            var line = input[row];
            var next = new HashSet<int>();

            foreach (var col in active)
            {
                if (col >= 0 && col < line.Length && line[col] == '^')
                {
                    sum++;
                    next.Add(col - 1);
                    next.Add(col + 1);
                }
                else
                {
                    next.Add(col);
                }
            }

            active = [.. next.Where(c => c >= 0 && c < line.Length)];

            if (active.Count == 0)
                break;
        }

        return sum;
    }
}

