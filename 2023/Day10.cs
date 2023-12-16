using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Advent.Common;

namespace Advent.y2023;

[AoC(2023)]
public class Day10 : Day
{
    public Day10() : base(10, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var connections = NavigateMap(CreateMap(input));
        return connections.Count / 2;
    }
    public override object Part2(List<string> input)
    {
        var map = CreateMap(input);
        var connections = NavigateMap(map);
        map = KeepOnlyConnections(map, connections);
        var resMap = RemoveUTurnsAndSimplify(map);

        return resMap.Aggregate(0, (sum, row) =>
        {
            return row.Aggregate((Parity: 0, Sum: sum), (data, c) => 
            {
                if (c == '│') data.Parity++;
                if (c == ' ' && data.Parity % 2 == 1) data.Sum++;
                return data;
            }).Sum;
        });
    }

    private static List<string> RemoveUTurnsAndSimplify(Dictionary<Coordinate, char> map)
    {
        return map.GroupBy(m => m.Key.Y).Select(row =>
        {
            var sb = new StringBuilder();
            row.OrderBy(r => r.Key.X).ForEach(c => sb.Append(c.Value));
            var removeUTurns = Regex.Replace(sb.ToString(), "┌─*┐|└─*┘", string.Empty);
            return Regex.Replace(removeUTurns, "└─*┐|┌─*┘", "│");
        }).ToList();
    }

    static Dictionary<Coordinate, char> CreateMap(List<string> input)
    {
        return input.SelectMany((row, y) => row.Select((c, x) => KeyValuePair.Create<Coordinate, char>(new (x, y), ReplaceWith(c)))).ToDictionary();

        static char ReplaceWith(char c) => c switch {
            '-' => '─',
            '|' => '│',
            'F' => '┌',
            '7' => '┐',
            'L' => '└',
            'J' => '┘',
            '.' => ' ',
            _ => c,
        };
    }

    static HashSet<Coordinate> NavigateMap(Dictionary<Coordinate, char> map)
    {
        var pos = map.Keys.Single(k => map[k] == 'S');
        var positions = new HashSet<Coordinate>();
        var direction = Coordinate.Directions.First(d => InverseConnectionTo(map[pos+d]).Contains(d));

        do
        {
            positions.Add(pos);
            pos += direction;
            if(map[pos] == 'S')
                break;
            direction = ConnectionsTo(map[pos]).Single(d => d != -direction);
        } while (!positions.Contains(pos));

        return positions;
    }

    private static List<Coordinate> InverseConnectionTo(char c)
    {
        return ConnectionsTo(c).Select(coordinate => -coordinate).ToList();
    }

    private static List<Coordinate> ConnectionsTo(char c) => c switch {
        '│' => [Coordinate.Up, Coordinate.Down],
        '─' => [Coordinate.Left, Coordinate.Right],
        '└' => [Coordinate.Up, Coordinate.Right],
        '┘' => [Coordinate.Up, Coordinate.Left],
        '┐' => [Coordinate.Left, Coordinate.Down],
        '┌' => [Coordinate.Right, Coordinate.Down],
        'S' => Coordinate.Directions,
        _ => []
    };
    
    private static Dictionary<Coordinate, char> KeepOnlyConnections(Dictionary<Coordinate, char> map, HashSet<Coordinate> connections)
    {
        return map.Keys.ToDictionary(kvp => kvp, kvp => connections.Contains(kvp) ? map[kvp] : ' ');
    }

    private static void DrawMap(Dictionary<Coordinate, char> map)
    {
        Console.WriteLine("-----------");
        var rows = map.Keys.Max(coordinate => coordinate.Y);
        var cols = map.Keys.Max(coordinate => coordinate.X);

        for (var y = 0; y <= rows; y++)
        {
            for (var x = 0; x <= cols; x++)
            {
                Console.Write(map[new(x, y)]);
            }
            Console.WriteLine();
        }
    }
}

