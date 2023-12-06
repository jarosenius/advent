using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day05 : Day
{
    public Day05() : base(5, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var (map, seeds) = CreateMapAndSeeds(input, i => i.Select(v => new Range(v, v)));
        var ranges = map.Get(seeds);

        return ranges.Min(r => r.From);
    }

    public override object Part2(List<string> input)
    {
        var (map, seeds) = CreateMapAndSeeds(input, i => i.Chunk(2).Select(v => new Range(v[0], v[0] + v[1]-1)));
        var ranges = map.Get(seeds);

        return ranges.Min(r => r.From);
    }

    private static (Map Map, List<Range> Seeds) CreateMapAndSeeds(List<string> input, Func<ulong[], IEnumerable<Range>> createSeedRange)
    {
        var seeds = createSeedRange(input[0][6..].SplitByAndParse(" ", ulong.Parse)).ToList();
        var groups = input.Skip(2).GroupWhile(r => r != "");
        var maps = groups.Select(g =>
        {
            var name = g.ElementAt(0).Split(new[] { " map:", "-" }, StringSplitOptions.RemoveEmptyEntries).Last();
            var ranges = g.Skip(1).Select(r =>
            {
                var values = r.SplitByAndParse(" ", ulong.Parse);
                return new MapRange(new Range(values[0], values[0] + values[2]-1), new Range(values[1], values[1]+values[2]-1), values[0]-values[1]);
            });
            return new Map(name, ranges.ToList());
        }).ToList();
        for (int i = 0; i < maps.Count - 1; i++)
        {
            var cMap = maps[i];
            var nMap = maps[i + 1];
            cMap.SetNextMap(nMap);
        }
        return(maps.First(m => m.Name == "soil"), seeds);
    }

    private class Map(string Name, List<MapRange> ranges)
    {
        private Map next;

        public List<MapRange> Ranges {get;} = ranges.OrderBy(r => r.Source.From).ToList();

        public string Name { get; } = Name;

        public void SetNextMap(Map map)
        {
            next = map;
        }

        public List<Range> Get(List<Range> seeds)
        {
            var res = new List<Range>();
            foreach (var seed in seeds)
            {
                var current = seed;
                var done = false;

                do
                {
                    var range = Ranges.LastOrDefault(r => r.Source.From <= current.From && current.From <= r.Source.To);
                    if(range == null)
                    {
                        range = Ranges.LastOrDefault(r => r.Source.From <= current.To && current.To <= r.Source.To);
                        if(range == null)
                        {
                            res.Add(current);
                            done = true;
                        }
                        else
                        {
                            res.Add(new Range(current.From, range.Source.From-1));
                            current = new Range(range.Source.From, range.Source.To);
                        }
                    }
                    else if (range.Source.To >= current.To)
                    {
                        res.Add(new Range(current.From + range.Offset, current.To + range.Offset));
                        done = true;
                    }
                    else
                    {
                        res.Add(new Range(current.From + range.Offset, range.Source.To + range.Offset));
                        current = new Range(range.Source.To + 1, current.To);
                    }
                } while (!done);
            }
            
            if(next != null)
                return next.Get(res);

            return res;
        }
    }
    private record MapRange(Range Destination, Range Source, ulong Offset);
    private record Range(ulong From, ulong To);

}

