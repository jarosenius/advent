using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2023;

[AoC(2023)]
public class Day21 : Day
{
    public Day21() : base(21, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var map = Coordinate.CreateMap(input);
        HashSet<Coordinate> plots = [map.Single(m => m.Value == 'S').Key];
        for (var i = 0; i < 64; i++)
        {
            plots = plots.SelectMany(p => GetPossibleStepsFrom(p, map).Select(p1 => p1.Item1)).ToHashSet();
        }
        return plots.Count;
    }
    public override object Part2(List<string> input)
    {
        var map = Coordinate.CreateMap(input);
        var width = input.Count;
        var totalSteps = 26501365;
        var repetitions = totalSteps / width;
        var remainingSteps = totalSteps % width;

        HashSet<Coordinate> plots = [map.Single(m => m.Value == 'S').Key];
        var perLoop = new List<(int Step, int Plots, int UniquePositions)>();

        for(var x = 1; x <= width*2; x++)
        {
            var d = plots.SelectMany(p => GetPossibleStepsFrom(p, map, true, width));
            var uniquePositionsOnMap = d.Select(p => p.Item2).ToHashSet();
            plots = d.Select(p => p.Item1).ToHashSet();
            perLoop.Add((x, plots.Count, uniquePositionsOnMap.Count));        
        }

        return 0;
    }


    private static IEnumerable<(Coordinate, Coordinate)> GetPossibleStepsFrom(Coordinate current, Dictionary<Coordinate, char> map, bool loopAround = false, int mapWidth = -1)
    {
        foreach (var dir in Direction.Directions)
        {
            var newPos = current + dir;
            if(loopAround)
                newPos = newPos with {X = ((newPos.X % mapWidth) + mapWidth) % mapWidth, Y = ((newPos.Y % mapWidth) + mapWidth) % mapWidth};
            if(map.TryGetValue(newPos, out var step) && step != '#')
                yield return (current + dir, newPos);
        }
    }
}
