using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day05() : Day(5, 2024)
{
    public override object Part1(List<string> input)
    {
        var rules = input.TakeWhile(r => r != string.Empty).Select(r => r.SplitByAndParseToInt("|")).ToList();
        var updates = input.Skip(rules.Count+1).Select(r => r.SplitByAndParseToInt(",")).ToList();
        return updates.Aggregate(0, (sum, update) => 
            InOrder(update, rules) 
            ? sum + MidValue(update) 
            : sum);
    }
    public override object Part2(List<string> input)
    {
        var rules = input.TakeWhile(r => r != string.Empty).Select(r => r.SplitByAndParseToInt("|")).ToList();
        var updates = input.Skip(rules.Count+1).Select(r => r.SplitByAndParseToInt(",")).ToList();
        return updates.Aggregate(0, (sum, update) => 
            InOrder(update, rules) 
            ? sum 
            : sum + MidValue(ReOrder(update, rules)));
    }

    private static bool InOrder(int[] update, List<int[]> rules)
    {
        var pos = update
            .Select((page, i) => new {Page = page, Index = i})
            .ToDictionary(kvp => kvp.Page, kvp => kvp.Index);
        return rules.All(r => !(pos.TryGetValue(r[0], out var a) && pos.TryGetValue(r[1], out var b) && a > b));
    }

    private static List<int> ReOrder(int[] update, List<int[]> rules)
    {
        var graph = update.ToDictionary(page => page, page => new List<int>());
        var depth = update.ToDictionary(page => page, page => 0);

        rules.Where(rule => update.Contains(rule[0]) && update.Contains(rule[1]))
            .ToList()
            .ForEach(rule =>
            {
                graph[rule[0]].Add(rule[1]);
                depth[rule[1]]++;
            });

        var sorted = new List<int>();
        var queue = new Queue<int>(depth.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key));

        while (queue.TryDequeue(out var current))
        {
            sorted.Add(current);
            graph[current].ForEach(n =>
            {
                depth[n]--;
                if (depth[n] == 0)
                    queue.Enqueue(n);
            });
        }

        return sorted;
    }
    
    private static int MidValue(IEnumerable<int> list) => list.ElementAt(list.Count() / 2);
}

