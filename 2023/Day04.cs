using System;
using System.Linq;
using System.Collections.Concurrent;
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
        var cards = CalculateCards(input).OrderBy(c => c.Id).ToList();
        var result = new ConcurrentDictionary<int, long>();
        cards.ForEach(c => CalculateWinnings(c, cards, result));
        return result.Sum(kvp => kvp.Value);
    }

    private static void CalculateWinnings((int Id, long Value, int TotalWinningNumbers) card, IEnumerable<(int Id, long Value, int TotalWinningNumbers)> cards, ConcurrentDictionary<int, long> result)
    {
        result.AddOrUpdate(card.Id, 1, (key, val) => val+1);
        if(card.TotalWinningNumbers == 0)
            return;
        var copies = cards.Skip(card.Id).Take(card.TotalWinningNumbers).ToList();
        copies.ForEach(c => CalculateWinnings(c, cards, result));
    }

    private static IEnumerable<(int Id, long Value, int TotalWinningNumbers)> CalculateCards(List<string> input)
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
            return (Id: id, Value: value, scratched.Count(s => winning.Contains(s)));
        });
    }
}

