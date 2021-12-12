using System.ComponentModel;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2021
{
    public class Day12 : Day
    {
        ConcurrentDictionary<string, Cave> Caves;
        public Day12() : base(12, 2021)
        {
        }

        public override long Part1(List<string> input)
        {
            Caves = new();
            input.ForEach(l => 
            {
                var parts = l.Split('-');
                var caves = parts.Select(c => Caves.GetOrAdd(c, nc => new Cave(nc)));
                caves.ElementAt(0).AddConnectedCave(caves.ElementAt(1));
            });
            Caves.Values.Where(c => c.IsStart == false && c.IsEnd == false && c.ConnectedCaves.Count == 1 && !c.IsLarge && !c.ConnectedCaves.First().IsLarge).ForEach(d => 
            {
                d.RemoveConnectedCave(d.ConnectedCaves.First());
                Caves.TryRemove(d.Name, out _);
            });
            Caves.TryGetValue("start", out var start);
            var sum = 0L;
            start.ConnectedCaves.ForEach(c =>
            {
                sum += c.GetPathsToEnd(new List<string>(){"start"});
            });
            return sum;
        }
        public override long Part2(List<string> input)
        {
             return 0;
        }

        private class Cave
        {
            public string Name {get;private set;}
            public bool IsStart {get;private set;}
            public bool IsEnd {get;private set;}
            public bool IsLarge {get;private set;}
            public HashSet<Cave> ConnectedCaves{get;set;}
            public Cave(string name)
            {
                Name = name;
                ConnectedCaves = new();
                IsStart = name == "start";
                IsEnd = name == "end";
                IsLarge = char.IsUpper(name[0]);
            }

            public void AddConnectedCave(Cave cave) 
            {
                if(ConnectedCaves.Contains(cave))
                    return;
                ConnectedCaves.Add(cave);
                cave.AddConnectedCave(this);
            }

            public void RemoveConnectedCave(Cave cave) 
            {
                if(ConnectedCaves.Contains(cave) == false)
                    return;
                ConnectedCaves.Remove(cave);
                cave.RemoveConnectedCave(this);
            }

            public long GetPathsToEnd(List<string> visits)
            {
                if(IsEnd)
                    return 1;
                    
                if(visits.Contains(Name) && !IsLarge)
                    return 0;
                var sum = 0L;
                var filtered = ConnectedCaves.Where(c => !c.IsStart && (c.IsLarge || !visits.Contains(c.Name)));
                visits.Add(Name);

                filtered.ForEach(c =>
                {
                    var s = c.GetPathsToEnd(visits.ToList());
                    
                    sum += s;
                });
                
                return sum;
            }
        }
    }
}
