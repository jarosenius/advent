using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MoreLinq.Extensions;

namespace Advent.y2023;

[AoC(2023)]
public class Day07 : Day
{
    public Day07() : base(7, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var hands = input.Select(Hand.Create)
            .OrderByDescending(h => h.TypeStrength)
            .ThenByDescending(h => h.CardStrengthAt(0))
            .ThenByDescending(h => h.CardStrengthAt(1))
            .ThenByDescending(h => h.CardStrengthAt(2))
            .ThenByDescending(h => h.CardStrengthAt(3))
            .ThenByDescending(h => h.CardStrengthAt(4))
            .ToList();

        return hands.Select((h, i) => h.Score(hands.Count-i)).Sum();
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }

    private sealed class Hand(char[] cards, int bid)
    {
        private readonly char[] cards = cards;
        private readonly int bid = bid;

        public int TypeStrength { get; } = GetTypeStrength(cards);
        public int CardStrengthAt(int pos) => GetCardStrength(cards[pos]);
        public int Score(int rank) => rank * bid;

        public static Hand Create(string input)
        {
            var parts = input.Split(" ");
            var cards = parts[0].ToCharArray();
            var bid = int.Parse(parts[1]);
            return new Hand(cards, bid);
        }


        public static int GetCardStrength(char c) => c switch
        {
            '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' => c - '0',
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => 0,
        };

        public static int GetTypeStrength(char[] cards)
        {
            var groups = 
                cards.GroupBy(c => c).OrderByDescending(g => g.Count());

            if(groups.Count() == 1)
                return GetTypeStrength([.. groups.First()], []);

            var twoBestGroups = groups.Take(2).ToArray();
            return GetTypeStrength([.. twoBestGroups[0]], [.. twoBestGroups[1]]);
        }

        public static int GetTypeStrength(char[] c1, char[] c2) => c1.Length switch
        {
            5 => 8,
            4 => 7,
            3 => c2.Length == 2 ? 6 : 5,
            2 => c2.Length + c1.Length,
            _ => 1,
        };
        
    }
}
