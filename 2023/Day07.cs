using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day07 : Day
{
    public Day07() : base(7, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        return GetScore(input, true);
    }

    public override object Part2(List<string> input)
    {
        return GetScore(input, false);
    }

    private static int GetScore(List<string> input, bool isPart1)
    {
        var hands = input.Select(i => Hand.Create(i, isPart1))
            .OrderByDescending(h => h.TypeStrength)
            .ThenByDescending(h => h.CardStrengthAt(0))
            .ThenByDescending(h => h.CardStrengthAt(1))
            .ThenByDescending(h => h.CardStrengthAt(2))
            .ThenByDescending(h => h.CardStrengthAt(3))
            .ThenByDescending(h => h.CardStrengthAt(4))
            .ToList();
            return  hands.Select((h, i) => h.Score(hands.Count - i)).Sum();
    }

    private sealed class Hand(char[] cards, int bid, bool part1)
    {
        private readonly char[] cards = cards;
        private readonly int bid = bid;

        public int TypeStrength { get; } = GetTypeStrength(cards, part1);
        public int CardStrengthAt(int pos) => GetCardStrength(cards[pos], part1);
        public int Score(int rank) => rank * bid;

        public static Hand Create(string input, bool part1)
        {
            var parts = input.Split(" ");
            var cards = parts[0].ToCharArray();
            var bid = int.Parse(parts[1]);
            return new Hand(cards, bid, part1);
        }


        public static int GetCardStrength(char c, bool part1) => c switch
        {
            '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' => c - '0',
            'T' => 10,
            'J' => part1? 11 : 1,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => 0,
        };

        public static int GetTypeStrength(char[] cards, bool part1)
        {
            var jokers = cards.Count(c => c == 'J');
            var groups = 
                cards.GroupBy(c => c).OrderByDescending(g => part1 ? g.Count() : g.Key == 'J' ? g.Count() : g.Count() + jokers).ThenByDescending(g => GetCardStrength(g.Key, part1));

            if(groups.Count() == 1)
                return GetTypeStrength(cards.Length, 0);

            var twoBestGroups = groups.Take(2).Select(g => g.ToList()).ToList();
            if(!part1 && jokers > 0)
            {
                var jokersInBest = twoBestGroups[0].Count(c => c == 'J');            
                var jokersInSecondBest = twoBestGroups[1].Count(c => c == 'J');
                
                if(jokersInSecondBest > 0)
                {
                    var (first, second) = GetGroupCounts(twoBestGroups[0], twoBestGroups[1], jokersInSecondBest);
                    return GetTypeStrength(first, second);
                }
                else if(jokersInBest > 0)
                {
                    var (first, second) = GetGroupCounts(twoBestGroups[1], twoBestGroups[0], jokersInBest);
                    return GetTypeStrength(first, second);
                }
                else
                {
                    var first = twoBestGroups[0].Count+jokers;
                    var second = twoBestGroups[1].Count;
                    return GetTypeStrength(first, second);
                }


            }
            return GetTypeStrength(twoBestGroups[0].Count, twoBestGroups[1].Count);

            static (int first, int second) GetGroupCounts(List<char> primary, List<char> secondary, int jokerCountInSecondary)
            {
                var first = primary.Count + jokerCountInSecondary;
                var second = secondary.Count - jokerCountInSecondary;
                return (first, second);
            }
        }

        public static int GetTypeStrength(int c1, int c2) => c1 switch
        {
            5 => 8,
            4 => 7,
            3 => c2 == 2 ? 6 : 5,
            2 => c2 + c1,
            _ => 1,
        };
        
    }
}
