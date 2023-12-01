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
        return input.Select(row => GetNumber(row, @"\d")).Sum();
    }
    public override object Part2(List<string> input)
    {
        return input.Select(row => GetNumber(row, @"\d|one|two|three|four|five|six|seven|eight|nine")).Sum();
    }

    private int GetNumber(string row, string pattern)
    {
        var first = Regex.Match(row, pattern).Value;
        var last = Regex.Match(row, pattern, RegexOptions.RightToLeft).Value;
        return GetNumber(first)*10 + GetNumber(last);
    }

    private int GetNumber (string num) => num switch
        {
            "" => 0,
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => int.Parse(num)
        };
}

