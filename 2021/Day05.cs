using System;
using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day05 : Day
    {
        public Day05() : base(5, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var vents = GetVents(input.ToList(), true);
            return CountOverlappingAtleastNTimes(vents, 2);
        }

        public override long Part2(List<string> input)
        {
            var vents = GetVents(input.ToList(), false);
            return CountOverlappingAtleastNTimes(vents, 2);
        }

        private long CountOverlappingAtleastNTimes(List<Vent> vents, int n)
        {
            var coordinates = vents.SelectMany(v => v.AllCoordinates).ToList();
            var grid = Utils.CreateMapWithSize<int>(coordinates.Max(v => v.X)+1, coordinates.Max(v => v.Y)+1);
            coordinates.ForEach(c => grid[c.Y][c.X]+=1);
            return grid.Sum(r => r.Count(c => c >= n));
        }

        private List<Vent> GetVents(List<string> input, bool skipDiagonal)
        {
            List<Vent> res = new();
            input.ForEach(line =>
            {
                var parts = line.SplitByFirstThenBySecondAndParseToInt(" -> ", ",");
                res.Add(new Vent(parts[0][0], parts[0][1], parts[1][0], parts[1][1]));
            });

            return skipDiagonal ? res.Where(v => v.IsDiagonal == false).ToList() : res;
        }

        private class Vent
        {
            public Coordinate Start { get;set; }
            public Coordinate End { get;set; }
            private List<Coordinate> allCoordinates;
            public List<Coordinate> AllCoordinates => allCoordinates ?? (allCoordinates = GetAllCoordinates());
            public bool IsHorizontal => Start.Y == End.Y;
            public bool IsVertical => Start.X == End.X;
            public bool IsDiagonal => !IsHorizontal && !IsVertical;
            public Vent(int startX, int startY, int endX, int endY)
            {
                Start = new Coordinate(startX, startY);
                End = new Coordinate(endX, endY);

                if(End.X < Start.X)
                {
                    var tmp = Start;
                    Start = End;
                    End = tmp;
                }
            }

            private List<Coordinate> GetAllCoordinates()
            {
                var minX = Math.Min(Start.X, End.X);
                var maxX = Math.Max(Start.X, End.X);
                var minY = Math.Min(Start.Y, End.Y);
                var maxY = Math.Max(Start.Y, End.Y);
                var countX = Math.Abs(maxX - minX);
                var countY = Math.Abs(maxY - minY);

                if(IsHorizontal)
                {
                    return Enumerable.Range(minX, countX + 1).Select(x => new Coordinate(x, Start.Y)).ToList();
                }
                else if(IsVertical)
                {
                    return Enumerable.Range(minY, countY + 1).Select(y => new Coordinate(Start.X, y)).ToList();
                }

                // IsDiagonal
                if(countX != countY)
                    throw new ArgumentException("Cannot handle this type of diagonal");

                if(End.Y < Start.Y)
                {
                    return Enumerable.Range(minY, countY + 1).Select(y => new Coordinate(maxX--, y)).ToList();
                }

                return Enumerable.Range(minX, countX + 1).Select(x => new Coordinate(x, minY++)).ToList();    
            }
        }
        private class Coordinate
        {
            public int X { get;set; }
            public int Y { get;set; }
            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
