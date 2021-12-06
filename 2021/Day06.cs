using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2021
{
    public class Day06 : Day
    {
        public Day06() : base(6, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var lanterns = input.First().SplitByAndParseToInt(",").Select(n => new Lanternfish(n, false)).ToList();

            for (int i = 0; i < 80; i++)
            {
                var newLanterns = lanterns.Count(l => l.Tick() == true);
                for (int j = 0; j < newLanterns; j++)
                {
                    lanterns.Add(new Lanternfish());
                }
            }

            return lanterns.Count();
        }
        public override long Part2(List<string> input)
        {
            var lanterns = input.First().SplitByAndParseToInt(",").GroupBy(n => n).ToDictionary(g => g.Key, g => (long)g.Count());    
            for (int i = 0; i < 256; i++)
            {
                lanterns = Tick(lanterns);
            }
            return lanterns.Values.Sum();
        }

        internal static Dictionary<int, long> Tick(Dictionary<int, long> lanterns)
        {
            return new Dictionary<int, long>()
            {
                [8] = lanterns.GetValueOrDefault(0),
                [7] = lanterns.GetValueOrDefault(8),
                [6] = lanterns.GetValueOrDefault(7) + lanterns.GetValueOrDefault(0),
                [5] = lanterns.GetValueOrDefault(6),
                [4] = lanterns.GetValueOrDefault(5),
                [3] = lanterns.GetValueOrDefault(4),
                [2] = lanterns.GetValueOrDefault(3),
                [1] = lanterns.GetValueOrDefault(2),
                [0] = lanterns.GetValueOrDefault(1),
            };
        }

        private class Lanternfish
        {
            public bool IsNew {get;set;} = false;
            public int InternalTimer {get;set;}
            public Lanternfish(int initialInternalTimer, bool isNew)
            {
                InternalTimer = initialInternalTimer;
                IsNew = isNew;
                if(isNew)
                    InternalTimer+=2;
            }
            public Lanternfish() : this(6, true)
            {

            }

            public bool Tick()
            {
                if(InternalTimer == 0)
                {
                    InternalTimer = 6;
                    return true;
                }
                else if(InternalTimer <= 6)
                {
                    IsNew = false;
                }

                InternalTimer--;
                return false;
            }
        }
    }
}
