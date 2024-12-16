using System.Collections.Generic;
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
        var nav = FindBestPath(map, start, goal);
        return nav;
    }
    public override object Part2(List<string> input)
    {
        return null;
    }

    private static int FindBestPath(Dictionary<Coordinate, char> map, Coordinate start, Coordinate goal)
    {
        var cost = 0;
        var visited = new HashSet<Step>();
        var queue = new PriorityQueue<Step, int>();
        var step = new Step(start, Direction.Right);

        do
        {
            if (step.Position == goal)
                return cost;
            Direction.Directions.ForEach(d =>
            {
                var next = new Step(step.Position+d, d);
                if(!map.TryGetValue(next.Position, out var value) || value == '#')
                    return;
                
                if(!visited.Contains(next))
                {
                    queue.Enqueue(next, cost + (IsTurn(d, step.Direction) ? 1001 : 1));
                    visited.Add(next);
                }
            });
        } while (queue.TryDequeue(out step, out cost));
        return -1;
    }

    private static bool IsTurn(Coordinate dir, Coordinate previous)
    {
        if(previous == Direction.None)
            return false;
        return dir != previous;    
    }

    private record Step(Coordinate Position, Coordinate Direction);
}

