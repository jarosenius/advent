using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day10 : Day
{
    static readonly Coordinate Right = new (1, 0);
    static readonly Coordinate Down = new(0, 1);
    static readonly Coordinate Left = new(-1, 0);
    static readonly Coordinate Up = new(0, -1);
    static readonly List<Coordinate> Directions = [Right, Down, Left, Up];

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
        map = Expand(map);
        map = FloodFill(map);

        return map.Values.Count(v => v == '#');
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
        var direction = Directions.First(d => InverseConnectionTo(map[pos+d]).Contains(d));

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

    private static Dictionary<Coordinate, char> Expand(Dictionary<Coordinate, char> map)
    {
        return map.Keys.SelectMany(k => 
            Expand(map[new(k.X, k.Y)])
            .SelectMany((s, y) => 
                    s.Select((c, x) => KeyValuePair.Create<Coordinate, char>(new(s.Length*k.X+x, s.Length*k.Y+y), c)))
            ).ToDictionary();

        static string[] Expand(char c) => c switch
        {
            '│' => [".│.", 
                    ".│.", 
                    ".│."],
            '─' => ["...", 
                    "───", 
                    "..."],
            '└' => [".│.", 
                    ".└─", 
                    "..."],
            '┘' => [".│.", 
                    "─┘.", 
                    "..."],
            '┐' => ["...", 
                    "─┐.", 
                    ".│."],
            '┌' => ["...", 
                    ".┌─", 
                    ".│."],
            'S' => [".│.", 
                    "─S─", 
                    ".│."],
            _ => ["...", 
                 $".{c}.", 
                  "..."], 
        };
    }

    private static List<Coordinate> InverseConnectionTo(char c)
    {
        return ConnectionsTo(c).Select(coordinate => -coordinate).ToList();
    }

    private static List<Coordinate> ConnectionsTo(char c) => c switch {
        '│' => [Up, Down],
        '─' => [Left, Right],
        '└' => [Up, Right],
        '┘' => [Up, Left],
        '┐' => [Left, Down],
        '┌' => [Right, Down],
        'S' => Directions,
        _ => []
    };
    
    private static Dictionary<Coordinate, char> KeepOnlyConnections(Dictionary<Coordinate, char> map, HashSet<Coordinate> connections)
    {
        return map.Keys.ToDictionary(kvp => kvp, kvp => connections.Contains(kvp) ? map[kvp] : '#');
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

    private static Dictionary<Coordinate, char> FloodFill(Dictionary<Coordinate, char> map)
    {
        var queue = new Queue<Coordinate>();
        queue.Enqueue(new Coordinate(0, 0));
        var visited = new HashSet<Coordinate>();
        var fill = new[]{'.', '#'};
        while(queue.TryDequeue(out var pos))
        {
            visited.Add(pos);
            if(!fill.Contains(map[pos]))
                continue;
            map[pos] = ' ';
            Directions.ForEach(d => 
            {
                var p = pos+d;
                if(map.ContainsKey(p) && !visited.Contains(p))
                    queue.Enqueue(p);
            });
        }
        return map;
    }

    private record Coordinate(int X, int Y){
        public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X+b.X, a.Y+b.Y);
        public static Coordinate operator -(Coordinate a) => new(-a.X, -a.Y);
    };
}

