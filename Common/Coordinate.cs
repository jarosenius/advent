using System.Collections.Generic;
using System.Linq;

namespace Advent.Common;
public record Coordinate(int X, int Y){

    public static readonly Coordinate Zero = new (0, 0);
    public static readonly Coordinate Right = new (1, 0);
    public static readonly Coordinate Down = new(0, 1);
    public static readonly Coordinate Left = new(-1, 0);
    public static readonly Coordinate Up = new(0, -1);
    public static readonly Coordinate TopLeft = new(-1, -1);
    public static readonly Coordinate TopRight = new(1, -1);
    public static readonly Coordinate BottomRight = new(1, 1);
    public static readonly Coordinate BottomLeft = new(-1, 1);
    public static readonly List<Coordinate> Directions = [Right, Down, Left, Up];
    public static readonly List<Coordinate> DirectionsWithDiagonals = [Up, TopRight, Right, BottomRight, Down, BottomLeft, Left, TopLeft];
    public static Dictionary<Coordinate, char> CreateMap(List<string> input) => input.SelectMany((row, y) => row.Select((c, x) => KeyValuePair.Create(new Coordinate(x, y), c))).ToDictionary();

    public static Coordinate operator +(Coordinate a, Coordinate b) => new(a.X+b.X, a.Y+b.Y);
    public static Coordinate operator -(Coordinate a) => new(-a.X, -a.Y);
};