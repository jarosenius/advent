using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent.y2023;

[AoC(2023)]
public class Day06 : Day
{
    public Day06() : base(6, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var infos = GetInfo(input);
        return infos.Aggregate(1L, (res, i) => res *= CountNumberOfWays(i));
    }

    public override object Part2(List<string> input)
    {
        var time = int.Parse(input[0][5..].Replace(" ", ""));
        var distance = long.Parse(input[1][9..].Replace(" ", ""));
        var info = new Info(time, distance);
        return CountNumberOfWays(info);
    }

    private static int CountNumberOfWays(Info info)
    {
        // travelTime = raceTime - buttonPressTime
        // distanceTraveled = travelTime * buttonPressTime
        // => distanceTraveled = (raceTime - buttonPressTime) * buttonPressTime
        // => distanceTraveled = (raceTime * buttonPressTime) - buttonPressTime²
        // => buttonPressTime² - (raceTime * buttonPressTime) + distanceTraveled = 0
        // ax² + bx + c = 0 => x = (-b ± sqrt(b² - 4ac)) / 2a
        
        var sqrt = Math.Sqrt(Math.Pow(info.Time, 2) - 4 * info.Distance);
        var x1 = (info.Time + sqrt) / 2;
        var x2 = (info.Time - sqrt) / 2;
        return (int)(Math.Ceiling(x1) - Math.Floor(x2) - 1);
    }
        
    private static IEnumerable<Info> GetInfo(List<string> input)
    {
        var time = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value));
        var distance = Regex.Matches(input[1], @"\d+").Select(m => long.Parse(m.Value));
        return time.Zip(distance).Select(z => new Info(z.First, z.Second));
    }

    private record Info(int Time, long Distance);
}
