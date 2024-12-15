using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Advent.Common;
public record Coordinate(int X, int Y)
{

    public static readonly Coordinate Zero = new(0, 0);
    public static Dictionary<Coordinate, char> CreateMap(List<string> input) => input.SelectMany((row, y) => row.Select((c, x) => KeyValuePair.Create(new Coordinate(x, y), c))).ToDictionary();
    public static Dictionary<Coordinate, T> CreateMap<T>(List<string> input, Func<string, T> parse) => input
        .SelectMany((row, y) => row
            .Select((c, x) => KeyValuePair.Create(new Coordinate(x, y), parse(c.ToString()))))
        .ToDictionary();

    public static Dictionary<Coordinate, T> CreateMap<T>(List<string> input, Func<char, T> parse) => input
        .SelectMany((row, y) => row
            .Select((c, x) => KeyValuePair.Create(new Coordinate(x, y), parse(c))))
        .ToDictionary();

    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X + b.X, a.Y + b.Y);
    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X - b.X, a.Y - b.Y);
    public static Coordinate operator -(Coordinate a) => new(-a.X, -a.Y);
    public static Coordinate operator *(int a, Coordinate b) => new(a * b.X, a * b.Y);
    public static Coordinate operator *(Coordinate a, int b) => new(a.X * b, a.Y * b);

};

public static class Direction
{
    public static readonly Coordinate None = new(0, 0);
    public static readonly Coordinate Right = new(1, 0);
    public static readonly Coordinate Down = new(0, 1);
    public static readonly Coordinate Left = new(-1, 0);
    public static readonly Coordinate Up = new(0, -1);
    public static readonly Coordinate UpLeft = new(-1, -1);
    public static readonly Coordinate UpRight = new(1, -1);
    public static readonly Coordinate DownRight = new(1, 1);
    public static readonly Coordinate DownLeft = new(-1, 1);
    public static readonly IReadOnlyList<Coordinate> Directions = [Right, Down, Left, Up];
    public static readonly IReadOnlyList<Coordinate> UpDown = [Up, Down];
    public static readonly IReadOnlyList<Coordinate> RightLeft = [Right, Left];
    public static readonly IReadOnlyList<Coordinate> DirectionsWithDiagonals = [Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft];
}

public static class MapExtensions
{
    public static void Print<T>(this Dictionary<Coordinate, T> map)
    {
        for (int y = 0; y <= map.Max(m => m.Key.Y); y++)
        {
            for (int x = 0; x <= map.Max(m => m.Key.X); x++)
            {
                Console.Write(map[new Coordinate(x, y)]);
            }
            Console.WriteLine();
        }
    }

    public static Dictionary<Coordinate, char> CreateMap(this List<string> input) => Coordinate.CreateMap(input);
    public static ImmutableDictionary<Coordinate, char> CreateImmutableMap(this List<string> input) => Coordinate.CreateMap(input).ToImmutableDictionary();
}
