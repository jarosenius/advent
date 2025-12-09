using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2025;

[AoC(2025)]
public class Day09() : Day(9, 2025)
{
    public override object Part1(List<string> input) => GetLargestRectangle(GetCoordinates(input));

    public override object Part2(List<string> input)
    {
        var coordinates = GetCoordinates(input);
        var polygon = Polygon.Create(coordinates);
        var ranges = GetRanges(coordinates, polygon);

        return GetLargestRectangle(coordinates, ranges, polygon);
    }

    private static List<Coordinate> GetCoordinates(List<string> input) =>
        [.. input.Select(row => row.SplitByAndParseToInt(",")).Select(parts => new Coordinate(parts[0], parts[1]))];
    
    private static long GetLargestRectangle(List<Coordinate> coordinates)
    {
        long max = 0;

        for (var i = 0; i < coordinates.Count; i++)
        {
            var c1 = coordinates[i];

            for (var j = i + 1; j < coordinates.Count; j++)
            {
                var c2 = coordinates[j];
                var area = (long)(Math.Abs(c2.X - c1.X) + 1) * (Math.Abs(c2.Y - c1.Y) + 1);

                if (area > max)
                    max = area;
            }
        }

        return max;
    }

    private static long GetLargestRectangle(
        List<Coordinate> redTiles,
        Dictionary<int, List<(int x1, int x2)>> validRanges,
        Polygon polygon)
    {
        long max = 0;
        var candidates = GetCandidates(redTiles);
        var tested = new HashSet<(int, int, int, int)>();
        var ranges = new Dictionary<int, List<(int x1, int x2)>>();
        var cache = new Dictionary<string, bool>();

        foreach (var (area, minX, maxX, minY, maxY) in candidates)
        {
            if (area <= max) break;

            var bounds = (minX, maxX, minY, maxY);
            if (!tested.Add(bounds))
                continue;

            if (IsValid(minX, maxX, minY, maxY, redTiles, validRanges, ranges, polygon, cache))
                max = area;
        }

        return max;
    }

    private static List<(long area, int minX, int maxX, int minY, int maxY)> GetCandidates(List<Coordinate> redTiles)
        =>
        [
            .. redTiles.SelectMany((c1, i) => redTiles.Skip(i + 1).Select(c2 =>
            {
                var minX = Math.Min(c1.X, c2.X);
                var maxX = Math.Max(c1.X, c2.X);
                var minY = Math.Min(c1.Y, c2.Y);
                var maxY = Math.Max(c1.Y, c2.Y);
                var area = (long)(maxX - minX + 1) * (maxY - minY + 1);

                return (area, minX, maxX, minY, maxY);
            })).OrderByDescending(c => c.area)
        ];


    private static Dictionary<int, List<(int x1, int x2)>> GetRanges(
        List<Coordinate> redTiles, 
        Polygon polygon)
    {
        var cache = new Dictionary<string, bool>();
        return polygon.Ys.Aggregate(new Dictionary<int, List<(int x1, int x2)>>(), (valid, y) =>
        {
            var ranges = GetYRanges(y, redTiles, polygon, cache);
            if (ranges.Count > 0)
                valid[y] = ranges;
            return valid;
        });
    }

    private static List<(int x1, int x2)> GetYRanges(
        int y, 
        List<Coordinate> redTiles, 
        Polygon polygon,
        Dictionary<string, bool> cache)
    {
        var ranges = new List<(int x1, int x2)>();
        polygon.Horizontal.TryGetValue(y, out var boundary);
        if (boundary != null)
            ranges.AddRange(boundary);
        
        var sorted = GetSortedCrossings(y, polygon, boundary);
        if (sorted.Count == 0)
            return ranges;
        
        sorted.Zip(sorted.Skip(1)).ForEach(pair =>
        {
            var (x1, x2) = pair;
            if (boundary != null && IsIntervalCoveredByBoundary(x1, x2, boundary))
                return;

            if (IsInside(y, redTiles, cache, x1, x2))
                ranges.Add((x1, x2));
        });

        return MergeRanges(ranges);
    }

    private static List<int> GetSortedCrossings(int y, Polygon polygon, List<(int x1, int x2)> boundary)
    {
        var crossings = polygon.Vertical.Aggregate(new HashSet<int>(), (set, v) =>
        {
            if (y >= v.y1 && y <= v.y2)
                set.Add(v.x); return set;
        });

        boundary?.ForEach(b =>
        {
            crossings.Add(b.x1);
            crossings.Add(b.x2);
        });
        
        return [.. crossings.OrderBy(x => x)];
    }

    private static bool IsInside(int y, List<Coordinate> redTiles, Dictionary<string, bool> cache, int x1, int x2)
    {
        var midX = (x1 + x2) / 2;
        var key = $"{midX},{y}";

        if (cache.TryGetValue(key, out var inside)) 
            return inside;
        
        return cache[key] = InPolygon(midX, y, redTiles);
    }

    private static bool IsIntervalCoveredByBoundary(int x1, int x2, List<(int x1, int x2)> boundary)
        => boundary.Any(b => x1 >= b.x1 && x2 <= b.x2);


