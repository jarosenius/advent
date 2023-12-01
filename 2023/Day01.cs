using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2023;

[AoC(2023)]
public class Day01 : Day
{
    public Day01() : base(1, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        return input
            .Select(row => 
                int.Parse(
                    $"{Regex.Match(row, @"\d")}{Regex.Match(row, @"\d", RegexOptions.RightToLeft)}")
            ).Sum();
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }
}

