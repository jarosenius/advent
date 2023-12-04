using System;
using System.Linq;
using System.Collections.Generic;

namespace Advent.y2023;

[AoC(2023)]
public class Day04 : Day
{
    public Day04() : base(4, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        return CalculateCards(input).Sum(g => g.Value);
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }

    private IEnumerable<(int Id, long Value)> CalculateCards(List<string> input)
    {
        return input.Select(row =>
        {
            var parts = row.Split(":");
            var id = int.Parse(parts[0].Replace(" ", "")[4..]);
            var numbers = parts[1].Split(" | ").ToArray();
            var scratched = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
            var winning = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).ToHashSet();
            var value = scratched.Aggregate(0L, (total, num) => {
                if(!winning.Contains(num))
                    return total;
                if(total == 0)
                    return 1;
                return total*2;
            });
            return (Id: id, Value: value);
        });
    }
}

