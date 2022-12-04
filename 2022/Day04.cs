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
                var r1 = sec.First();
                var r2 = sec.Last();

                return r1.From <= r2.From && r2.To <= r1.To || 
                        r2.From <= r1.From && r1.To <= r2.To;
            });
        }
        public override long Part2(List<string> input)
        {
            return GetSections(input).Count(sec => 
            {
                var r1 = sec.First();
                var r2 = sec.Last();

                return r1.To >= r2.From && r1.From <= r2.To || 
                        r2.To >= r1.From && r2.From <= r1.To;
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
