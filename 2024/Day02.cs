using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2024;

[AoC(2024)]
public class Day02 : Day
{
    public Day02() : base(2, 2024)
    {

    }

    public override object Part1(List<string> input)
    {
        var reports = input.Select(r => new Report(r.SplitByAndParseToInt(" "))).ToList();
        return reports.Count(r => r.IsValid(false));
    }

    public override object Part2(List<string> input)
    {
        var reports = input.Select(r => new Report(r.SplitByAndParseToInt(" "))).ToList();
        return reports.Count(r => r.IsValid(true));
    }


    private record Report(int[] Data)
    {
        private readonly bool increasing = Data[1] > Data[0];
        private readonly IEnumerable<(int First, int Second)> pairs = Data.Zip(Data.Skip(1));
        public bool IsValid(bool dampen)
        {
            if(!dampen)
                return pairs.All(p => increasing ? 1 <= p.Second - p.First && p.Second - p.First <= 3 : 1 <= p.First - p.Second && p.First - p.Second <= 3);
            
            return Dampen().Any(r => r.IsValid(false));
        }

        private List<Report> Dampen()
        {
            var reports = new List<Report>();
            for (int i = 0; i <= Data.Length; i++)
            {
                var data = Data.Take(i-1);
                reports.Add(new Report(data.Concat(Data.Skip(i)).ToArray()));
            }
            return reports;
        }
    }
}

