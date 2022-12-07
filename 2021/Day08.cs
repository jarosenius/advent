using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Advent.y2021
{
    [AoC(2021)]
    public class Day08 : Day
    {
        public Day08() : base(8, 2021)
        {

        }

        public override long Part1(List<string> input)
        {
            var parts = input.Select(x => x.Split("|"));
            var rows = parts.Select(p => (pattern: p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), output: p[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))).ToList();

            return rows.Sum(r => r.output.Count(c => c.Length is 7 or > 1 and < 5));
        }
        public override long Part2(List<string> input)
        {
            var parts = input.Select(x => x.Split("|"));
            var rows = parts.Select(p => (pattern: p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), output: p[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))).ToList();

            return rows.Sum(r =>
            {
                var d = new Dictionary<int, string>();

                d[1] = r.pattern.First(p => p.Length == 2);
                d[4] = r.pattern.First(p => p.Length == 4);
                d[5] = r.pattern.First(p => p.Length == 5
                    && d[4].Except(d[1]).All(p.Contains));
                d[0] = r.pattern.First(p => p.Length == 6
                    && d[5].All(p.Contains) == false
                    && d[4].All(p.Contains) == false);
                d[2] = r.pattern.First(p => p.Length == 5
                    && d[1].All(p.Contains) == false
                    && d[4].Except(d[1]).All(p.Contains) == false);
                d[3] = r.pattern.First(p => p.Length == 5
                    && d[1].All(p.Contains));
                d[6] = r.pattern.First(p => p.Length == 6
                    && d[5].All(p.Contains)
                    && d[4].All(p.Contains) == false);
                d[7] = r.pattern.First(p => p.Length == 3);
                d[8] = r.pattern.First(p => p.Length == 7);
                d[9] = r.pattern.First(p => p.Length == 6
                    && d[4].All(p.Contains)
                    && d[5].All(p.Contains));

                return int.Parse(string.Join("", r.output.Select(o => d.First(i => i.Value.All(o.Contains) && o.Length == i.Value.Length).Key)));
            });
        }


    }
}
