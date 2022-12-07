using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Advent.y2021
{
    [AoC(2021)]
    public class Day03 : Day
    {
        public Day03() : base(3, 2021)
        {

        }

        public override long Part1(List<string> input)
        {
            StringBuilder sb = new();

            for (var i = 0; i < input.ElementAt(0).Length; i++)
            {
                var r = GetMostCommonAtPosition(input, i);
                sb.Append(r.Ones > r.Zeroes ? "1" : "0");
            }
            var str = sb.ToString();
            var inverted = Utils.InvertBinaryString(str);

            var res = Utils.ConvertFromBinaryString(str) * Utils.ConvertFromBinaryString(inverted);

            return res;
        }

        public override long Part2(List<string> input)
        {
            var oxygen = GetData(input, r => r.Ones >= r.Zeroes);
            var co2 = GetData(input, r => r.Ones < r.Zeroes);

            return oxygen * co2;
        }

        private long GetData(IEnumerable<string> input, Func<(int Ones, int Zeroes), bool> IsMatch)
        {
            var modifiedInput = input.ToList();
            for (var i = 0; i < input.ElementAt(0).Length; i++)
            {
                var r = GetMostCommonAtPosition(modifiedInput, i);
                modifiedInput = GetDataFromMostCommon(modifiedInput, i, IsMatch(r));

                if (modifiedInput.Count == 1)
                    break;
            }
            return Utils.ConvertFromBinaryString(modifiedInput[0]);
        }

        private (int Ones, int Zeroes) GetMostCommonAtPosition(IEnumerable<string> input, int i)
        {
            var ones = 0;
            var zeroes = 0;
            foreach (var s in input)
            {
                if (s[i] == '1')
                    ones++;
                else if (s[i] == '0')
                    zeroes++;
            }

            return (ones, zeroes);
        }
        private List<string> GetDataFromMostCommon(IEnumerable<string> input, int i, bool match)
        {
            List<string> r = new();
            foreach (var s in input)
            {
                if (s[i] == '1' && match
                    || s[i] == '0' && !match)
                    r.Add(s);
            }
            return r;
        }
    }
}
