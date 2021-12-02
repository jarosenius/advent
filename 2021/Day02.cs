using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2021
{
    public class Day02 : Day
    {
        public Day02() : base(2, 2021)
        {
            
        }

        public override async Task<long> Part1(IEnumerable<string> input)
        {
            long hpos = 0;
            long vpos = 0;
            var cleaned = GetCleanedInput(input);
            var groups = cleaned.GroupBy(d => d.Direction);
            hpos += groups.FirstOrDefault(g => g.Key == Direction.Forward).Sum(g => g.Moves);
            vpos += groups.FirstOrDefault(g => g.Key == Direction.Down).Sum(g => g.Moves);
            vpos -= groups.FirstOrDefault(g => g.Key == Direction.Up).Sum(g => g.Moves);
            await Task.CompletedTask; 
            return hpos * vpos;
        }
        public override async Task<long> Part2(IEnumerable<string> input)
        {
            long hpos = 0;
            long vpos = 0;
            long aim = 0;
            var cleaned = GetCleanedInput(input);
            cleaned.ForEach(i => {
                switch (i.Direction)
                {
                    case Direction.Forward:
                        hpos += i.Moves;
                        vpos += (i.Moves * aim);
                        break;
                    case Direction.Up:
                        aim -= i.Moves;
                        break;
                    case Direction.Down:
                        aim += i.Moves;
                        break;
                    default:
                        break;
                }
            });
            await Task.CompletedTask; 
            return hpos * vpos;
        }

        private enum Direction
        {
            Forward,
            Up,
            Down,
        }
        private static List<(Direction Direction, int Moves)> GetCleanedInput(IEnumerable<string> input)
        {
            return input.Select(r => {
                var parts = r.Split(" ");
                Direction.TryParse(parts[0], true, out Direction dir);
                return (dir, int.Parse(parts[1]));
            }).ToList();
        }
    }
}
