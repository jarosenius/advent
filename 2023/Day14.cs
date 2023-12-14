using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day14 : Day
{
    public Day14() : base(14, 2023)
    {
    }

    public override object Part1(List<string> input) => GetLoad(input, 1, 1);
    public override object Part2(List<string> input) => GetLoad(input, 1000000000, 4);

    private static long GetLoad(List<string> input, int cycles, int rotations)
    {
        var board = input.Select(i => i.ToArray()).ToArray();
        var cache = new Dictionary<string, int>();
        for (int i = 0; i < cycles; i++)
        {
            board = PerformCycle(board, rotations);
            var key = string.Join("", board.SelectMany(row => row.Select(c => c)));
            if(cache.TryGetValue(key, out var foundAt))
            {
                var remaining = cycles - i - 1;
                remaining %= cache.Count - foundAt;
                while(remaining-- > 0)
                {
                    board = PerformCycle(board, rotations);
                }
                break;
            }
            cache[key] = i;
        }

        return board.Select((row, i) => (board.Length - i) * row.Count(c => c == 'O')).Sum();
    }

    private static char[][] PerformCycle(char[][] input, int cycleLength)
    {
        if(cycleLength == 1)
            return Tilt(input);

        return Enumerable.Range(0, cycleLength-1).Aggregate(Rotate(Tilt(input)), (tilted, _) => Rotate(Tilt(tilted)));
    }

    private static char[][] Rotate(char[][] input)
    {
        var rotated = Utils.CreateMapWithSize<char>(input.Length, input[0].Length);
        for (var x = input[0].Length-1; x >= 0; x--)
        for (var y = 0; y < input.Length; y++)
            rotated[x][y] = input[input.Length - y - 1][x];

        return rotated;
    }
    private static char[][] Tilt(char[][] input)
    {
        var tilted = Utils.CreateMapWithSize<char>(input[0].Length, input.Length);
        for (int x = 0; x < input[0].Length; x++)
        {
            var current = 0;
            for (var y = 0; y < input.Length; y++)
            {
                var block = input[y][x] == '#';
                tilted[y][x] = block ? '#' : '.';
                
                if(block)
                    current = y+1;
                else if (input[y][x] == 'O')
                    tilted[current++][x] = 'O';
            }
        }
        return tilted;
    }
}
