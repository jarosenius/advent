using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day16() : Day(16, 2024)
{
    public override object Part1(List<string> input)
    {
        var map = input.CreateMap();
        var start = map.Single(p => p.Value == 'S').Key;
        var goal = map.Single(p => p.Value == 'E').Key;
        return FindBestPath(map, start, goal).Cost;
    }
    public override object Part2(List<string> input)
    {
        var map = input.CreateMap();
        var start = map.Single(p => p.Value == 'S').Key;
        var goal = map.Single(p => p.Value == 'E').Key;
        return FindBestPath(map, start, goal).PossiblePath.Count;
    }

    private static (HashSet<Coordinate> PossiblePath, int Cost) FindBestPath(Dictionary<Coordinate, char> map, Coordinate start, Coordinate goal)
    {
        var cost = 0;
        var best = -1;
        var possiblePath = new HashSet<Coordinate>([start]);
        var queue = new PriorityQueue<Step, int>();
        var step = new Step(start, Direction.Right, []);
        var costForPosition = new Dictionary<(Coordinate, Coordinate), int>();
        do
        {
            if (step.Position == goal && (best == -1 || cost <= best))
            {
                step.Visited.ForEach(v => possiblePath.Add(v.Pos));
                best = cost;
                continue;
            }
            Direction.Directions.ForEach(d =>
            {
                var next = new Step(step.Position+d, d, step.Visited);
                if(!map.TryGetValue(next.Position, out var value) || value == '#')
                    return;

                var newCost = cost + (IsTurn(d, step.Direction) ? 1001 : 1);
                if(newCost <= costForPosition.GetValueOrDefault((next.Position, d), int.MaxValue))
                {
                    costForPosition[(next.Position, d)] = newCost;
                    queue.Enqueue(next with {Visited = next.Visited.Add((next.Position, d))}, newCost);
                }
            });
        } while (queue.TryDequeue(out step, out cost));
        return (possiblePath, best);
    }

    private static bool IsTurn(Coordinate dir, Coordinate previous)
    {
        if(previous == Direction.None)
            return false;
        return dir != previous;    
    }

    private record Step(Coordinate Position, Coordinate Direction, ImmutableHashSet<(Coordinate Pos, Coordinate Dir)> Visited);
}

