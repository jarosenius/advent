using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
namespace Advent.y2021
{
    public class Day09 : Day
    {
        public Day09() : base(9, 2021)
        {
            
        }
        private int[][] heatmap;
        private List<(int X, int Y)> lows = new();
        public override long Part1(List<string> input)
        {
            heatmap = input.Select(i => i.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
            var sum = 0;
            for (int y = 0; y < heatmap.Length; y++)
            {
                for (int x = 0; x < heatmap[y].Length; x++)
                {
                    var comparisons = new int[4];
                    comparisons[0] =  y == 0 ? 9 : heatmap[y-1][x]; // above
                    comparisons[1] = x == heatmap[y].Length-1 ? 9 : heatmap[y][x+1]; // right
                    comparisons[2] = y == heatmap.Length-1 ? 9 : heatmap[y+1][x]; // below
                    comparisons[3] = x == 0 ? 9 : heatmap[y][x-1]; // left
                    if(comparisons.All(i => heatmap[y][x] < i))
                    {
                        sum += heatmap[y][x]+1;
                        lows.Add((x, y));
                    }
                }
                
            }
             return sum;
        }
        public override long Part2(List<string> input)
        {
            HashSet<(int X, int Y)> evaluated = new();
            List<long> res = new();
            
            for (int i = 0; i < lows.Count; i++)
            {
                
                var basin = new List<(int X, int Y)>{lows[i]};
                var size = 0;

                for (int j = 0; j < basin.Count; j++)
                {
                    var (x, y) = basin[j];
                    var current = heatmap[y][x];
                    var key = $"{x},{y}";
                    if(evaluated.Contains((x, y)))
                        continue;
                    if(heatmap[y][x] == 9)
                        continue;
                    evaluated.Add((x, y));
                    size++;
                    if(y > 0 && heatmap[y-1][x] > current)
                        basin.Add((x, y-1));
                    if(y < heatmap.Length-1 && heatmap[y+1][x] > current)
                        basin.Add((x, y+1));
                    if(x > 0 && heatmap[y][x-1] > current)
                        basin.Add((x-1, y));
                    if(x < heatmap[y].Length-1 && heatmap[y][x+1] > current)
                        basin.Add((x+1, y));
                }

                res.Add(size);
            }
            return res.OrderByDescending(s => s).Take(3).Aggregate(1L, (s1, s2) => s1*s2);
        }
    }
}
