using System;
using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day08 : Day
    {
        public Day08() : base(8, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var parts = input.Select(x => x.Split("|"));
            var rows = parts.Select(p => (pattern: p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), output: p[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))).ToList();

             return rows.Sum(r => r.output
                .Count(c => 
                    c.Length == 2 || 
                    c.Length == 3 ||
                    c.Length == 4 ||
                    c.Length == 7));
        }
        public override long Part2(List<string> input)
        {
             return 0;
        }
    }
}
