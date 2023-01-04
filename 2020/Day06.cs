using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day06 : Day
    {
        public Day06() : base(6, 2020)
        {

        }

        private IEnumerable<IEnumerable<string>> GetGroups(IEnumerable<string> input)
        {
            var groups = new List<List<string>>();
            var group = new List<string>();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    groups.Add(group);
                    group = new List<string>();
                    continue;
                }
                group.Add(line);
            }
            groups.Add(group);
            return groups;
        }
        public override object Part1(List<string> input)
        {
            var groups = GetGroups(input);
            var count = 0;
            foreach (var group in groups)
            {
                count += group.SelectMany(s => s.ToCharArray()).ToList().Distinct().Count();
            }

            return count;
        }
        public override object Part2(List<string> input)
        {
            var groups = GetGroups(input);
            var count = 0;
            foreach (var group in groups)
            {
                var distinctYes = group.SelectMany(s => s.ToCharArray()).ToList().Distinct().ToList();
                foreach (var yes in distinctYes)
                {
                    if (group.All(g => g.Contains(yes)))
                        count++;
                }
            }

            return count;
        }
    }
}
