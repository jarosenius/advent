using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2023;

[AoC(2023)]
public sealed class Day08 : Day
{
    public Day08() : base(8, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        return GetSteps(input, "AAA", "ZZZ");
    }
    public override object Part2(List<string> input)
    {
        return GetSteps(input, "..A", "..Z");
    }

    private static long GetSteps(List<string> input, string start, string target)
    {
        var order = input[0];
        var map = input.Skip(2).Select(i =>{
            var parts = Regex.Matches(i, @"\w+");
            return KeyValuePair.Create(parts[0].Value, (Left: parts[1].Value, Right: parts[2].Value));
        }).ToDictionary();

        return map.Keys.Aggregate(1L, (steps, key) => Regex.IsMatch(key, start) ? GetSteps(steps, key) : steps);

        long GetSteps(long steps, string key) {
            var count = 0;
            while (!Regex.IsMatch(key, target)) {
                var step = order[count++ % order.Length];
                key = GetTarget(step, key);
            }
            return steps.LeastCommonMultiple(count);
        }

        string GetTarget(char c, string key) => c switch
        {
            'R' => map[key].Right,
            'L' => map[key].Left,
            _ => throw new ArgumentException("Invalid input"),
        };
    }
}
