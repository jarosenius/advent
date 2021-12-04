using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day07 : Day
    {
        public Day07() : base(7, 2020)
        {
            
        }

        public override long Part1(IEnumerable<string> input)
        {
            var rules = new List<Tuple<string, List<string>>>();
            foreach (var rawRule in input)
            {
                var parts = rawRule.Split(" bags contain ");
                var color = parts[0];
                var containsColor = new List<string>();

                if (parts[1].Contains("no other bags") == false)
                {
                    var cleaned = parts[1].Replace(".", string.Empty).Replace(" bags", string.Empty).Replace(" bag", string.Empty).Split(',');
                    foreach (var clean in cleaned)
                    {
                        var c = clean.Substring(clean.Trim().IndexOf(" ") + 1).Trim();
                        containsColor.Add(c.Trim());
                    }
                }

                rules.Add(Tuple.Create(color, containsColor));
            }

            
            return GetNumberOfBagsThatFitColor("shiny gold", rules);
        }
        public override long Part2(IEnumerable<string> input)
        {
            var rules = new List<Tuple<string, List<string>>>();
            foreach (var rawRule in input)
            {
                var parts = rawRule.Split(" bags contain ");
                var color = parts[0];
                var containsColor = new List<string>();

                if (parts[1].Contains("no other bags") == false)
                {
                    var cleaned = parts[1].Replace(".", string.Empty).Replace(" bags", string.Empty).Replace(" bag", string.Empty).Split(',');
                    foreach (var clean in cleaned)
                    {
                        containsColor.Add(clean.Trim());
                    }
                }

                rules.Add(Tuple.Create(color, containsColor));
            }

            
            return GetBagsRequiredForRule("shiny gold", rules)-1;
        }

        static int GetBagsRequiredForRule(string ruleName, List<Tuple<string, List<string>>> rules)
        {
            var rule = rules.First(r => r.Item1 == ruleName);
            if (rule == null || rule.Item2.Any() == false)
                return 1;

            var count = 1;
            foreach(var sub in rule.Item2)
            {
                var splitPos = sub.IndexOf(" ");
                var snumber = sub.Substring(0, splitPos + 1).Trim();
                var number = int.Parse(snumber);
                var color = sub.Substring(splitPos).Trim();
                count += number * GetBagsRequiredForRule(color, rules);
            }
            return count;
        }

        static int GetBagsInBagWithColor(string color)
        {
            switch (color)
            {
                case "dotted black":
                    return 0;
                case "faded blue":
                    return 0;
                case "light red":
                    return GetBagsInBagWithColor("bright white") + 2 * GetBagsInBagWithColor("muted yellow");
                case "dark orange":
                    return 3 * GetBagsInBagWithColor("bright white") + 4 * GetBagsInBagWithColor("muted yellow");
                case "bright white":
                    return GetBagsInBagWithColor("shiny gold");
                case "muted yellow":
                    return 2 * GetBagsInBagWithColor("shiny gold") + 9 * GetBagsInBagWithColor("faded blue");
                case "shiny gold":
                    return GetBagsInBagWithColor("dark olive") + 2 * GetBagsInBagWithColor("vibrant plum");
                case "dark olive":
                    return 3 * GetBagsInBagWithColor("faded blue") + 4 * GetBagsInBagWithColor("dotted black");
                case "vibrant plum":
                    return 5*GetBagsInBagWithColor("faded blue") + 6 * GetBagsInBagWithColor("dotted black");
            }
            return 0;
        }

        static int GetNumberOfBagsThatFitColor(string color, List<Tuple<string, List<string>>> rules)
        {
            var count = 0;
            foreach(var bag in rules.Select(r => r.Item1))
            {
                if(CanBagFitColor(bag, color, rules))
                {
                    count++;
                }
            }
            return count;
        }

        static bool CanBagFitColor(string bag, string color, List<Tuple<string, List<string>>> rules)
        {
            var bagRule = rules.FirstOrDefault(r => r.Item1 == bag);
            if (bagRule == null || bagRule.Item2.Any() == false)
                return false;
            if (bagRule.Item2.Contains(color))
                return true;
            return bagRule.Item2.Any(c => CanBagFitColor(c, color, rules));
        }
    }
}
