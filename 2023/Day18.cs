using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2023;

[AoC(2023)]
public class Day18 : Day
{
    public Day18() : base(18, 2023)
    {

    }

    public override object Part1(List<string> input) => CountDepth(input.Select(i => DigInstruction.Create(i, false)).ToList());

    public override object Part2(List<string> input) => CountDepth(input.Select(i => DigInstruction.Create(i, true)).ToList());

    private static long CountDepth(List<DigInstruction> digPlan)
    {
        var corners = Corners(digPlan).ToList();

        var shifted = corners.Skip(1).Append(corners[0]);
        var zip = corners.Zip(shifted);
        var x = zip.Select(p => (Math.Abs(p.Second.X - p.First.X + p.Second.Y - p.First.Y), (long)p.First.X * p.Second.Y - (long)p.First.Y * p.Second.X));
        var border = Math.Abs(x.Sum(i => i.Item1));
        var inside = (Math.Abs(x.Sum(i => i.Item2)) / 2) - border / 2 + 1;

        return border + inside;
    }

    private static HashSet<Coordinate> Corners(List<DigInstruction> instructions)
    {
        var current = Coordinate.Zero;
        var list = new HashSet<Coordinate>();
        foreach (var instr in instructions)
        {
            list.Add(current = ApplyInstruction(current, instr));
        }

        return list;

        static Coordinate ApplyInstruction(Coordinate current, DigInstruction instr)
        {
            if(instr.Dir == Direction.Right)
                return new(current.X+instr.Steps, current.Y);
            if(instr.Dir == Direction.Down)
                return new(current.X, current.Y+instr.Steps);
            if(instr.Dir == Direction.Left)
                return new(current.X-instr.Steps, current.Y);
            if(instr.Dir == Direction.Up)
                return new(current.X, current.Y-instr.Steps);  
            throw new ArgumentException("Not a valid instruction"); 
        }
    }

    private record DigInstruction(Coordinate Dir, int Steps)
    {
        public static DigInstruction Create(string input, bool part2)
        {
            var parts = input.Split(" ");
            var d = part2 ? parts[2][7] : parts[0][0];
            var direction = d switch {
                'R' or '0' => Direction.Right,
                'U' or '1' => Direction.Up,
                'L' or '2' => Direction.Left,
                'D' or '3' => Direction.Down,
                _ => throw new ArgumentException($"'{input}' is not a valid line.")
            };
            var steps = part2 ? Convert.ToInt32(parts[2][2..7], 16) : int.Parse(parts[1]);
            return new DigInstruction(direction, steps);
        }
    }
}