    private static List<(int x1, int x2)> MergeRanges(List<(int x1, int x2)> ranges)
    {
        if (ranges.Count == 0)
            return ranges;
            
        ranges.Sort();
        var merged = new List<(int x1, int x2)> { ranges[0] };

        ranges.ForEach(r =>
        {
            var (x1, x2) = merged[^1];
            if (r.x1 <= x2 + 1)
                merged[^1] = (x1, Math.Max(x2, r.x2));
            else
                merged.Add(r);
        });

        return merged;
    }

    private static bool InPolygon(int x, int y, List<Coordinate> coordinates)
    {
        var inside = false;
        var count = coordinates.Count;

        for (var i = 0; i < count; i++)
        {
            var j = (i + 1) % count;
            var pi = coordinates[i];
            var pj = coordinates[j];

            if (pi.Y > y != (pj.Y > y) && x < (pj.X - pi.X) * (y - pi.Y) / (pj.Y - pi.Y) + pi.X)
                inside = !inside;
        }

        return inside;
    }

    private static bool IsValid(
        int minX, int maxX, int minY, int maxY,
        List<Coordinate> redTiles,
        Dictionary<int, List<(int x1, int x2)>> validRanges,
        Dictionary<int, List<(int x1, int x2)>> computedRanges,
        Polygon polygon,
        Dictionary<string, bool> cache)
    {
        if (maxY - minY + 1 > 10 && !ValidateSampleYs(minX, maxX, redTiles, validRanges, computedRanges, polygon, cache, [minY, (minY + maxY) / 2, maxY]))
            return false;

        return ValidateAllYs(minX, maxX, minY, maxY, redTiles, validRanges, computedRanges, polygon, cache);
    }

    private static bool ValidateSampleYs(
        int minX, int maxX,
        List<Coordinate> redTiles,
        Dictionary<int, List<(int x1, int x2)>> validRanges,
        Dictionary<int, List<(int x1, int x2)>> computedRanges,
        Polygon polygon,
        Dictionary<string, bool> cache,
        List<int> samples) => 
            !samples
                .Select(y => GetYRanges(y, redTiles, validRanges, computedRanges, polygon, cache))
                .Any(ranges => ranges == null || !Covered(minX, maxX, ranges));

    private static bool ValidateAllYs(
        int minX, int maxX, int minY, int maxY,
        List<Coordinate> redTiles,
        Dictionary<int, List<(int x1, int x2)>> validRanges,
        Dictionary<int, List<(int x1, int x2)>> computedRanges,
        Polygon polygon,
        Dictionary<string, bool> cache)
    {
        for (var y = minY; y <= maxY; y++)
        {
            var ranges = GetYRanges(y, redTiles, validRanges, computedRanges, polygon, cache);
            if (ranges == null || !Covered(minX, maxX, ranges))
                return false;
        }

        return true;
    }

    private static List<(int x1, int x2)> GetYRanges(
        int y,
        List<Coordinate> redTiles,
        Dictionary<int, List<(int x1, int x2)>> validRanges,
        Dictionary<int, List<(int x1, int x2)>> ranges,
        Polygon polygon,
        Dictionary<string, bool> cache)
    {
        if (validRanges.TryGetValue(y, out var r))
            return r;

        if (ranges.TryGetValue(y, out r)) 
            return r.Count > 0 ? r : null;
        r = GetYRanges(y, redTiles, polygon, cache);
        ranges[y] = r;

        return r.Count > 0 ? r : null;
    }

    private static bool Covered(int minX, int maxX, List<(int x1, int x2)> ranges) =>
        ranges.Aggregate(minX, (currentX, range) =>
            range.x1 > currentX ? currentX : Math.Max(currentX, range.x2 + 1)) > maxX;
        

    private record Polygon(
        Dictionary<int, List<(int x1, int x2)>> Horizontal,
        List<(int x, int y1, int y2)> Vertical,
        SortedSet<int> Ys
    )
    {
        public static Polygon Create(List<Coordinate> redTiles)
        {
            var ys = new SortedSet<int>();
            var horizontal = new Dictionary<int, List<(int x1, int x2)>>();
            var vertical = new List<(int x, int y1, int y2)>();

            for (var i = 0; i < redTiles.Count; i++)
            {
                var current = redTiles[i];
                var next = redTiles[(i + 1) % redTiles.Count];

                ys.Add(current.Y);

                if (current.X == next.X)
                    AddVertical(current, next, vertical, ys);
                else if (current.Y == next.Y)
                    AddHorizontal(current, next, horizontal);
            }

            return new Polygon(horizontal, vertical, ys);
        }

        private static void AddVertical(
            Coordinate current, 
            Coordinate next, 
            List<(int x, int y1, int y2)> vertical, 
            SortedSet<int> ys)
        {
            var minY = Math.Min(current.Y, next.Y);
            var maxY = Math.Max(current.Y, next.Y);
            vertical.Add((current.X, minY, maxY));

            for (var y = minY + 1; y < maxY; y++)
            {
                ys.Add(y);
            }
        }

        private static void AddHorizontal(
            Coordinate current, 
            Coordinate next, 
            Dictionary<int, List<(int x1, int x2)>> horizontal)
        {
            var minX = Math.Min(current.X, next.X);
            var maxX = Math.Max(current.X, next.X);

            if (!horizontal.ContainsKey(current.Y))
                horizontal[current.Y] = [];

            horizontal[current.Y].Add((minX, maxX));
        }
    }
}

