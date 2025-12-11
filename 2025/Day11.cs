using System.Collections.Generic;
using System.Linq;
using Advent.Common;
using MoreLinq;

namespace Advent.y2025;

[AoC(2025)]
public class Day11() : Day(11, 2025)
{
    public override object Part1(List<string> input) => CountPaths(Parse(input), "you", "out", []);

    public override object Part2(List<string> input)
    {
        return null;
    }

    private static Dictionary<string, List<string>> Parse(List<string> input) =>
        input.Select(line => line.Split(": "))
            .ToDictionary(
                parts => parts[0],
                parts => parts[1].Split(' ').ToList());

    private static int CountPaths(Dictionary<string, List<string>> graph, string current, string target, HashSet<string> visited)
    {
        if (current == target)
            return 1;

        if (!(graph.TryGetValue(current, out var value) && !visited.Contains(current)))
            return 0;

        visited.Add(current);

        var count = value.Sum(next => CountPaths(graph, next, target, visited));
        visited.Remove(current);

        return count;
    }
}