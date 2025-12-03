using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day03() : Day(3, 2025)
{
    public override object Part1(List<string> input) => input.Sum(row => GetMaxJoltage(row, 2));
    public override object Part2(List<string> input) => input.Sum(row => GetMaxJoltage(row, 12));

    private static long GetMaxJoltage(string row, int batteries)
    {
        var result = 0L;
        var start = 0;

        for (var pos = 0; pos < batteries; pos++)
        {
            var remaining = batteries - pos;
            var end = row.Length - remaining + 1;

            var max = '0';
            var maxIdx = start;

            for (var i = start; i < end; i++)
            {
                if (row[i] <= max)
                    continue;
                max = row[i];
                maxIdx = i;

                if (max == '9')
                    break;
            }

            result = result * 10 + (max - '0');
            start = maxIdx + 1;
        }

        return result;
    }
}

