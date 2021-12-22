using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2021
{
    public class Day22 : Day
    {
        public Day22() : base(22, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            return RebootReactor(input, true, 50);
        }
        public override long Part2(List<string> input)
        {
            return RebootReactor(input);
        }

        private long RebootReactor(List<string> input, bool limit = false, int lim = 50)
        {
            var instructions = input.Select(r => r.Split(new[]{',', ' '},StringSplitOptions.RemoveEmptyEntries)).Select(i => {
                var x = i[1][2..].Split("..").Select(int.Parse).ToArray();
                var y = i[2][2..].Split("..").Select(int.Parse).ToArray();
                var z = i[3][2..].Split("..").Select(int.Parse).ToArray();
                return new Instruction(i[0] == "on", new Cuboid(new Range(x[0], x[1]), new Range(y[0], y[1]), new Range(z[0], z[1])));
            }).ToArray();
            List<Cuboid> state = new();
            var instr = limit ? instructions.Where(i => i.RangesWithinLimit(lim)) : instructions;
            instr.ForEach(i =>{
                List<Cuboid> tmpState = new();
                if(i.On)
                    tmpState.Add(i.Cuboid);      
                state.ForEach(s => tmpState.AddRange(s.SymetricDifference(i.Cuboid)));
                state = tmpState;
            });

            return state.Sum(s => s.X.Count * s.Y.Count * s.Z.Count);
        }

        private record Range(long From, long To)
        {
            public long Count => 1+To-From;
            public bool WithinLimit(int limit) => From >= limit*-1 && To <= limit;
        }
        private record Cuboid(Range X, Range Y, Range Z)
        {
            public Cuboid Intersection(Cuboid c)
            {
                var xr = new Range(Math.Max(X.From, c.X.From), Math.Min(X.To, c.X.To));
                var yr = new Range(Math.Max(Y.From, c.Y.From), Math.Min(Y.To, c.Y.To));
                var zr = new Range(Math.Max(Z.From, c.Z.From), Math.Min(Z.To, c.Z.To));
                return new Cuboid(xr, yr, zr);
            }
            public List<Cuboid> SymetricDifference(Cuboid c)
            {
                var intersection = Intersection(c);
                if(!intersection.Valid)
                    return new(){this};

                List<Cuboid> res = new();    
                if(intersection.X.From > X.From)
                    res.Add(new Cuboid(new Range(X.From, intersection.X.From-1), Y, Z));
                if(intersection.X.To < X.To)
                    res.Add(new Cuboid(new Range(intersection.X.To+1, X.To), Y, Z));

                if(intersection.Y.From > Y.From)
                    res.Add(new Cuboid(intersection.X, new Range(Y.From, intersection.Y.From-1), Z));
                if(intersection.Y.To < Y.To)
                    res.Add(new Cuboid(intersection.X, new Range(intersection.Y.To+1, Y.To), Z));

                if(intersection.Z.From > Z.From)
                    res.Add(new Cuboid(intersection.X, intersection.Y, new Range(Z.From, intersection.Z.From-1)));
                if(intersection.Z.To < Z.To)
                    res.Add(new Cuboid(intersection.X, intersection.Y, new Range(intersection.Z.To+1, Z.To)));

                return res;
            }
            public bool Valid => X.From <= X.To && Y.From <= Y.To && Z.From <= Z.To;
        }
        private record Instruction(bool On, Cuboid Cuboid)
        {
            public bool RangesWithinLimit(int limit) => Cuboid.X.WithinLimit(limit) && Cuboid.Y.WithinLimit(limit) && Cuboid.Z.WithinLimit(limit);
        }
    }
}
