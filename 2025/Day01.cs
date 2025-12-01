using System.Collections.Generic;

namespace Advent.y2025;

[AoC(2025)]
public class Day01() : Day(1, 2025)
{
    public override object Part1(List<string> input) => Solve(input, true);

    public override object Part2(List<string> input) => Solve(input, false);


    private static int Solve(List<string> input, bool onlyEndPositions)
    {
        var pos = 50;
        var count = 0;

        foreach (var line in input)
        {
            var dir = line[0];
            var distance = int.Parse(line[1..]);

            if (onlyEndPositions)
            {
                pos = Rotate(pos, distance, dir);
                if (pos == 0)
                    count++;
            }
            else
            {
                count += CountZeroCrossings(pos, distance, dir);
                pos = Rotate(pos, distance, dir);
            }
        }

        return count;
    }

    private static int Rotate(int pos, int distance, char dir) =>
        dir == 'L'
            ? (pos - distance).Mod(100)
            : (pos + distance) % 100;

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

