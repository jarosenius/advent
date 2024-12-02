using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    [AoC(2020)]
    public class Day03() : Day(3, 2020)
    {
        public override object Part1(List<string> input)
        {
            return CountTrees(input, 3, 1); ;
        }
        public override object Part2(List<string> input)
        {
            return CountTrees(input, 1, 1)
           * CountTrees(input, 3, 1)
           * CountTrees(input, 5, 1)
           * CountTrees(input, 7, 1)
           * CountTrees(input, 1, 2); ;
        }

        long CountTrees(IEnumerable<string> lines, int right, int down)
        {
            var trees = 0;

            for (var i = 0; i < lines.Count(); i++)
            {
                var line = lines.ElementAt(i);
                var width = line.Count();
                var idx = ((i / down) * right) % width;
                if ((down == 1 || i % down == 0) && line.ElementAt(idx) == '#')
                    trees++;
            }
            return trees;
        }
    }
}
