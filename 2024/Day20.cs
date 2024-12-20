using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day20() : Day(20, 2024)
{
    public override object Part1(List<string> input) => FindCheatsForPath([.. FindPath(input)], 2).Count(c => c.Value >= 100);

    public override object Part2(List<string> input) => FindCheatsForPath([.. FindPath(input)], 20).Count(c => c.Value >= 100);

    private static HashSet<Coordinate> FindPath(List<string> input)
    {
        var map = input.CreateImmutableMap();
        var start = map.Single(k => k.Value == 'S').Key;
        var end = map.Single(k => k.Value == 'E').Key;
        var (path, cost) = FindPath(map, start, end);
        return path;
    }

    private static (HashSet<Coordinate> Path, int Cost) FindPath(ImmutableDictionary<Coordinate, char> map, Coordinate start, Coordinate goal)
    {
        var cost = 0;
        var pos = start;
        var queue = new PriorityQueue<Coordinate, int>();
        var costForPosition = new Dictionary<Coordinate, int>(){{start, cost}};
        var path = new Dictionary<Coordinate, Coordinate>();
        do
        {
            if (pos == goal)
                return (WalkPathBackwards(path, pos), costForPosition[pos]);
            Direction.Directions.ForEach(d =>
            {
                var next = pos+d;
                if(!map.TryGetValue(next, out var value) || value == '#')
                    return;

                var newCost = costForPosition[pos] + 1;
                if(newCost < costForPosition.GetValueOrDefault(next, int.MaxValue))
                {
                    path[next] = pos;
                    costForPosition[next] = newCost;
                    queue.Enqueue(next, newCost);
                }
            });
        } while (queue.TryDequeue(out pos, out cost));
        return ([], -1);
    }

    private static HashSet<Coordinate> WalkPathBackwards(Dictionary<Coordinate, Coordinate> path, Coordinate pos)
    {
        Stack<Coordinate> res = new();
        res.Push(pos);
        while (path.ContainsKey(pos))
        {
            pos = path[pos];
            res.Push(pos);
        }
        return [.. res];
    }

    private static Dictionary<(Coordinate, Coordinate), int> FindCheatsForPath(List<Coordinate> path, int maxDistance)
    {
        var costs = Enumerable.Range(0, path.Count).ToArray();
        return costs.SelectMany(p => FindCheatsForPosition(p, costs[0..p], path, maxDistance)).ToDictionary();
    }
    private static IEnumerable<KeyValuePair<(Coordinate, Coordinate), int>> FindCheatsForPosition(int end, int[] costs, List<Coordinate> path, int maxDistance)
    {
        return from start in costs[0..end]
                let distance = path[start].Distance(path[end])
                where distance <= maxDistance
                select KeyValuePair.Create((path[start], path[end]), end - (start + distance));
    }
}
