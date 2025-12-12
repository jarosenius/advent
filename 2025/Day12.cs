using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day12() : Day(12, 2025)
{
    public override object Part1(List<string> input)
    {
        var (shapes, regions) = Parse(input, shapeCount: 5, linesPerShape: 6);
        return null;
    }

    public override object Part2(List<string> input)
    {
        return null;
    }

    private static (List<Shape> Shapes, List<Region> Regions) Parse(List<string> input, int shapeCount,
        int linesPerShape)
    {
        var shapeLines = shapeCount * linesPerShape;
        var shapes =
            input.Take(shapeLines)
                .Chunk(linesPerShape)
                .Select(chunk => new Shape([.. chunk.Skip(1).Take(linesPerShape - 2)]))
                .ToList();

        var regions = input.Skip(shapeLines).Select(line =>
        {
            var parts = line.Split(": ");
            var dims = parts[0].SplitByAndParseToInt("x");
            var counts = parts[1].SplitByAndParseToInt(" ").ToList();

            return new Region(dims[0], dims[1], counts);
        }).ToList();

        return (shapes, regions);
    }

    private record Shape(List<string> pattern);

    private record Region(int Width, int Height, List<int> Counts);
}