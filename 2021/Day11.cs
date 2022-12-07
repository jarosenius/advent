using System;
using System.Collections.Generic;
using System.Linq;


namespace Advent.y2021
{
    [AoC(2021)]
    public class Day11 : Day
    {
        public Day11() : base(11, 2021)
        {

        }

        public override long Part1(List<string> input)
        {
            var octopi = input.Select((l, y) => l.Select((c, x) => new Octopus(int.Parse(c.ToString()), x, y)).ToArray()).ToArray();
            var flat = octopi.SelectMany(o => o);
            flat.ForEach(o => o.AddNeighbors(octopi.GetNeighbors(o.X, o.Y, true)));
            return Enumerable.Range(0, 100).Sum(i => flat.Sum(o => o.IncreaseEnergy(i).Flashes));
        }
        public override long Part2(List<string> input)
        {
            var octopi = input.Select((l, y) => l.Select((c, x) => new Octopus(int.Parse(c.ToString()), x, y)).ToArray()).ToArray();
            var flat = octopi.SelectMany(o => o).ToList();
            flat.ForEach(o => o.AddNeighbors(octopi.GetNeighbors(o.X, o.Y, true)));

            var i = 0;
            do
            {
                flat.ForEach(o => o.IncreaseEnergy(i));
                i++;
            } while (flat.All(o => o.Flashed) == false);

            return i;
        }

        private class Octopus
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public bool Flashed { get; private set; }
            public int EnergyLevel { get; private set; } = 0;
            private List<Octopus> neighbors = new();
            private int lastStep = -1;
            public Octopus(int energyLevel, int x, int y)
            {
                X = x;
                Y = y;
                EnergyLevel = energyLevel;
                neighbors = new();
            }
            public void AddNeighbors(IEnumerable<Octopus> n)
            {
                var toAdd = n.Except(neighbors);
                if (toAdd.Any() == false)
                    return;
                neighbors.AddRange(toAdd);
            }

            public (long Flashes, bool Flashed) IncreaseEnergy(int step)
            {
                if (step > lastStep && Flashed)
                {
                    EnergyLevel = 0;
                    Flashed = false;
                }

                EnergyLevel++;
                if (Flashed && lastStep == step)
                    return (0, true);
                lastStep = step;
                if (EnergyLevel > 9)
                {
                    Flashed = true;
                    return (neighbors.Sum(n => n.IncreaseEnergy(step).Flashes) + 1, true);
                }
                return (0, false);
            }
        }
    }
}
