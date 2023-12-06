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
        var (maps, seeds) = CreateMapAndSeeds(input);
        var map = maps.ElementAt(0);

        return seeds.Min(s => map.Get("location", s));
    }

    public override object Part2(List<string> input)
    {
        return 0;
    }

    private static (List<Map> Maps, long[] Seeds) CreateMapAndSeeds(List<string> input)
    {
        var seeds = input[0][6..].SplitByAndParseToLong(" ");
        var groups = input.Skip(2).GroupWhile(r => r != "");
        var maps = groups.Select(g =>
        {
            var name = g.ElementAt(0).Split(new[] { " map:", "-" }, StringSplitOptions.RemoveEmptyEntries).Last();
            var ranges = g.Skip(1).Select(r =>
            {
                var values = r.SplitByAndParseToLong(" ");
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
        return(maps, seeds);
    }

    private record Map(string Name, List<MapRange> Ranges)
    {
        private Map next;

        public void SetNextMap(Map map)
        {
            next = map;
        }
        
        public long Get(string name, long value)
        {
            var target = value;
            var range = Ranges.FirstOrDefault(r => value >= r.Source.From && value <= r.Source.To);
            if(range != null)
                target = range.Offset + value;

            if(name == Name)
                return target;

            if(next != null)
                return next.Get(name, target);

            return 0;
        }
    }
    private record MapRange(Range Destination, Range Source, long Offset);
    private record Range(long From, long To);

}

