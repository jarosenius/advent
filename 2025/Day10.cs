using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Common;

namespace Advent.y2025;

[AoC(2025)]
public partial class Day10() : Day(10, 2025)
{
    public override object Part1(List<string> input) => input.Sum(SolveMachine);

    public override object Part2(List<string> input) => input.Sum(SolveMachineJoltage);


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

    private static long SolveMachineJoltage(string line)
    {
        var (targets, buttons) = ParseMachineJoltage(line);

        var result = Solve(targets, [.. buttons.Select(b => b.ToArray())]);
        return result ?? -1;
    }

    private static long? Solve(int[] targets, int[][] buttons)
    {
        /*
         * Inspiration from: https://github.com/xiety/AdventOfCode/blob/main/2025/0/Problem10/Problem10.cs
         */
        var cache = new Dictionary<long, long?>();
        var numComponents = targets.Length;
        var numButtons = buttons.Length;

        var buttonMasks = new int[numButtons];
        for (var b = 0; b < numButtons; b++)
        {
            var mask = buttons[b].Aggregate(0, (current, c) => current | (1 << c));
            buttonMasks[b] = mask;
        }

        var parityResults = new int[1 << numButtons];
        for (var m = 0; m < parityResults.Length; m++)
        {
            var resultMask = 0;
            for (var b = 0; b < numButtons; b++)
                if (((m >> b) & 1) == 1)
                    resultMask ^= buttonMasks[b];
            parityResults[m] = resultMask;
        }

        return SolveRecursive(targets);

        long GetHashCode(int[] arr) => arr.Aggregate<int, long>(0, (current, t) => current * 31 + t);

        long? SolveRecursive(int[] target)
        {
            var allZero = target.All(t => t == 0);
            if (allZero)
                return 0;

            var key = GetHashCode(target);
            if (cache.TryGetValue(key, out var cached))
                return cached;

            var parityMask = 0;
            for (var i = 0; i < target.Length; i++)
                if ((target[i] & 1) == 1)
                    parityMask |= 1 << i;

            long? minCost = null;

            for (var m = 0; m < (1 << numButtons); m++)
            {
                var maskCost = System.Numerics.BitOperations.PopCount((uint)m);
                if (maskCost >= minCost)
                    continue;

                if (parityResults[m] != parityMask)
                    continue;

                var newTarget = new int[numComponents];
                var valid = true;

                for (var c = 0; c < numComponents; c++)
                {
                    var contribution = 0;
                    for (var b = 0; b < numButtons; b++)
                    {
                        if (((m >> b) & 1) == 1 && ((buttonMasks[b] >> c) & 1) == 1)
                            contribution++;
                    }

                    var remaining = target[c] - contribution;
                    if (remaining < 0 || (remaining & 1) != 0)
                    {
                        valid = false;
                        break;
                    }

                    newTarget[c] = remaining >> 1;
                }

                if (!valid)
                    continue;

                var res = SolveRecursive(newTarget);
                if (!res.HasValue)
                    continue;
                var cost = maskCost + (res.Value << 1);
                minCost = minCost.HasValue ? Math.Min(minCost.Value, cost) : cost;
            }

            cache[key] = minCost;
            return minCost;
        }
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

    private static (int[] targets, List<List<int>> buttons) ParseMachineJoltage(string line)
    {
        var targets =
            JoltageRegex()
                .Match(line).Groups[1].Value.SplitByAndParseToInt(",");

        var buttons =
            ButtonRegex()
                .Matches(line)
                .Select(m => m.Groups[1].Value.SplitByAndParseToInt(",").ToList())
            .ToList();

        return (targets, buttons);
    }

    [GeneratedRegex(@"\[([.#]+)\]")]
    private static partial Regex TargetRegex();
    [GeneratedRegex(@"\(([0-9,]+)\)")]
    private static partial Regex ButtonRegex();
    [GeneratedRegex(@"\{([0-9,]+)\}")]
    private static partial Regex JoltageRegex();
}