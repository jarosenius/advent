using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2023;

[AoC(2023)]
public class Day17 : Day
{
    public Day17()
        : base(17, 2023) { }

    public override object Part1(List<string> input) => CalculateHeatloss(Coordinate.CreateMap(input, int.Parse), 3, _ => true);

    public override object Part2(List<string> input) => CalculateHeatloss(Coordinate.CreateMap(input, int.Parse), 10, (Crucible c) => c.MovesInStraightLine > 3);

    private static int CalculateHeatloss(Dictionary<Coordinate, int> map, int maxStraightMoves, Func<Crucible, bool> allowedToTurn)
    {
        var target = map.Keys.MaxBy(c => c.X + c.Y);
        var queue = new PriorityQueue<Crucible, int>();

        var visited = new HashSet<Crucible>();
        var loss = 0;
        var crucible = new Crucible(Coordinate.Zero, Direction.Right, 0);
        queue.Enqueue(new Crucible(Coordinate.Zero, Direction.Down, 0), 0);

        do
        {
            if (crucible.Position == target)
                return loss;
            foreach (var district in GetNeighboringDistricts(crucible, maxStraightMoves, allowedToTurn))
            {
                if(map.TryGetValue(district.Position, out int value) && !visited.Contains(district))
                {
                    queue.Enqueue(district, loss + value);
                    visited.Add(district);
                }
            }
        } while (queue.TryDequeue(out crucible, out loss));
        return -1;
    }

    private static List<Crucible> GetNeighboringDistricts(Crucible crucible, int maxStraightMoves, Func<Crucible, bool> allowedToTurn)
    {
        var districts = new List<Crucible>();
        if(crucible.MovesInStraightLine < maxStraightMoves)
            districts.Add(crucible 
                with {
                    Position = crucible.Position + crucible.Direction, 
                    MovesInStraightLine = crucible.MovesInStraightLine + 1 
                });

        if(allowedToTurn(crucible))
        {
            GetNewDirections(crucible.Direction).ForEach(d =>
            {
                districts.Add(crucible 
                    with {
                        Position = crucible.Position + d, 
                        Direction = d,
                        MovesInStraightLine = 1 
                    });
            });
        }

        return districts;
    }

    private static IReadOnlyList<Coordinate> GetNewDirections(Coordinate direction)
    {
        if(Direction.RightLeft.Contains(direction))
            return Direction.UpDown;
        return Direction.RightLeft;
    }

    private record Crucible(Coordinate Position, Coordinate Direction, int MovesInStraightLine);
}
