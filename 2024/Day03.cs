using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day03() : Day(3, 2024)
{
    public override object Part1(List<string> input)
    {
        var pattern = "(mul\((\d{1,3}),(\d{1,3})\))";
        var reg = new Regex(pattern, RegexOptions.Compiled);
        var mem = string.Join("",input);
        var match = reg.Matches(mem);
        return null;
    }
    public override object Part2(List<string> input)
    {
        return null;
    }
}

