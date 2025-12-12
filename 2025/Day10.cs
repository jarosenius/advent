using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2025;

[AoC(2025)]
public partial class Day10() : Day(10, 2025)
{
    public override object Part1(List<string> input) => input.Sum(SolveMachine);

    public override object Part2(List<string> input)
    {
        /*
         * Looking at the example I can see that it's a ILP problem.
         *         b1  b2    b3  b4    b5    b6
         * [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
         * b6 = 3, b2 + b6 = 5, b3 + b4 + b5 = 4, b1 + b2 + b4 = 7.
         * and so on.
         * i = 1..n (n = number of buttons)
         * b[i] = number of presses for button i
         * j = 1..m (m = number of counters)
         * sum(b[i] for all buttons i that affect counter j) = target[j]
         * minimize sum(b[i]) for i = 1..n
         * So far I haven't been able to create a solution that works in reasonable time by myself.
         */
        return null;
    }

    private static int SolveMachine(string line)
    {
        var (target, buttons) = ParseMachine(line);

        var n = buttons.Count;
        var minPresses = int.MaxValue;

        for (var mask = 0; mask < (1 << n); mask++)
        {
            var state = new bool[target.Length];
            var presses = 0;

            for (var i = 0; i < n; i++)
            {
                if ((mask & (1 << i)) == 0)
                    continue;

                foreach (var light in buttons[i])
                {
                    state[light] = !state[light];
                }
                presses++;
            }

            if (state.SequenceEqual(target))
                minPresses = Math.Min(minPresses, presses);
        }

        return minPresses == int.MaxValue ? 0 : minPresses;
    }

    private static (bool[] target, List<List<int>> buttons) ParseMachine(string line)
    {
        var target =
            TargetRegex()
                .Match(line).Groups[1].Value.Select(c => c == '#')
                .ToArray();

        var buttons =
            ButtonRegex()
                .Matches(line)
                .Select(m => m.Groups[1].Value.SplitByAndParseToInt(",").ToList())
            .ToList();

        return (target, buttons);
    }

    [GeneratedRegex(@"\[([.#]+)\]")]
    private static partial Regex TargetRegex();
    [GeneratedRegex(@"\(([0-9,]+)\)")]
    private static partial Regex ButtonRegex();
}
