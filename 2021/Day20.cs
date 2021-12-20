using System;
using System.Collections.Generic;
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
            Enumerable.Range(0, times).ForEach(i => image = Enhance(image, enhancement, i));
            return image.Count();
        }

        private HashSet<(int X, int Y)> Enhance(HashSet<(int X, int Y)> image, bool[] enhancement, int i)
        {
            var min = (X: image.Min(e => e.X), Y: image.Min(e => e.Y));
            var max = (X: image.Max(e => e.X), Y: image.Max(e => e.Y));

            void AddIfEnabled((int X, int Y) pos, bool[] enhancement, HashSet<(int X, int Y)> set)
            {
                bool IsEnabled((int X, int Y) pos)
                {
                    if(i % 2 == 0)
                        return image.Contains(pos);
                    if(enhancement[0] == false || (pos.X >= min.X && pos.X <= max.X && pos.Y >= min.Y && pos.Y <= max.Y))
                        return image.Contains(pos);
                    return true;
                }

                var positions = Offsets.Select(o => (o.X + pos.X, o.Y + pos.Y)).ToList();
                var num = Convert.ToInt32(new string(positions.Select(p => IsEnabled(p) ? '1' : '0').ToArray()), 2);      
                if(enhancement[num])
                    set.Add(pos);
            }

            var xCount = (max.X - min.X)+3;
            var yCount = (max.Y - min.Y)+3;
            HashSet<(int X, int Y)> res = new();

            Enumerable.Range(min.Y-1, yCount).SelectMany(y => 
                Enumerable.Range(min.X-1, xCount).Select(x => (x, y)))
            .ForEach(p => AddIfEnabled(p, enhancement, res));
            return res;
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
