using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2021
{
    [AoC(2021)]
    public class Day06 : Day
    {
        public Day06() : base(6, 2021)
        {

        }

        public override long Part1(List<string> input)
        {
            var lanterns = input.First().SplitByAndParseToInt(",").GroupBy(n => n).ToDictionary(g => g.Key, g => (long)g.Count());
            for (int i = 0; i < 80; i++)
            {
                lanterns = Tick(lanterns);
            }
            return lanterns.Values.Sum();
        }
        public override long Part2(List<string> input)
        {
            var lanterns = input.First().SplitByAndParseToInt(",").GroupBy(n => n).ToDictionary(g => g.Key, g => (long)g.Count());
            for (int i = 0; i < 256; i++)
            {
                lanterns = Tick(lanterns);
            }
            return lanterns.Values.Sum();
        }

        internal static Dictionary<int, long> Tick(Dictionary<int, long> lanterns)
        {
            return new Dictionary<int, long>()
            {
                [8] = lanterns.GetValueOrDefault(0),
                [7] = lanterns.GetValueOrDefault(8),
                [6] = lanterns.GetValueOrDefault(7) + lanterns.GetValueOrDefault(0),
                [5] = lanterns.GetValueOrDefault(6),
                [4] = lanterns.GetValueOrDefault(5),
                [3] = lanterns.GetValueOrDefault(4),
                [2] = lanterns.GetValueOrDefault(3),
                [1] = lanterns.GetValueOrDefault(2),
                [0] = lanterns.GetValueOrDefault(1),
            };
        }
    }
}
