using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2023;

[AoC(2023)]
public class Day18 : Day
{
    public Day18() : base(18, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var digPlan = input.Select(DigInstruction.Create).ToList();
        var map = CreateMap(digPlan);
        return CountDepth(map);
    }



    public override object Part2(List<string> input)
    {
        return null;
    }

    private static HashSet<Coordinate> CreateMap(List<DigInstruction> digPlan)
    {
        var map = new HashSet<Coordinate>();
        var current = Coordinate.Zero;
        digPlan.ForEach(instr => {
            for (int i = 0; i < instr.Steps; i++)
            {
                map.Add(current += instr.Dir);
            }
        });
        return map;
    }

    private static int CountDepth(HashSet<Coordinate> map)
    {
        var minX = map.Min(c => c.X);
        var maxX = map.Max(c => c.X);
        var minY = map.Min(c => c.Y);
        var maxY = map.Max(c => c.Y);
        var sum = 0;
        for (int y = minY; y <= maxY; y++)
        {
            var parity = 0;
            for (int x = minX; x <= maxX; x++)
            {
                if(map.Contains(new(x, y)))
                {
                    var startConnectedTop = map.Contains(new(x, y-1));
                    var startConnectedBottom = map.Contains(new(x, y+1));

                    if(startConnectedTop && startConnectedBottom)
                        parity++;

                    sum++;
                    var start = x;
                    while(x+1 <= maxX && map.Contains(new(x+1, y)))
                    {
                        x++;
                        sum++;
                    }

                    if(x - start > 0)
                    {
                        var endConnectedTop = map.Contains(new(x, y-1));
                        var endConnectedBottom = map.Contains(new(x, y+1));

                        if((startConnectedTop && endConnectedBottom) || (startConnectedBottom && endConnectedTop))
                            parity++;
                    }
                }    
                else if(parity % 2 == 1)
                {
                    sum++;
                }
            } 
        }
        return sum;
    }

    private record DigInstruction(Coordinate Dir, int Steps)
    {
        public static DigInstruction Create(string input)
        {
            var parts = input.Split(" ");
            var direction = parts[0][0] switch {
                'R' => Direction.Right,
                'U' => Direction.Up,
                'L' => Direction.Left,
                'D' => Direction.Down,
                _ => throw new ArgumentException($"'{input}' is not a valid line.")
            };
            return new DigInstruction(direction, int.Parse(parts[1]));
        }
    }
}

