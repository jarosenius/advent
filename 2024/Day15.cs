using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day15() : Day(15, 2024)
{
    public override object Part1(List<string> input)
    {
        var mapPart = input.TakeWhile(r => !string.IsNullOrEmpty(r)).ToList();
        var map = mapPart.CreateImmutableMap();
        var movements = string.Join("", input.Skip(mapPart.Count));
        return ApplyMoves(ref map, movements).Sum(kvp => kvp.Value == 'O' ? (100 * kvp.Key.Y + kvp.Key.X) : 0);
    }



    public override object Part2(List<string> input)
    {
        var mapPart = input.TakeWhile(r => !string.IsNullOrEmpty(r)).Select(r =>
            r.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.")
        ).ToList();
        var map = mapPart.CreateImmutableMap();
        var movements = string.Join("", input.Skip(mapPart.Count));
        return ApplyMoves(ref map, movements).Sum(kvp => (kvp.Value == 'O' || kvp.Value == '[') ? (100 * kvp.Key.Y + kvp.Key.X) : 0);
    }

    private static ImmutableDictionary<Coordinate, char> ApplyMoves(ref ImmutableDictionary<Coordinate, char> map, string movements)
    {
        var current = map.First(m => m.Value == '@').Key;
        foreach (var m in movements)
        {
            var move = GetDirection(m);
            if (Push(ref map, current, move))
                current += move;
        }
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

    private static bool Push(ref ImmutableDictionary<Coordinate, char> map, Coordinate from, Coordinate direction)
    {
        var mapBackup = map;
        var to = from + direction;
        var value = map[from];

        if (value == '.')
            return true;
        else if (value is 'O' or '@')
        {
            if (Push(ref map, to, direction))
            {
                map = map
                    .SetItem(to, value)
                    .SetItem(from, '.');
                return true;
            }
        }
        else if (value == '[')
        {
            if (Direction.UpDown.Contains(direction))
            {
                var rightOfTo = to + Direction.Right;
                if (Push(ref map, to, direction) && Push(ref map, rightOfTo, direction))
                {
                    map = map
                        .SetItem(to, '[')
                        .SetItem(rightOfTo, ']')
                        .SetItem(from + Direction.Right, '.')
                        .SetItem(from, '.');
                    return true;
                }
            }
            else if (direction == Direction.Right)
            {
                var otherEdge = from + 2 * direction;
                if (Push(ref map, otherEdge, direction))
                {
                    map = map
                        .SetItem(to, '[')
                        .SetItem(otherEdge, ']')
                        .SetItem(from, '.');
                    return true;
                }
            }
            else if (direction == Direction.Left)
            {
                if (Push(ref map, to, direction))
                {
                    map = map
                        .SetItem(from + Direction.Right, '.')
                        .SetItem(to, '[')
                        .SetItem(from, ']');
                    return true;
                }
            }
        }
        else if (value == ']')
            return Push(ref map, from + Direction.Left, direction);

        map = mapBackup;
        return false;
    }
}

