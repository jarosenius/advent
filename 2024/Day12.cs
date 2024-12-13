using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day12() : Day(12, 2024)
{
    public override object Part1(List<string> input)
    {
        var gardenPlots = GetGardenPlots(Coordinate.CreateMap(input));
        return gardenPlots.Values.SelectMany(v => v).Sum(g => CalculateCost(g, false));
    }

    public override object Part2(List<string> input)
    {
        var gardenPlots = GetGardenPlots(Coordinate.CreateMap(input));
        return gardenPlots.Values.SelectMany(v => v).Sum(g => CalculateCost(g, true));
    }

    private static ConcurrentDictionary<char, List<HashSet<Coordinate>>> GetGardenPlots(Dictionary<Coordinate, char> map)
    {
        var gardenPlots = new ConcurrentDictionary<char, List<HashSet<Coordinate>>>();

        while (map.Count > 0)
        {
            var current = map.First().Key;
            var c = map[current];
            var garden = new HashSet<Coordinate>{current};
            map.Remove(current);

            var queue = new Queue<Coordinate>(Direction.Directions.Select(d => current + d));
            while (queue.TryDequeue(out current))
            {
                if (map.TryGetValue(current, out var value) && value == c)
                {
                    Direction.Directions.ForEach(d => queue.Enqueue(current + d));
                    garden.Add(current);
                    map.Remove(current);
                }
            }
            gardenPlots.GetOrAdd(c, []).Add(garden);
        }

        return gardenPlots;
    }

    private static int CalculateCost(HashSet<Coordinate> garden, bool discount = false) => garden.Count * (discount ? CalculateCorners(garden) : CalculatePerimeter(garden));

    private static int CalculatePerimeter(HashSet<Coordinate> garden) => garden.Sum(p =>
                    (!garden.Contains(p + Direction.Up) ? 1 : 0) +
                    (!garden.Contains(p + Direction.Down) ? 1 : 0) +
                    (!garden.Contains(p + Direction.Left) ? 1 : 0) +
                    (!garden.Contains(p + Direction.Right) ? 1 : 0)
                );

    public static int CalculateCorners(HashSet<Coordinate> garden)
    {
        var corners = 0; 
        var cornerTypes = new[] 
        {
            (Direction.Left, Direction.Up), 
            (Direction.Up, Direction.Right),
            (Direction.Right, Direction.Down),
            (Direction.Down, Direction.Left)
        };

        foreach (var plot in garden)
        {
            foreach (var (a, b) in cornerTypes)
            {
                if(!garden.Contains(plot + a) && !garden.Contains(plot+b))
                    corners++;

                if(garden.Contains(plot + a) && garden.Contains(plot+b) && !garden.Contains(plot+a+b))
                    corners++;
            }
        }
        return corners;
    }
}
