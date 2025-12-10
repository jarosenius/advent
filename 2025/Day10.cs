using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2025;

[AoC(2025)]
public partial class Day10() : Day(10, 2025)
{
    public override object Part1(List<string> input) => input.Sum(SolveMachine);
    
    public override object Part2(List<string> input) => null;

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
