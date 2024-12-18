using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day18() : Day(18, 2024)
{
    public override object Part1(List<string> input)
    {
        var wh = UsedExampleData ? (6, 6) : (70, 70);
        var (map, data) = ParseInput(input, wh);
        return Simulate(map, data, UsedExampleData ? 12 : 1024, wh).Cost;
    }

    public override object Part2(List<string> input)
    {
        var wh = UsedExampleData ? (6, 6) : (70, 70);
        var (map, data) = ParseInput(input, wh);
        var initial = UsedExampleData ? 12 : 1024;
        var (umap, path, cost) = Simulate(map, data, initial, wh);
        for (var i = 1; initial+i < data.Count; i++)
        {
            var corrupted = data[initial+i-1];
            if(!path.Contains(corrupted))
                continue;

            (umap, path, cost) = Simulate(map, data, initial+i, wh);
            if(cost == -1)
            {
                Console.WriteLine($"{corrupted.X},{corrupted.Y}");
                return -1;
            }
        }
        return null;
    }

    private static (ImmutableDictionary<Coordinate, char> Map, List<Coordinate> Data) ParseInput(List<string> input, (int, int) wh)
    {
        var map = (from y in Enumerable.Range(0, wh.Item2 + 1)
               from x in Enumerable.Range(0, wh.Item1 + 1)
               select new Coordinate(x, y)).ToImmutableDictionary(c => c, _ => '.');
        var data = input.Select(r =>
        {
            var parts = r.SplitByAndParseToInt(",");
            return new Coordinate(parts[0], parts[1]);
        }).ToList();
        return (map, data);
    }

    private static (ImmutableDictionary<Coordinate, char> Map, HashSet<Coordinate> Path, int Cost) Simulate(ImmutableDictionary<Coordinate, char> map, List<Coordinate> corruptedMemory, int count, (int Width, int Height) wh)
    {
        var updatedMap = map.SetItems(corruptedMemory.Take(count).Select(m => KeyValuePair.Create(m, '#')));
        var start = Coordinate.Zero;
        var end = new Coordinate(wh.Width, wh.Height);
        var (path, cost) = CostOfBestPath(updatedMap, start, end);
        return (updatedMap, path, cost);
    }

    private static (HashSet<Coordinate> Path, int Cost) CostOfBestPath(ImmutableDictionary<Coordinate, char> map, Coordinate start, Coordinate goal)
    {
        var cost = 0;
        var pos = start;
        var queue = new PriorityQueue<Coordinate, int>();
        var costForPosition = new Dictionary<Coordinate, int>(){{start, 0}};
        var path = new Dictionary<Coordinate, Coordinate>();
        do
        {
            if (pos == goal)
            {
                return (WalkPathBackwards(path, pos), costForPosition[pos]);
            }
            Direction.Directions.ForEach(d =>
            {
                var next = pos+d;
                if(!map.TryGetValue(next, out var value) || value != '.')
                    return;

                var newCost = costForPosition[pos] + 1;
                if(newCost < costForPosition.GetValueOrDefault(next, int.MaxValue))
                {
                    path[next] = pos;
                    costForPosition[next] = newCost;
                    var distanceFromGoal = Math.Abs(pos.X - goal.X) + Math.Abs(pos.Y - goal.Y);
                    queue.Enqueue(next, newCost+ distanceFromGoal);
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
}
