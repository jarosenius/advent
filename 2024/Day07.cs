using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day07() : Day(7, 2024)
{
    public override object Part1(List<string> input)
    {
        var eq = input.Select(r => r.SplitByFirstThenBySecondAndParseToLong(": ", " ")).ToList();
        return eq.Where(e => Solvable(e[0][0], e[1], 1, e[1][0])).Select(e => e[0][0]).Sum();
    }
    public override object Part2(List<string> input)
    {
        return null;
    }

    private static bool Solvable(long target, long[] numbers, int index, long currentValue)
    {
        if (index == numbers.Length)
        {
            return currentValue == target;
        }

        if (Solvable(target, numbers, index + 1, currentValue + numbers[index]))
        {
            return true;
        }

        if (Solvable(target, numbers, index + 1, currentValue * numbers[index]))
        {
            return true;
        }

        return false;
    }
}

