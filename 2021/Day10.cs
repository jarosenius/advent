using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2021
{
    [AoC(2021)]
    public class Day10 : Day
    {
        public Day10() : base(10, 2021)
        {

        }

        public override object Part1(List<string> input)
        {
            Dictionary<char, long> cost = new() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            return input.Select(line =>
            {
                var (end, s) = GetUntilErrorOrComplete(line);
                if (line.Length == end)
                    return 0;
                return s.Count > 0 ? cost[line.Skip(end).FirstOrDefault()] : 0;
            }).Sum();
        }
        public override object Part2(List<string> input)
        {
            Dictionary<char, long> cost = new() { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };
            var x =
            input.Select(line =>
            {
                var (end, s) = GetUntilErrorOrComplete(line);
                return (LineLength: line.Length, ErrorPosOrEnd: end, s);
            })
            .Where(r => r.LineLength == r.ErrorPosOrEnd && r.s.Count > 0)
            .Select(r =>
            {
                var res = 0L;
                while (r.s.Count > 0)
                {
                    res *= 5;
                    res += cost[r.s.Pop()];
                }

                return res;
            })
            .OrderBy(r => r).ToList();

            return x.Skip(x.Count / 2).First();
        }

        private (int ErrorPosOrEnd, Stack<char> Stack) GetUntilErrorOrComplete(string line)
        {
            Stack<char> s = new();
            var untilError = line.TakeWhile(c =>
            {
                char checkFor;
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        s.Push(c);
                        return true;
                    case ')':
                        checkFor = '(';
                        break;
                    case ']':
                        checkFor = '[';
                        break;
                    case '}':
                        checkFor = '{';
                        break;
                    case '>':
                        checkFor = '<';
                        break;
                    default:
                        return false;
                }
                if (s.Peek() == checkFor)
                {
                    s.Pop();
                    return true;
                }
                else
                    return false;
            }).ToArray();
            return (ErrorPosOrEnd: untilError.Length, s);
        }
    }
}
