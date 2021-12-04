using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day17 : Day
    {
        public Day17() : base(17, 2020)
        {
            
        }

        public override long Part1(IEnumerable<string> input)
        {
            var c = new Conway(input);
            
            return c.CountActiveCubesAfter(6);
        }
        public override long Part2(IEnumerable<string> input)
        {
            var c = new Conway4d(input);
            
            return c.CountActiveCubesAfter(6);
        }

        private class Conway
        {
            public HashSet<(int x, int y, int z)> active;
            private HashSet<(int x, int y, int z)> temp;
            private readonly Dictionary<(int x, int y, int z), int> neighbors;
            private static readonly List<(int x, int y, int z)> offsets = new List<(int, int, int)>()
            {
                (1, 1,  0), (1, 0,  0), (1, -1,  0), (0, -1,  0), (-1, -1,  0), (-1, 0,  0), (-1, 1,  0), (0, 1,  0),
                (1, 1,  1), (1, 0,  1), (1, -1,  1), (0, -1,  1), (-1, -1,  1), (-1, 0,  1), (-1, 1,  1), (0, 1,  1), (0, 0,  1),
                (1, 1, -1), (1, 0, -1), (1, -1, -1), (0, -1, -1), (-1, -1, -1), (-1, 0, -1), (-1, 1, -1), (0, 1, -1), (0, 0, -1)
            };

            public Conway(IEnumerable<string> input)
            {
                active = new HashSet<(int, int, int)>();
                temp = new HashSet<(int, int, int)>();
                neighbors = new Dictionary<(int x, int y, int z), int>();

                for (int x = 0; x < input.Count(); x++)
                {
                    for (int y = 0; y < input.ElementAt(x).Length; y++)
                    {
                        if (input.ElementAt(x)[y] == '#')
                        {
                            active.Add((x, y, 0));
                        }
                    }
                }
            }

            private void Run()
            {
                neighbors.Clear();
                foreach (var cube in active)
                {
                    CheckNeighbors(cube);
                }

                temp.Clear();
                foreach (var (cube, count) in neighbors)
                {
                    if (count == 2 && active.Contains(cube))
                    {
                        temp.Add(cube);
                    }
                    else if (count == 3)
                    {
                        temp.Add(cube);
                    }
                }

                var tmp = active;
                active = temp;
                temp = tmp;
            }

            private void CheckNeighbors((int x, int y, int z) cube)
            {
                foreach (var (x, y, z) in offsets)
                {
                    var pos = (cube.x + x, cube.y + y, cube.z + z);
                    if (neighbors.ContainsKey(pos))
                    {
                        neighbors[pos]++;
                    }
                    else
                    {
                        neighbors[pos] = 1;
                    }
                }
            }
            public long CountActiveCubesAfter(int cycles)
            {
                for (int i = 0; i < cycles; i++)
                {
                    Run();
                }

                return active.Count;
            }
        }

        private class Conway4d
        {
            public HashSet<(int x, int y, int z, int w)> active;
            private HashSet<(int x, int y, int z, int w)> temp;
            private readonly Dictionary<(int x, int y, int z, int w), int> neighbors;
            private static readonly List<(int x, int y, int z, int w)> offsets = new List<(int, int, int, int)>()
            {
                (1, 1, 0, 0), (1, 0, 0, 0), (1, -1, 0, 0), (0, -1, 0, 0), (-1, -1, 0, 0), (-1, 0, 0, 0), (-1, 1, 0, 0), (0, 1, 0, 0), (0, 0, 1, 0), (1, 1, 1, 0), (1, 0, 1, 0), (1, -1, 1, 0), (0, -1, 1, 0), (-1, -1, 1, 0), (-1, 0, 1, 0), (-1, 1, 1, 0), (0, 1, 1, 0), (0, 0, -1, 0), (1, 1, -1, 0), (1, 0, -1, 0), (1, -1, -1, 0), (0, -1, -1, 0), (-1, -1, -1, 0), (-1, 0, -1, 0), (-1, 1, -1, 0), (0, 1, -1, 0),
                (0, 0, 0, 1), (1, 1, 0, 1), (1, 0, 0, 1), (1, -1, 0, 1), (0, -1, 0, 1), (-1, -1, 0, 1), (-1, 0, 0, 1), (-1, 1, 0, 1), (0, 1, 0, 1), (0, 0, 1, 1), (1, 1, 1, 1), (1, 0, 1, 1), (1, -1, 1, 1), (0, -1, 1, 1), (-1, -1, 1, 1), (-1, 0, 1, 1), (-1, 1, 1, 1), (0, 1, 1, 1), (0, 0, -1, 1), (1, 1, -1, 1), (1, 0, -1, 1), (1, -1, -1, 1), (0, -1, -1, 1), (-1, -1, -1, 1), (-1, 0, -1, 1), (-1, 1, -1, 1),
                (0, 1, -1, 1), (0, 0, 0, -1), (1, 1, 0, -1), (1, 0, 0, -1), (1, -1, 0, -1), (0, -1, 0, -1), (-1, -1, 0, -1), (-1, 0, 0, -1), (-1, 1, 0, -1), (0, 1, 0, -1), (0, 0, 1, -1), (1, 1, 1, -1), (1, 0, 1, -1), (1, -1, 1, -1), (0, -1, 1, -1), (-1, -1, 1, -1), (-1, 0, 1, -1), (-1, 1, 1, -1), (0, 1, 1, -1), (0, 0, -1, -1), (1, 1, -1, -1), (1, 0, -1, -1), (1, -1, -1, -1), (0, -1, -1, -1), (-1, -1, -1, -1), (-1, 0, -1, -1), (-1, 1, -1, -1), (0, 1, -1, -1)
            };

            public Conway4d(IEnumerable<string> input)
            {
                active = new HashSet<(int, int, int, int)>();
                temp = new HashSet<(int, int, int, int)>();
                neighbors = new Dictionary<(int x, int y, int z, int w), int>();

                for (int x = 0; x < input.Count(); x++)
                {
                    for (int y = 0; y < input.ElementAt(x).Length; y++)
                    {
                        if (input.ElementAt(x)[y] == '#')
                        {
                            active.Add((x, y, 0, 0));
                        }
                    }
                }
            }

            private void Run()
            {
                neighbors.Clear();
                foreach (var cube in active)
                {
                    CheckNeighbors(cube);
                }

                temp.Clear();
                foreach (var (cube, count) in neighbors)
                {
                    if (count == 2 && active.Contains(cube))
                    {
                        temp.Add(cube);
                    }
                    else if (count == 3)
                    {
                        temp.Add(cube);
                    }
                }

                var tmp = active;
                active = temp;
                temp = tmp;
            }

            private void CheckNeighbors((int x, int y, int z, int w) cube)
            {
                foreach (var (x, y, z, w) in offsets)
                {
                    var pos = (cube.x + x, cube.y + y, cube.z + z, cube.w + w);
                    if (neighbors.ContainsKey(pos))
                    {
                        neighbors[pos]++;
                    }
                    else
                    {
                        neighbors[pos] = 1;
                    }
                }
            }
            public long CountActiveCubesAfter(int cycles)
            {
                for (int i = 0; i < cycles; i++)
                {
                    Run();
                }

                return active.Count;
            }
        }
    }
}
