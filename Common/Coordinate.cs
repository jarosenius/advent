using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Common;
public record Coordinate(int X, int Y){

    public static readonly Coordinate Zero = new (0, 0);
    public static Dictionary<Coordinate, char> CreateMap(List<string> input) => input.SelectMany((row, y) => row.Select((c, x) => KeyValuePair.Create(new Coordinate(x, y), c))).ToDictionary();
    public static Dictionary<Coordinate, T> CreateMap<T>(List<string> input, Func<string, T> parse) => input
        .SelectMany((row, y) => row
            .Select((c, x) => KeyValuePair.Create(new Coordinate(x, y), parse(c.ToString()))))
        .ToDictionary();

    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X+b.X, a.Y+b.Y);
    public static Coordinate operator -(Coordinate a, Coordinate b) => new(a.X-b.X, a.Y-b.Y);
    public static Coordinate operator -(Coordinate a) => new(-a.X, -a.Y);
};

public static class Direction
{
    public static readonly Coordinate Right = new (1, 0);
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