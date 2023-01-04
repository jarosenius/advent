using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day10 : Day
    {
        public Day10() : base(10, 2020)
        {

        }

        public override object Part1(List<string> input)
        {
            var list = input.Select(int.Parse);

            return GetAdapters(list, 0, list.Max() + 3, 3, 0, 0);
        }
        public override object Part2(List<string> input)
        {
            var maxDiff = 3;
            var list = new[] { 0 }.Concat(input.Select(int.Parse));
            var target = list.Max() + maxDiff;
            list = list.OrderBy(i => i).Reverse();
            Console.Clear();

            return GetCombinations(list, maxDiff, target);
        }

        private static int GetAdapters(IEnumerable<int> list, int start, int target, int maxDiff, int oneDiff, int threeDiff)
        {
            if (list.Any() == false)
            {
                var d = target - start;
                return (oneDiff + (d == 1 ? 1 : 0)) * (threeDiff + (d == 3 ? 1 : 0));
            }
            var selectedAdapters = list.Where(i => i >= start && i <= start + maxDiff);
            var selectedAdapter = selectedAdapters.Min();
            var diff = selectedAdapter - start;
            var oD = oneDiff;
            var tD = threeDiff;
            if (diff == 1)
                oD++;
            else if (diff == 3)
                tD++;

            return GetAdapters(list.Except(new[] { selectedAdapter }), selectedAdapter, target, maxDiff, oD, tD);
        }

        private static long GetCombinations(IEnumerable<int> list, int maxDiff, int target)
        {
            var adapterCombinations = new Dictionary<int, long>();
            adapterCombinations.Add(target, 1);
            var diffs = Enumerable.Range(1, maxDiff).ToList();
            foreach (var adapter in list)
            {
                var combinations = 0L;
                diffs.ForEach(d => { if (adapterCombinations.TryGetValue(adapter + d, out long comb)) combinations += comb; });
                adapterCombinations[adapter] = combinations;
            }

            return adapterCombinations[0];
        }
    }
}
