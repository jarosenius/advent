using System.Collections.Generic;

namespace Advent.y2025;

[AoC(2025)]
public class Day01() : Day(1, 2025)
{
    public override object Part1(List<string> input)
    {
        var pos = 50;
        var count = 0;

        foreach (var line in input)
        {
            var dir = line[0];
            var distance = int.Parse(line[1..]);

            pos = dir == 'L'
                ? (pos - distance).Mod(100)
                : (pos + distance) % 100;

            if (pos == 0)
                count++;
        }

        return count;
    }
    public override object Part2(List<string> input)
    {
        var pos = 50;
        var count = 0;

        foreach (var line in input)
        {
            var dir = line[0];
            var distance = int.Parse(line[1..]);

            count += CountZeroCrossings(pos, distance, dir);

            pos = dir == 'L'
                ? (pos - distance).Mod(100)
                : (pos + distance) % 100;
        }

        return count;
    }

    private static int CountZeroCrossings(int start, int distance, char dir)
    {
        var crossings = 0;
        var clicks = dir == 'R'
            ? (100 - start).Mod(100)
            : start == 0 ? 100 : start;

        if (dir == 'R' && clicks == 0)
            clicks = 100;

        if (distance >= clicks)
            crossings = 1 + (distance - clicks) / 100;

        return crossings;
    }

}

