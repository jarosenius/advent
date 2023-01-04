using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day08 : Day
    {
        public Day08() : base(8, 2020)
        {

        }

        public override object Part1(List<string> input)
        {

            return RunInstructions(input);
        }
        public override object Part2(List<string> input)
        {
            var instr = input.ToArray();
            var res = new List<Tuple<long, bool>>();
            for (int i = 0; i < instr.Count(); i++)
            {
                var op = instr[i].Split(' ');
                var num = int.Parse(op[1]);
                if (op[0] == "jmp")
                {
                    var ni = instr.ToArray();
                    ni[i] = $"nop {num}";
                    res.Add(IsInfinite(ni));
                }
            }
            var item = res.FirstOrDefault(itm => itm.Item2 == false);

            return item.Item1;
        }

        private static long RunInstructions(IEnumerable<string> input)
        {
            var acc = 0;
            var visited = new Dictionary<int, string>();
            var instr = input.ToArray();
            var cnt = 0;
            for (int i = 0; i < instr.Count(); i++)
            {
                cnt++;
                var op = instr[i].Split(' ');
                var num = int.Parse(op[1]);
                if (visited.ContainsKey(i))
                {
                    if (visited[i] == instr[i])
                    {
                        return acc;
                    }
                }
                visited[i] = instr[i];
                if (op[0] == "nop")
                    continue;
                else if (op[0] == "acc")
                {
                    acc += num;
                    continue;
                }
                else if (op[0] == "jmp")
                {
                    i += (num > 0 ? num - 1 : num - 1);
                }
            }
            return acc;
        }

        private static Tuple<long, bool> IsInfinite(IEnumerable<string> input)
        {
            var acc = 0L;
            var visited = new Dictionary<int, string>();
            var instr = input.ToArray();
            bool infinite = false;
            var cnt = 0;
            for (int i = 0; i < instr.Count(); i++)
            {
                cnt++;
                var op = instr[i].Split(' ');
                var num = int.Parse(op[1]);
                if (visited.ContainsKey(i))
                {
                    if (visited[i] == instr[i])
                    {
                        infinite = true;
                        break;
                    }
                }
                visited[i] = instr[i];
                if (op[0] == "nop")
                    continue;
                else if (op[0] == "acc")
                {
                    acc += num;
                    continue;
                }
                else if (op[0] == "jmp")
                {
                    i += (num > 0 ? num - 1 : num - 1);
                }
            }

            return Tuple.Create(acc, infinite);
        }
    }
}
