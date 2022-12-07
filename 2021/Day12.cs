using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Advent.y2021
{
    [AoC(2021)]
    public class Day12 : Day
    {
        public Day12() : base(12, 2021)
        {
        }

        public override long Part1(List<string> input)
        {
            ConcurrentDictionary<string, Cave> Caves = new();
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
            return start.ConnectedCaves.Sum(c => c.GetPathsToEnd(new List<string>() { "start" }, false));
        }
        public override long Part2(List<string> input)
        {
            ConcurrentDictionary<string, Cave> Caves = new();
            input.ForEach(l =>
            {
                var parts = l.Split('-');
                var caves = parts.Select(c => Caves.GetOrAdd(c, nc => new Cave(nc)));
                caves.ElementAt(0).AddConnectedCave(caves.ElementAt(1));
            });
            Caves.TryGetValue("start", out var start);
            return start.ConnectedCaves.Sum(c => c.GetPathsToEnd(new List<string>() { "start" }, true));
        }

        private class Cave
        {
            public string Name { get; private set; }
            public bool IsStart { get; private set; }
            public bool IsEnd { get; private set; }
            public bool IsLarge { get; private set; }
            public HashSet<Cave> ConnectedCaves { get; set; }
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
                if (ConnectedCaves.Contains(cave))
                    return;
                ConnectedCaves.Add(cave);
                cave.AddConnectedCave(this);
            }

            public void RemoveConnectedCave(Cave cave)
            {
                if (ConnectedCaves.Contains(cave) == false)
                    return;
                ConnectedCaves.Remove(cave);
                cave.RemoveConnectedCave(this);
            }

            public long GetPathsToEnd(List<string> visits, bool visitTwice)
            {
                if (IsEnd)
                    return 1;

                if (CanVisit(visits, visitTwice) == false)
                    return 0;

                if (!IsEnd || !IsLarge || !IsStart)
                    visits.Add(Name);

                return ConnectedCaves.Where(c =>
                            !c.IsStart
                            && (c.IsLarge || (c.CanVisit(visits, visitTwice))))
                        .Sum(c => c.GetPathsToEnd(visits.ToList(), visitTwice));
            }
            private bool CanVisit(List<string> visits, bool visitTwice)
            {
                if (IsLarge)
                    return true;

                if (visitTwice == false && visits.Contains(Name))
                    return false;
                else if (visitTwice && visits.Count(v => v == Name) == 2)
                    return false;
                else if (visitTwice && visits.Contains(Name) && !CanVisitSmallCave(visits))
                    return false;
                return true;
            }

            private bool CanVisitSmallCave(List<string> visits)
            {
                var smallCaveVisits = visits.Where(v => char.IsLower(v[0]));
                return smallCaveVisits.Count() == smallCaveVisits.Distinct().Count();
            }

        }
    }
}
