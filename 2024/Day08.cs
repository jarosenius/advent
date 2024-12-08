using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day08() : Day(8, 2024)
{
    public override object Part1(List<string> input) => CountUniqueAntinodes(Coordinate.CreateMap(input));

    public override object Part2(List<string> input) => CountUniqueAntinodes(Coordinate.CreateMap(input), true);

    private static int CountUniqueAntinodes(Dictionary<Coordinate, char> map, bool part2 = false)
    {
        var maxX = map.Max(kvp => kvp.Key.X);
        var maxY = map.Max(kvp => kvp.Key.Y);

        var antennas = new Dictionary<char, List<Coordinate>>();
        map.ForEach(kvp => {
            if(kvp.Value != '.')
            {
                if(!antennas.TryGetValue(kvp.Value, out var list))
                    antennas[kvp.Value] = list = [];
                list.Add(kvp.Key);
            }
        });

        var antinodes = new HashSet<Coordinate>();
        antennas.ForEach(a => {
            var locations = a.Value;
            for (var i = 0; i < locations.Count; i++)
            {
                for (var j = i + 1; j < locations.Count; j++)
                {
                    if(i == j)
                        continue;

                    var c1 = locations[i];
                    var c2 = locations[j];
                    var d = c2 - c1;
                    
                    var a1 = c1 - d;
                    var a2 = c2 + d;
                    var added = false;
                    
                    do
                    {
                        added = false;
                        if(a1.X >= 0 && a1.X <= maxX && a1.Y >= 0 && a1.Y <= maxY)
                        {
                            antinodes.Add(a1);
                            added = true;
                            a1 -= d;
                        }
                        if(a2.X >= 0 && a2.X <= maxX && a2.Y >= 0 && a2.Y <= maxY)
                        {
                            antinodes.Add(a2);
                            added = true;  
                            a2 += d;
                        }
                    }
                    while(part2 && added);
                    if(part2)
                    {
                        antinodes.Add(c1);
                        antinodes.Add(c2);
                    }
                }
            }
        });
        return antinodes.Count;
    }
}

