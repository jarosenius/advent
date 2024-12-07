using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day03() : Day(3, 2024)
{
    public override object Part1(List<string> input)
    {
        var reg = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);
        return reg.Matches(string.Join("", input)).Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
    }
    public override object Part2(List<string> input)
    {
        var reg = new Regex(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled);
        var matches = reg.Matches(string.Join("", input));
        return matches.Aggregate((0,false), (data, m) =>
        {
            var skip = (m.Groups[0].Value == "don't()" || data.Item2) && m.Groups[0].Value != "do()";
            return skip || m.Groups[0].Value == "do()" 
                ? data with { Item2 = skip } 
                : data with { Item1 = data.Item1 + int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value), Item2 = skip };
        }).Item1;
    }
}

