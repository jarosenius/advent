using System.Collections.Generic;
using System.Linq;

namespace Advent.y2022
{
    public class Day04 : Day
    {
        public Day04() : base(4, 2022)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var sections = input
                .Select(row => row.Split(',')
                    .Select(s => s.Split('-')
                    .Select(int.Parse))
                    .Select(s => new Section(s.First(), s.Last())));

            return sections.Count(sec => 
            {
                var f = sec.First();
                var t = sec.Last();

                return f.From <= t.From && t.To <= f.To || 
                        t.From <= f.From && f.To <= t.To;
            });
        }
        public override long Part2(List<string> input)
        {
            return 0;
        }

        record Section(int From, int To);
    }
}
