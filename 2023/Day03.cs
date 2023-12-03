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
        var gears = input.Matches(@"\*");
        var numbers = input.Matches(@"\d+");

        return gears
            .Select(g => numbers.Where(n => n.IsNextTo(g)).Select(n => int.Parse(n.Content)))
            .Where(nb => nb.Count() == 2)
            .Select(nb => nb.First() * nb.Last())
            .Sum();
    }

    (int X, int Y)[] Offsets =>
        [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1),];
}
