using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Advent.y2023;

[AoC(2023)]
public class Day03 : Day
{
    public Day03()
        : base(3, 2023) { }

    public override object Part1(List<string> input)
    {
        var map = input.Select(row => row.ToCharArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;

        var sum = 0;
        var number = 0;
        var isPartNumber = false;
        for (var y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var c = map[y][x];
                if (char.IsDigit(c))
                {
                    number = number * 10 + (c - '0');
                    var neighbors = map.GetNeighbors(x, y, true);
                    if (neighbors.Any(n => n != '\0' && !char.IsDigit(n) && n != '.'))
                        isPartNumber = true;
                }
                else
                    NewNumber();
            }
            NewNumber();
        }

        return sum;

        void NewNumber()
        {
            if (isPartNumber)
                sum += number;
            number = 0;
            isPartNumber = false;
        }
    }

    public override object Part2(List<string> input)
    {
        var map = input.Select(row => row.ToCharArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;

        var gears = new Dictionary<(int X, int Y), List<int>>();
        var neighbors = new HashSet<(int X, int Y)>();
        var number = 0;
        for (var y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var c = map[y][x];

                if (char.IsDigit(c))
                {
                    number = number * 10 + (c - '0');
                    Offsets.ForEach(o =>
                    {
                        var nX = x + o.X;
                        var nY = y + o.Y;
                        if (nX < 0 || nY < 0 || nX >= width || nY >= height)
                            return;
                        if (map[nY][nX] == '*')
                            neighbors.Add((nX, nY));
                    });
                }
                else
                    NewNumber();
            }
            NewNumber();
        }

        return gears.Where(g => g.Value.Count == 2).Select(g => g.Value[0] * g.Value[1]).Sum();

        void NewNumber()
        {
            if (neighbors.Count > 0)
            {
                neighbors.ForEach(n =>
                {
                    var xy = (n.X, n.Y);
                    if (!gears.ContainsKey(xy))
                        gears[xy] = [];
                    gears[xy].Add(number);
                });
            }

            number = 0;
            neighbors.Clear();
        }
    }

    (int X, int Y)[] Offsets =>
        [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1),];
}
