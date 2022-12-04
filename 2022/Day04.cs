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
            return GetSections(input).Count(sec => 
            {
                var f = sec.First();
                var t = sec.Last();

                return f.From <= t.From && t.To <= f.To || 
                        t.From <= f.From && f.To <= t.To;
            });
        }
        public override long Part2(List<string> input)
        {
            return GetSections(input).Count(sec => 
            {
                var f = sec.First();
                var t = sec.Last();

                return f.To >= t.From && f.From <= t.To || 
                        t.To >= f.From && t.From <= f.To;
            });
        }

        private IEnumerable<IEnumerable<Section>> GetSections(IEnumerable<string> input) 
        {
           return input
                .Select(row => row.Split(',')
                    .Select(s => s.Split('-')
                    .Select(int.Parse))
                    .Select(s => new Section(s.First(), s.Last())));
        }

        record Section(int From, int To);
    }
}
