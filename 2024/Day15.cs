using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day15() : Day(15, 2024)
{
    public override object Part1(List<string> input)
    {
        var mapPart = input.TakeWhile(r => !string.IsNullOrEmpty(r)).ToList();
        var map = mapPart.CreateMap();
        var movements = string.Join("", input.Skip(mapPart.Count));
        return ApplyMoves(map, movements).Sum(kvp => kvp.Value == 'O' ? (100 * kvp.Key.Y + kvp.Key.X) : 0);
    }



    public override object Part2(List<string> input)
    {
        return null;
    }

    private static Dictionary<Coordinate, char> ApplyMoves(Dictionary<Coordinate, char> mapInput, string movements)
    {
        var current = mapInput.First(m => m.Value == '@').Key;
        var map = mapInput.ToDictionary();
        foreach (var m in movements)
        {
            var move = GetDirection(m);
            if(Push(map, current, move))
                current += move;
        }
        //map.Print();
        return map;
    }

    private static Coordinate GetDirection(char move) => move switch
    {
        '>' => Direction.Right,
        '<' => Direction.Left,
        '^' => Direction.Up,
        'v' => Direction.Down,
        _ => Direction.None
    };

    private static bool Push(Dictionary<Coordinate, char> map, Coordinate from, Coordinate direction)
    {
        var to = from+direction;
        if(!map.TryGetValue(to, out var value) || value == '#' || value == '@')
            return false;

        if(value == '.' || Push(map, to, direction))
        {
            map[to] = map[from];
            map[from] = '.';
            return true;
        }

        return false;
    }
}

