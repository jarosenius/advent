using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2023;
[AoC(2023)]
public class Day16 : Day
{
    public Day16() : base(16, 2023)
    {

    }

    public override object Part1(List<string> input) => FloodCells(Coordinate.CreateMap(input), new(Coordinate.Zero, Direction.Right));
    public override object Part2(List<string> input)
    {
        var map = Coordinate.CreateMap(input);
        return GetBeams(map).Max(b => FloodCells(map, b));
    }

    private static IEnumerable<Beam> GetBeams(Dictionary<Coordinate, char> map)
    {
        var bottomRight = map.Keys.MaxBy(c => c.X + c.Y);
        return map.Keys.Where(c => c.Y == bottomRight.Y)
            .Select(c => new Beam(c, Direction.Up))
            .Concat(map.Keys.Where(c => c.X == bottomRight.X).Select(c => new Beam(c, Direction.Left)))
            .Concat(map.Keys.Where(c => c.Y == 0).Select(c => new Beam(c, Direction.Down)))
            .Concat(map.Keys.Where(c => c.X == 0).Select(c => new Beam(c, Direction.Right)));
    }

    private static int FloodCells(Dictionary<Coordinate, char> map, Beam beam) 
    {
        HashSet<Beam> visited = [];
        Queue<Beam> queue = new();

        do
        {
            visited.Add(beam);
            foreach (var dir in GetDirections(map[beam.Position], beam.Direction))
            {
                var newBeam = new Beam(beam.Position + dir, dir);
                if (map.ContainsKey(newBeam.Position) && !visited.Contains(newBeam))
                {
                    queue.Enqueue(newBeam);
                }
            }
        }
        while (queue.TryDequeue(out beam));

        return visited.Select(beam => beam.Position).ToHashSet().Count;
    }

    private static IEnumerable<Coordinate> GetDirections(char c, Coordinate direction) => c switch 
    {
        '|' when Direction.RightLeft.Contains(direction) => Direction.UpDown,
        '-' when Direction.UpDown.Contains(direction) => Direction.RightLeft,
        '\\' => [new Coordinate(direction.Y, direction.X)],
        '/' => [-new Coordinate(direction.Y, direction.X)],
        _ => [direction]
    };    

    private record Beam(Coordinate Position, Coordinate Direction);
}

