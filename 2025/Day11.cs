using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day11() : Day(11, 2025)
{
    public override object Part1(List<string> input) => CountPaths(Parse(input), "you", "out", [], [], []);

    public override object Part2(List<string> input) => CountPaths(Parse(input), "svr", "out", [], ["dac", "fft"], []);

    private static Dictionary<string, List<string>> Parse(List<string> input) =>
        input.Select(line => line.Split(": "))
            .ToDictionary(
                parts => parts[0],
                parts => parts[1].Split(' ').ToList());

    private static long CountPaths(
        Dictionary<string, List<string>> graph,
        string current,
        string target,
        HashSet<string> visited,
        HashSet<string> requiredNodes,
        Dictionary<string, long> mem)
    {
        if (current == target)
            return requiredNodes.Count == 0 ? 1 : 0;

        if (!graph.TryGetValue(current, out var outputs) || visited.Contains(current))
            return 0;

        var key = $"{current}:{string.Join(",", requiredNodes.OrderBy(x => x))}";
        if (mem.TryGetValue(key, out var cachedResult))
            return cachedResult;

        visited.Add(current);
        var required = requiredNodes.Remove(current);

        var count = outputs.Sum(next => CountPaths(graph, next, target, visited, requiredNodes, mem));

        visited.Remove(current);
        if (required)
            requiredNodes.Add(current);

        mem[key] = count;
        return count;
    }
}