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
        return CalculateCards(input).Sum(g => Math.Pow(2, g.TotalWinningNumbers-1));
    }
    public override object Part2(List<string> input)
    {
        var cards = CalculateCards(input).OrderBy(c => c.Id).ToList();
        var result = cards.ToDictionary(kvp => kvp.Id, _ => 1);
        foreach (var card in cards)
        {
            var (Id, Counts) = (cards[card.Id-1], result[card.Id]);
            for (int j = 0; j < card.TotalWinningNumbers; j++)
            {
                result[card.Id+j+1] += Counts;
            }
        }
        return result.Sum(kvp => kvp.Value);
    }

    private static IEnumerable<(int Id, int TotalWinningNumbers)> CalculateCards(List<string> input)
    {
        return input.Select(row =>
        {
            var parts = row.Split(':', '|');
            var id = int.Parse(parts[0].Replace(" ", "")[4..]);
            var scratched = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToHashSet();
            var winning = parts[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim()).ToHashSet();
            return (Id: id, scratched.Count(s => winning.Contains(s)));
        });
    }
}

