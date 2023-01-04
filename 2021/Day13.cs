using System;
using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    [AoC(2021)]
    public class Day13 : Day
    {
        public Day13() : base(13, 2021)
        {

        }

        public override object Part1(List<string> input)
        {
            var (points, instructions) = GetPointsAndInstructions(input);
            return PerformInstruction(instructions.First(), points).Count();
        }
        public override object Part2(List<string> input)
        {
            var (points, instructions) = GetPointsAndInstructions(input);
            instructions.ForEach(i => points = PerformInstruction(i, points));
            Console.WriteLine();
            Enumerable.Range(0, points.Max(p => p.Y) + 1).ForEach(y =>
            {
                Enumerable.Range(0, points.Max(p => p.X) + 1).ForEach(x => Console.Write(points.Any(p => p.X == x && p.Y == y) ? "█" : " "));
                Console.WriteLine();
            });
            return points.Count();
        }

        private (HashSet<(int X, int Y)> Points, List<(bool FoldUp, int Position)> Instructions) GetPointsAndInstructions(List<string> input)
        {
            var emptyEntryIndex = input.IndexOf(string.Empty);
            var points = input.Take(emptyEntryIndex).Select(p => (X: int.Parse(p.Split(",")[0]), Y: int.Parse(p.Split(",")[1]))).ToHashSet();

            var instructions =
                (from instr in input.Skip(emptyEntryIndex + 1)
                 let data = instr.Split("fold along ", StringSplitOptions.RemoveEmptyEntries)
                 from row in data
                 let parts = row.Split('=', StringSplitOptions.RemoveEmptyEntries)
                 select (Up: (parts[0] == "y" ? true : false), Position: int.Parse(parts[1].ToString())))
                .ToList();
            return (points, instructions);
        }

        private HashSet<(int X, int Y)> PerformInstruction((bool FoldUp, int Position) instruction, HashSet<(int X, int Y)> positions)
        {
            HashSet<(int X, int Y)> folded = new(positions.Where(p => (instruction.FoldUp ? p.Y : p.X) < instruction.Position));
            if (instruction.FoldUp)
                positions.Except(folded).ForEach(p => folded.Add(new(p.X, instruction.Position - (p.Y - instruction.Position))));
            else
                positions.Except(folded).ForEach(p => folded.Add(new(instruction.Position - (p.X - instruction.Position), p.Y)));
            return folded;
        }
    }
}
