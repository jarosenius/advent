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
        return 0;
    }

    static Dictionary<Coordinate, char> CreateMap(List<string> input)
    {
        var map = new Dictionary<Coordinate, char>();
        for (int row = 0; row < input.Count; row++)
        {
            for (int col = 0; col < input[0].Length; col++)
            {
                map[new Coordinate(col, row)] = input[row][col];
            }
        }
        return map;
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

    private static List<Coordinate> InverseConnectionTo(char c)
    {
        return ConnectionsTo(c).Select(coordinate => -coordinate).ToList();
    }

    private static List<Coordinate> ConnectionsTo(char c) => c switch {
        '|' => [Up, Down],
        '-' => [Left, Right],
        'L' => [Up, Right],
        'J' => [Up, Left],
        '7' => [Left, Down],
        'F' => [Right, Down],
        'S' => Directions,
        _ => []
    };

    private record Coordinate(int X, int Y){
        public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X+b.X, a.Y+b.Y);
        public static Coordinate operator -(Coordinate a) => new(-a.X, -a.Y);
    };
}

