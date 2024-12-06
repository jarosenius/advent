using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day06() : Day(6, 2024)
{
    public override object Part1(List<string> input)    
    {
        var map = Coordinate.CreateMap(input);
        return NavigateMap(map, map.First(m => m.Value == '^').Key).Visited.Count;
    }    
    
    public override object Part2(List<string> input)
    {
        var map = Coordinate.CreateMap(input);
        var start = map.First(m => m.Value == '^').Key;
        return NavigateMap(map, start).Visited
            .Except([start])
            .Sum(p => NavigateMap(new Dictionary<Coordinate, char>(map){ [p] = '#'}, start).Looping ? 1 : 0);
    }

    private static (HashSet<Coordinate> Visited, bool Looping) NavigateMap(Dictionary<Coordinate, char> map, Coordinate start)
    {
        var (currentPos, val, direction) = (start, map[start], Direction.Up);
        var visited = new HashSet<(Coordinate Position, Coordinate Direction)>{(currentPos, direction)};
        var looping = false;     

        while(!looping && map.TryGetValue(currentPos + direction, out val))
        {
            if(val == '#')
            {
                direction = new(-direction.Y, direction.X);
                continue;
            }

            currentPos += direction;
            looping = !visited.Add((currentPos, direction));
        }

        return (visited.Select(p => p.Position).ToHashSet(), looping);
    }
}

