using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day02 : Day
    {
        public Day02() : base(2, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            long hpos = 0;
            long vpos = 0;
            var cleaned = GetCleanedInput(input);
            var groups = cleaned.GroupBy(d => d.Direction);
            hpos += groups.FirstOrDefault(g => g.Key == Direction.Forward).Sum(g => g.Moves);
            vpos += groups.FirstOrDefault(g => g.Key == Direction.Down).Sum(g => g.Moves);
            vpos -= groups.FirstOrDefault(g => g.Key == Direction.Up).Sum(g => g.Moves);
             
            return hpos * vpos;
        }
        public override long Part2(List<string> input)
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
