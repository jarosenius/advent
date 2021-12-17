using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2021
{
    public class Day17 : Day
    {
        public Day17() : base(17, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
			var reg = new Regex(@"[xy]=(-?\d+)..(-?\d+)", RegexOptions.Compiled);
            var match = reg.Matches(input[0]);
            var iPos = match.SelectMany(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value))).ToArray();
            var area = new Pos(iPos[0], iPos[1], iPos[2], iPos[3]);

            return GetHighestAndDistinctVelocities(area).Highest;
        }

        public override long Part2(List<string> input)
        {
			var reg = new Regex(@"[xy]=(-?\d+)..(-?\d+)", RegexOptions.Compiled);
            var match = reg.Matches(input[0]);
            var iPos = match.SelectMany(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value))).ToArray();
            var area = new Pos(iPos[0], iPos[1], iPos[2], iPos[3]);

            return GetHighestAndDistinctVelocities(area).DistinctVelocities;
        }

        private (long Highest, long DistinctVelocities) GetHighestAndDistinctVelocities(Pos area)
        {
            var highest = 0L; 
            HashSet<(int x, int y)> hitsByVelocity = new();
            Enumerable.Range(area.yMin, (2*Math.Abs(area.yMax)) - area.yMin).ForEach(y => {
                Enumerable.Range(0, Math.Abs(area.xMax)*2).ForEach(x =>
                {
                    var t = CalculateTrajectory(area, x, y);
                    if(t.InArea)
                    {
                        hitsByVelocity.Add((x, y));
                        if(t.Highest > highest)
                            highest = t.Highest;
                    }
                });
            });      
            return (highest, hitsByVelocity.Count());
        }

        private Trajectory CalculateTrajectory(Pos area, int vx, int vy)
        {
            var x = 0;
            var y = 0;
            var highest = 0;
            do
            {
                x+=vx;
                y+=vy;
                if(x > area.xMax || y < area.yMin)
                    return new(false, highest);
                if(y > highest)
                    highest = y;
                if(x >= area.xMin && x <= area.xMax && y >= area.yMin && y <= area.yMax)
                    return new(true, highest);
                if(vx != 0)
                    vx -= (vx > 0 ? 1 : -1);
                vy -= 1;
            } while (true);
        }

        private record Trajectory(bool InArea, int Highest);
        private record Pos(int xMin, int xMax, int yMin, int yMax);
    }
}
