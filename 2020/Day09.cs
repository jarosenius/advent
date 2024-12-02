using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    [AoC(2020)]
    public class Day09 : Day
    {
        public Day09() : base(9, 2020)
        {

        }

        public override object Part1(List<string> input)
        {
            return Part1(input, 25);
        }
        static long Part1(IEnumerable<string> input, int save)
        {
            var numbers = input.Select(long.Parse);
            var toSave = numbers.Take(save).ToList();
            for (int i = save; i < numbers.Count(); i++)
            {
                var num = numbers.ElementAt(i);
                var isValid = IsNumberValid(num, toSave, save);
                if (isValid)
                {
                    toSave.RemoveAt(0);
                    toSave.Add(num);
                }
                else
                    return num;
            }

            return -1;
        }
        private static bool IsNumberValid(long num, List<long> toSave, int save)
        {
            if (toSave.Count() < save)
            {
                toSave.Add(num);
                return true;
            }

            var valid = false;
            for (int i = 0; i < toSave.Count(); i++)
            {
                var a = toSave[i];
                valid = toSave.Skip(i + 1).Any(b => b + a == num);
                if (valid == true)
                    return true;
            }
            return valid;
        }

        public override object Part2(List<string> input)
        {
            var part1 = Part1(input, 25);
            return
           Part2(input, 2, part1);
        }

        static long Part2(IEnumerable<string> input, int startWith, long lookFor)
        {
            var numbers = input.Select(long.Parse);
            var toSave = numbers.Take(startWith).ToList();
            for (int i = startWith; i < numbers.Count(); i++)
            {
                var num = numbers.ElementAt(i);
                while (toSave.Sum() + num > lookFor)
                {
                    toSave.RemoveAt(0);
                }

                toSave.Add(num);
                if (toSave.Sum() == lookFor)
                {
                    var min = toSave.Min();
                    var max = toSave.Max();
                    return min + max;
                }
            }
            return -1;
        }
    }
}
