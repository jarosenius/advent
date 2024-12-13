using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2024;

[AoC(2024)]
public class Day13() : Day(13, 2024)
{
    private readonly Regex ButtonMatch = new (@"X\+(\d+), Y\+(\d+)", RegexOptions.Compiled);
    private readonly Regex PrizeMatch = new (@"X=(\d+), Y=(\d+)", RegexOptions.Compiled);
    public override object Part1(List<string> input)
    {
        return input.GroupWhile(r => !string.IsNullOrWhiteSpace(r)).Select(d => CreateMachine(d, false)).Sum(m => m.FindMinPresses(3, 1));
    }

    public override object Part2(List<string> input)
    {
        return input.GroupWhile(r => !string.IsNullOrWhiteSpace(r)).Select(d => CreateMachine(d, true)).Sum(m => m.FindMinPresses(3, 1));
    }

    
    private Machine CreateMachine(IEnumerable<string> group, bool part2)
    {
        var machineData = group.ToList();
        if(machineData.Count != 3)
            throw new ArgumentException("Invalid input");
        
        var a = ButtonMatch.Match(machineData[0]);
        var b = ButtonMatch.Match(machineData[1]);
        var prize = PrizeMatch.Match(machineData[2]);
        return new Machine(
            (int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value)), 
            (int.Parse(b.Groups[1].Value), int.Parse(b.Groups[2].Value)),
            (long.Parse(prize.Groups[1].Value) + (part2?10000000000000 : 0), long.Parse(prize.Groups[2].Value) + (part2?10000000000000 : 0)));
    }

    private record Machine((int X, int Y) A, (int X, int Y) B, (long X, long Y) Prize)
    {
        // Linear Diophantine equation
        public long FindMinPresses(int tokenA, int tokenB)
        {
            long minB = (Prize.Y * A.X - Prize.X * A.Y) / (B.Y * A.X - B.X * A.Y);
            long minA = (Prize.X - B.X * minB) / A.X;

            if (minA * A.X + minB * B.X == Prize.X && minA * A.Y + minB * B.Y == Prize.Y)
                return (minA * tokenA) + (minB * tokenB);
            else
                return 0;
        }
    }
}
