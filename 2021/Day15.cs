﻿using System.Collections.Generic;
using System.Linq;

namespace Advent.y2021
{
    public class Day15 : Day
    {
        public Day15() : base(15, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var map = input.SelectMany((r, y) => r.Select((c, x) => new KeyValuePair<(int X, int Y), int>((x, y), int.Parse(c.ToString())))).ToDictionary(c => c.Key, c => c.Value);
            var shortest = FindAStarPath(map, (0, 0), (map.Keys.Max(p => p.X), map.Keys.Max(p => p.Y)));
            return shortest.Skip(1).Sum(v => map[v]);
        }
        public override long Part2(List<string> input)
        {
            var map = input.SelectMany((r, y) => r.Select((c, x) => new KeyValuePair<(int X, int Y), int>((x, y), int.Parse(c.ToString())))).ToDictionary(c => c.Key, c => c.Value);     
            map = GrowMap(map);
            var shortest = FindAStarPath(map, (0, 0), (map.Keys.Max(p => p.X), map.Keys.Max(p => p.Y)));
            return shortest.Skip(1).Sum(v => map[v]);
        }

        private List<(int X, int Y)> FindAStarPath(Dictionary<(int X, int Y), int> map, (int X, int Y) start, (int X, int Y) goal)
        {
            Queue<(int X, int Y)> pathsToEvaluate = new();
            Dictionary<(int X, int Y), long> score = new();
            Dictionary<(int X, int Y), (int X, int Y)> path = new();
            score[start] = 0;
            var maxX = map.Keys.Max(p => p.X);
            var maxY = map.Keys.Max(p => p.Y);
            var pos = start;
            do
            {
                if(pos == goal)
                    return WalkPathBackwards(path, pos);
                GetNeighbors(pos, maxX, maxY).ForEach(neighbor =>
                {
                    var neighborScore = score[pos] + map.GetValueOrDefault(neighbor, 10000);
                    if(map.ContainsKey(neighbor) && neighborScore < score.GetValueOrDefault(neighbor, int.MaxValue))
                    {
                        path[neighbor] = pos;
                        score[neighbor] = neighborScore;
                        pathsToEvaluate.Enqueue(neighbor);
                    }
                });
            } while (pathsToEvaluate.TryDequeue(out pos));
            return new List<(int X, int Y)>();
        }

        private IEnumerable<(int X, int Y)> GetNeighbors((int X, int Y) pos, int maxX, int maxY)
        {
            List<(int X, int Y)> res = new();
            var (X, Y) = pos;
            if(X < maxX)
                res.Add((X+1, Y));
            if(X > 0)
                res.Add((X-1, Y));
            if(Y > 0)
                res.Add((X, Y-1));
            if(Y < maxY)
                res.Add((X, Y+1));
            return res;
        }

        private List<(int X, int Y)> WalkPathBackwards(Dictionary<(int X, int Y), (int X, int Y)> path, (int X, int Y) pos)
        {
            List<(int X, int Y)> res = new();
            res.Add(pos);
            while(path.ContainsKey(pos))
            {
                pos = path[pos];
                res.Add(pos);
            }
            res.Reverse();
            return res;
        }
        
        private Dictionary<(int X, int Y), int> GrowMap(Dictionary<(int X, int Y), int> map)
        {
            var max = map.Max(p => p.Key.X)+1;
            var nmap = map.ToDictionary(d => d.Key, d => d.Value);
            nmap.Keys.ToList().ForEach(p =>
            {
                var threat = nmap[p];
                Enumerable.Range(1, 4).ForEach(i =>
                {
                    threat = ++threat > 9 ? 1 : threat;
                    nmap[(p.X, p.Y + (max * i))] = threat;
                });
            });
            nmap.Keys.ToList().ForEach(p =>
            {
                var threat = nmap[p];
                Enumerable.Range(1, 4).ForEach(i =>
                {
                    threat = ++threat > 9 ? 1 : threat;
                    nmap[(p.X + (max * i), p.Y)] = threat;
                });
            });
            return nmap;
        }
    }
}
