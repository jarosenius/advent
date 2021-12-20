using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent.y2021
{
    public class Day20 : Day
    {
        private static List<(int X, int Y)> Offsets = new List<(int X, int Y)> 
        { 
            (-1, -1), 
            (0, -1), 
            (1, -1), 
            (-1, 0), 
            (0, 0), 
            (1, 0), 
            (-1, 1), 
            (0, 1), 
            (1, 1)
        };

        public Day20() : base(20, 2021)
        {
        }
        public override long Part1(List<string> input)
        {
            var enhancement = input[0].Select(c => c == '#').ToArray();
            var image = input.Skip(2)
                .SelectMany((r, y) => r
                .Select((c, x) => (X: x, Y: y, C: c)))
                .Where(e => e.C == '#')
                .Select(e => (e.X, e.Y))
                .ToHashSet<(int X, int Y)>();
            return Run(2, image, enhancement);
        }
        public override long Part2(List<string> input)
        {
            var enhancement = input[0].Select(c => c == '#').ToArray();
            var image = input.Skip(2)
                .SelectMany((r, y) => r
                .Select((c, x) => (X: x, Y: y, C: c)))
                .Where(e => e.C == '#')
                .Select(e => (e.X, e.Y))
                .ToHashSet<(int X, int Y)>();
            return Run(50, image, enhancement);
        }

        private long Run(int times, HashSet<(int X, int Y)> image, bool[] enhancement)
        {
            Enumerable.Range(0, times).ForEach(i => 
            {
                var minX = image.Min(e => e.X);
                var minY = image.Min(e => e.Y);
                var maxX = image.Max(e => e.X);
                var maxY = image.Max(e => e.Y);

                bool IsEnabled((int X, int Y) pos)
                {
                    if(i % 2 == 0)
                        return image.Contains(pos);
                    
                    var inCurrent = pos.X >= minX && pos.X <= maxX && pos.Y >= minY && pos.Y <= maxY;
                    if(inCurrent || enhancement[0] == false)
                    {
                        return image.Contains(pos);
                    }

                    return true;
                }

                HashSet<(int X, int Y)> res = new();

                for (int y = minY-1; y <= maxY+1; y++)
                {
                    for (int x = minX-1; x <= maxX+1; x++)
                    {  
                        var positions = Offsets.Select(o => (o.X + x, o.Y + y)).ToList();
                        var bin = new string(positions.Select(pos => IsEnabled(pos) ? '1' : '0').ToArray());
                        var bin2num = Convert.ToInt32(bin, 2);      
                        if(enhancement[bin2num])
                            res.Add((x, y));
                    }
                }
                image = res;
            });
            return image.Count();
        }
        
        private void PrintImage(HashSet<(int X, int Y)> image)
        {
            var rowWidth = image.Max(k => k.X) - image.Min(k => k.X)+1;
            var written = 0;
            for (var y = image.Min(p => p.Y); y <= image.Max(p => p.Y); y++)
            {
                for (var x = image.Min(p => p.X); x <= image.Max(p => p.X); x++)
                {
                    Console.Write(image.Contains((x, y)) ? "#" : ".");
                    if(++written == rowWidth)
                    {
                        Console.Write(Environment.NewLine);
                        written = 0;
                    }
                }
            }
            Console.WriteLine("-------------");
        }
    }
}
