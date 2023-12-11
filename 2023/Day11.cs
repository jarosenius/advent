using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day11 : Day
{
    public Day11() : base(11, 2023)
    {

    }

    public override object Part1(List<string> input) => GetResult(input);

    public override object Part2(List<string> input) => GetResult(input, 1000000);

    private static long GetResult(List<string> input, int expansion = 2)
    {
        var galaxies = GetCoordinatesForGalaxies(input);
        var emptyRows = GetEmptyRows(input);
        var emptyColumns = GetEmptyColumns(input);
        var pairs = galaxies.SelectMany((first, i) => galaxies.Skip(i+1).Select(second => (First: first, Second: second)));
        return pairs.Select(p => DistanceBetweenPoints(p.First, p.Second, expansion, emptyRows, emptyColumns)).Sum();
    }

    private static List<Point> GetCoordinatesForGalaxies(List<string> input) => 
        Enumerable.Range(0, input.Count)
        .SelectMany(y => 
            Enumerable.Range(0, input[0].Length)
            .Where(x => input[y][x] == '#')
            .Select(x => new Point(x, y)))
        .ToList();

    private static long DistanceBetweenPoints(Point first, Point second, int expansion, List<int> emptyRows, List<int> emptyColumns)
    {
        var diffX = Math.Abs(first.X - second.X);
        var diffY = Math.Abs(first.Y - second.Y);
        return diffX + diffY + Expansion(diffX, first.X, second.X, false) + Expansion(diffY, first.Y, second.Y, true);
        long Expansion(int diff, int a, int b, bool row)
        {
            var emptySpace = Enumerable.Range(Math.Min(a, b), diff).Count(i => row ? emptyRows.Contains(i) : emptyColumns.Contains(i));
            return (expansion-1)*emptySpace;
        }
    }
        
    private static List<int> GetEmptyRows(List<string> input) => Enumerable.Range(0, input.Count).Where(y => input[y].All(c => c == '.')).ToList();
    private static List<int> GetEmptyColumns(List<string> input) => Enumerable.Range(0, input[0].Length).Where(x => input.All(row => row[x] == '.')).ToList();
}

