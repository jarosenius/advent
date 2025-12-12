using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day12() : Day(12, 2025)
{
    public override object Part1(List<string> input)
    {
        var (shapes, regions) = Parse(input, shapeCount: 6, linesPerShape: 5);
        return regions.Count(region => CanFitPresents(region, shapes));
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

    private static bool CanFitPresents(Region region, List<Shape> shapes)
    {
        var presents = new List<Shape>();

        for (var i = 0; i < region.Counts.Count && i < shapes.Count; i++)
        {
            for (var j = 0; j < region.Counts[i]; j++)
            {
                presents.Add(shapes[i]);
            }
        }

        var totalArea = presents.Sum(p => p.GetVariants()[0].Points.Count);

        if (totalArea > region.Width * region.Height)
            return false;

        presents.Sort((a, b) =>
        {
            var aVariants = a.GetVariants();
            var bVariants = b.GetVariants();
            var sizeCompare = bVariants[0].Points.Count.CompareTo(aVariants[0].Points.Count);
            return sizeCompare != 0 ? sizeCompare : aVariants.Count.CompareTo(bVariants.Count);
        });

        var grid = new bool[region.Height, region.Width];
        return TryPlacePresents(grid, presents, 0);
    }

    private static bool TryPlacePresents(bool[,] grid, List<Shape> presents, int presentIdx)
    {
        if (presentIdx >= presents.Count)
            return true;

        var shape = presents[presentIdx];
        var variants = shape.GetVariants();

        foreach (var variant in variants)
        {
            var maxRow = grid.GetLength(0) - variant.Height + 1;
            var maxCol = grid.GetLength(1) - variant.Width + 1;

            for (var row = 0; row < maxRow; row++)
            {
                for (var col = 0; col < maxCol; col++)
                {
                    if (!TryPlace(grid, variant, row, col))
                        continue;

                    if (TryPlacePresents(grid, presents, presentIdx + 1))
                        return true;

                    Place(grid, variant, row, col, false);
                }
            }
        }

        return false;
    }

    private static bool TryPlace(bool[,] grid, ShapeVariant variant, int row, int col)
    {
        foreach (var (r, c) in variant.Points)
        {
            if (grid[row + r, col + c])
                return false;
        }
        Place(grid, variant, row, col, true);
        return true;
    }

    private static void Place(bool[,] grid, ShapeVariant variant, int row, int col, bool place)
    {
        foreach (var (r, c) in variant.Points)
        {
            grid[row + r, col + c] = place;
        }
    }

    private record Shape(List<string> Pattern)
    {
        private List<ShapeVariant> _cache;

        public List<ShapeVariant> GetVariants()
        {
            if (_cache != null)
                return _cache;

            var variants = new HashSet<ShapeVariant>(new ShapeVariantComparer());
            var current = Pattern;

            for (var i = 0; i < 4; i++)
            {
                variants.Add(new ShapeVariant(current));
                current = Rotate90(current);
            }

            current = Flip(Pattern);
            for (var i = 0; i < 4; i++)
            {
                variants.Add(new ShapeVariant(current));
                current = Rotate90(current);
            }

            _cache = [.. variants];
            return _cache;
        }

        private static List<string> Rotate90(List<string> pattern)
        {
            var height = pattern.Count;
            var width = pattern[0].Length;
            var res = new List<string>(width);

            for (var col = 0; col < width; col++)
            {
                var chars = new char[height];
                for (var row = 0; row < height; row++)
                {
                    chars[row] = pattern[height - 1 - row][col];
                }
                res.Add(new string(chars));
            }

            return res;
        }
        private static List<string> Flip(List<string> pattern) => [.. pattern.Select(row => new string([.. row.Reverse()]))];
    }

    private record Region(int Width, int Height, List<int> Counts);

    private record ShapeVariant
    {
        public List<(int Row, int Col)> Points { get; }
        public int Height { get; }
        public int Width { get; }

        public ShapeVariant(List<string> pattern)
        {
            Points = [];
            Height = pattern.Count;
            Width = pattern[0].Length;

            for (var r = 0; r < pattern.Count; r++)
            {
                for (var c = 0; c < pattern[r].Length; c++)
                {
                    if (pattern[r][c] == '#')
                        Points.Add((r, c));
                }
            }
        }
    }

    private record ShapeVariantComparer : IEqualityComparer<ShapeVariant>
    {
        public bool Equals(ShapeVariant x, ShapeVariant y)
        {
            if (x.Width != y.Width || x.Height != y.Height || x.Points.Count != y.Points.Count)
                return false;

            return x.Points.SequenceEqual(y.Points);
        }

        public int GetHashCode(ShapeVariant obj)
        {
            var hash = new HashCode();
            hash.Add(obj.Width);
            hash.Add(obj.Height);
            foreach (var point in obj.Points)
                hash.Add(point);
            return hash.ToHashCode();
        }
    }
}