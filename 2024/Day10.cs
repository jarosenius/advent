using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day10() : Day(10, 2024)
{
    public override object Part1(List<string> input)
    {
        var map = Coordinate.CreateMap(input, c => c-'0');
        return map.Where(m => m.Value == 0).Sum(t => TrailHeadScore(t.Key, map));
    }

    public override object Part2(List<string> input)
    {
        var map = Coordinate.CreateMap(input, c => c-'0');
        return map.Where(m => m.Value == 0).Sum(t => TrailHeadScore(t.Key, map, true));
    }

    private static int TrailHeadScore(Coordinate t, Dictionary<Coordinate, int> map, bool byRating = false)
    {
        var reached = new ConcurrentDictionary<Coordinate, int>();
        var queue = new Queue<(Coordinate Coordinate, int Height)>([(t, 0)]);
        while(queue.TryDequeue(out var current))
        {
            Direction.Directions.ForEach(d => 
            {
                var next = current.Coordinate + d;
                if(map.TryGetValue(next, out var value) && value == current.Height+1)
                {
                    if(value == 9)
                        reached.AddOrUpdate(next, 1, (_, v) => v+1);
                    else
                        queue.Enqueue((next, value));
                }
            });
        }


        return byRating ? reached.Values.Sum() : reached.Keys.Count;
    }
}
