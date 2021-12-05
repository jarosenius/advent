using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2021
{
    public class Day05 : Day
    {
        public Day05() : base(5, 2021)
        {
            
        }

        public override long Part1(IEnumerable<string> input)
        {
            var vents = GetVents(input, true);
            return CountOverlappingAtleastNTimes(vents, 2);
        }

        public override long Part2(IEnumerable<string> input)
        {
            var vents = GetVents(input, false);
            return CountOverlappingAtleastNTimes(vents, 2);
        }

        private long CountOverlappingAtleastNTimes(List<Vent> vents, int n)
        {
            var coordinates = vents.SelectMany(v => v.AllCoordinates).ToList();
            
            var width = coordinates.Max(v => v.X)+1;
            var length = coordinates.Max(v => v.Y)+1;
            var grid = new int[length][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new int[width];
            }
            coordinates.ForEach(c =>  grid[c.Y][c.X]+=1);
            return grid.Sum(r => r.Count(c => c >= n));
        }

        private List<Vent> GetVents(IEnumerable<string> input, bool skipDiagonal)
        {
            List<Vent> res = new();

            foreach (var line in input)
            {
                var parts = line.Split(" -> ");
                var start = parts[0].Split(",").Select(int.Parse).ToArray();
                var end = parts[1].Split(",").Select(int.Parse).ToArray();

                res.Add(new Vent(start[0], start[1], end[0], end[1]));
            }
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
                    if(Start.X < End.X)
                    {
                        var x = maxX;
                        return Enumerable.Range(minY, countY + 1).Select(y => new Coordinate(x--, y)).ToList();
                    }
                    else
                    {
                        var x = minX;
                        return Enumerable.Range(minY, countY + 1).Select(y => new Coordinate(x++, y)).ToList();
                    }
                }

                var y = minY;
                return Enumerable.Range(minX, countX + 1).Select(x => new Coordinate(x, y++)).ToList();    
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
