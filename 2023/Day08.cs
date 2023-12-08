using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day08 : Day
{
    public Day08() : base(8, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        return GetSteps(input);
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }

    private static int GetSteps(List<string> input)
    {
        var order = input[0];
        var map = input.Skip(2).Select(i =>{
            var parts = i.Split(" = ");
            var leftRight = parts[1].Split(", ");
            var kvp = new KeyValuePair<string, (string Left, string Right)>(parts[0], (leftRight[0][1..], leftRight[1][..^1]));
            return kvp;
        }).ToDictionary();


        var count = 0;
        var current = "AAA";
        do
        {
            var step = order[count++%order.Length];
            current = step switch
            {
                'R' => map[current].Right,
                'L' => map[current].Left,
                _ => throw new ArgumentException("Invalid input"),
            };
        } while (current != "ZZZ");
        
        return count;
    }
}
