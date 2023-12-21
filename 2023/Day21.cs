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
        for (int i = 0; i < 64; i++)
        {
            plots = plots.SelectMany(p => GetPossibleStepsFrom(p, map)).ToHashSet();
        }
        return plots.Count;
    }
    public override object Part2(List<string> input)
    {
        return null;
    }


    private static IEnumerable<Coordinate> GetPossibleStepsFrom(Coordinate current, Dictionary<Coordinate, char> map)
    {
        var up = current+Direction.Up;
        var right = current+Direction.Right;
        var down = current+Direction.Down;
        var left = current+Direction.Left;
        if(map.TryGetValue(up, out var uStep) && uStep != '#')
            yield return up;
        if(map.TryGetValue(right, out var rStep) && rStep != '#')
            yield return right;
        if(map.TryGetValue(down, out var dStep) && dStep != '#')
            yield return down;
        if(map.TryGetValue(left, out var lStep) && lStep != '#')
            yield return left;
    }
}
