using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day14 : Day
    {
        public Day14() : base(14, 2020)
        {
            
        }

        public override long Part1(IEnumerable<string> input)
        {
            var memory = new Dictionary<int, long>();
            var mask = "";

            foreach (var instruction in input)
            {
                var parts = instruction.Split(" = ", StringSplitOptions.RemoveEmptyEntries);
                if (parts[0] == "mask") 
                    mask = parts[1];
                else
                {
                    //Console.WriteLine("----------------------------------------------");
                    var memPos = int.Parse(parts[0].Replace("mem[", "").Replace("]", ""));
                    var maskedValue = ApplyMask(long.Parse(parts[1]), mask, false);
                    memory[memPos] = ConvertFromBinary(maskedValue);

                }
            }

            
            return memory.Values.Sum();
        }
        public override long Part2(IEnumerable<string> input)
        {
            var memory = new Dictionary<long, long>();
            var mask = "";

            foreach (var instruction in input)
            {
                var parts = instruction.Split(" = ", StringSplitOptions.RemoveEmptyEntries);
                if (parts[0] == "mask")
                    mask = parts[1];
                else
                {
                    //Console.WriteLine("----------------------------------------------");
                    var memPos = int.Parse(parts[0].Replace("mem[", "").Replace("]", ""));
                    var maskedValue = ApplyMask(memPos, mask);

                    var branches = CreateBranchesFromMask(maskedValue);
                    foreach (var newMask in branches)
                        memory[ConvertFromBinary(newMask)] = int.Parse(parts[1]);
                }
            }

            
            return memory.Values.Sum();
        }

        static long ConvertFromBinary(string bin, int i = 0) 
        {
            return (long)bin.Select(c => c == '1' ? Math.Pow(2, i++) : 0 * i++).Sum();
        }
        static string ApplyMask(long num, string mask, bool part2 = true)
        {
            var val = ConvertToBinary(num);
            //Console.WriteLine($"value:  {val}  (decimal {num})");
            //Console.WriteLine($"mask:   {mask}");
            return string.Join("", Mask(mask.ToArray(), val.ToArray(), part2));
        }
        static IEnumerable<char> Mask(char[] mask, char[] number, bool part2)
        {
            var minLength = Math.Min(mask.Length, number.Length);
            var merged = new char[minLength];
            for (var i = 0; i < minLength; i++)
            {
                if (mask[i] == '1' || mask[i] == (part2 ? 'X' : '0'))
                    merged[i] = mask[i];
                else if(mask[i] == 'X')
                {
                    merged[i] = number[i];
                }
                else
                    merged[i] = number[i];
            }
            //Console.WriteLine($"result: {string.Join("", merged)}  (decimal {ConvertFromBinary(string.Join("", merged.Reverse()))})");

            return merged.Reverse();
        }

        static string ConvertToBinary(long num)
        {
            var bin = Convert.ToString(num, 2);
            return $"{string.Join("", Enumerable.Repeat("0", 36 - bin.Length))}{bin}";
        }

        static IEnumerable<string> CreateBranchesFromMask(string mask)
        {
            var res = new List<string>();
            var idx = mask.IndexOf('X');
            var modMask = mask.Remove(idx, 1);
            res.AddRange(new[] { modMask.Insert(idx, "0"), modMask.Insert(idx, "1") });
            if (res[0].Contains("X") == false) 
                return res.ToArray();
            res.AddRange(CreateBranchesFromMask(res[0]).Union(CreateBranchesFromMask(res[1])));
            return res;
        }
    }
}
