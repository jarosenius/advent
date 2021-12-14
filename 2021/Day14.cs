using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2021
{
    public class Day14 : Day
    {
        public Day14() : base(14, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var template = input.First();
            var pairInserts = input.Skip(2).Select(p => p.Split(" -> ")).ToDictionary(p => p[0], p => p[1]);
            Enumerable.Range(0, 10).ForEach(i => template = ApplyInsertion(template, pairInserts));
            var groups = template.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
            
            return groups.Values.Max() - groups.Values.Min();
        }
        public override long Part2(List<string> input)
        {
            return 0;
        }

        private string ApplyInsertion(string template, Dictionary<string, string> inserts)
        {
            List<(string Text, int At)> toInsert2 = new();
            var pairs = template.Zip(template.Skip(1)).Select(p => $"{p.First}{p.Second}");
            var num = 0;
            var toInsert = pairs.Select((p, i) => (Text: inserts[p], At: i+1+(num++)));
            return toInsert.Aggregate(template, (ac, i) => ac.Insert(i.At, i.Text));
        }
    }
}
