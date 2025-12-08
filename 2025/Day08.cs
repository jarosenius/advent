using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2025;

[AoC(2025)]
public class Day08() : Day(8, 2025)
{
    public override object Part1(List<string> input)
    {
        var junctionBoxes = input.Select(Coordinate3D.Parse).ToList();
        var connections = UsedExampleData ? 10 : 1000;
        var pairs = GetSorted(junctionBoxes);
        var uf = new UnionFind(junctionBoxes.Count);

        for (var i = 0; i < connections && i < pairs.Count; i++)
        {
            var (idx1, idx2, _) = pairs[i];
            uf.Union(idx1, idx2);
        }

        var circuitSizes = uf.GetComponentSizes()
            .OrderByDescending(size => size)
            .Take(3)
            .ToList();

        return circuitSizes[0] * circuitSizes[1] * circuitSizes[2];
    }
    public override object Part2(List<string> input)
    {
        return null;
    }

    private static List<(int Index1, int Index2, double Distance)> GetSorted(List<Coordinate3D> junctionBoxes) =>
        [.. junctionBoxes
            .SelectMany((jbox1, i) => junctionBoxes
                .Skip(i + 1)
                .Select((jbox2, j) => (i, j: i + j + 1, jbox1.EuclideanDistance(jbox2))))
            .OrderBy(pair => pair.Item3)];
}

