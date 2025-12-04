using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2025;

[AoC(2025)]
public class Day04() : Day(4, 2025)
{
    public override object Part1(List<string> input) => CountRemovableRolls(GetRolls(input), true);
    public override object Part2(List<string> input) => CountRemovableRolls(GetRolls(input), false);

    private static HashSet<Coordinate> GetRolls(List<string> input) =>
        [.. input.CreateMap().Where(kvp => kvp.Value == '@').Select(kvp => kvp.Key)];

    private static int CountRemovableRolls(HashSet<Coordinate> rolls, bool part1)
    {
        var totalRemoved = 0;
        var removed = GetRemovableRolls(rolls);
        if (part1)
            return removed.Count;

        do
        {
            totalRemoved += removed.Count;
            rolls.ExceptWith(removed);
            removed = GetRemovableRolls(rolls);
        } while (removed.Count > 0);

        return totalRemoved;
    }

    private static List<Coordinate> GetRemovableRolls(HashSet<Coordinate> rolls)
    {
        var toRemove = new List<Coordinate>();
        foreach (var roll in rolls)
        {
            if (CountAdjacentRolls(roll, rolls) < 4)
            {
                toRemove.Add(roll);
            }
        }

        return toRemove;
    }

    private static int CountAdjacentRolls(Coordinate roll, HashSet<Coordinate> rolls) =>
        Direction.DirectionsWithDiagonals.Count(dir => rolls.Contains(roll + dir));
}
