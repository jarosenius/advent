using System.Collections.Generic;
using System.Linq;
namespace Advent.y2022;

[AoC(2022)]
public class Day02 : Day
{
    public Day02() : base(2, 2022)
    {

    }

    public override long Part1(List<string> input)
    {
        var scores = new Dictionary<string, int>()
        {
            {"XA", 1+3},
            {"XB", 1+0},
            {"XC", 1+6},
            {"YA", 2+6},
            {"YB", 2+3},
            {"YC", 2+0},
            {"ZA", 3+0},
            {"ZB", 3+6},
            {"ZC", 3+3},
        };

        return input.Sum(row => scores[$"{row[2..]}{row[..1]}"]);
    }
    public override long Part2(List<string> input)
    {
        var scores = new Dictionary<string, int>()
        {
            {"XA", 0+3},
            {"XB", 0+1},
            {"XC", 0+2},
            {"YA", 3+1},
            {"YB", 3+2},
            {"YC", 3+3},
            {"ZA", 6+2},
            {"ZB", 6+3},
            {"ZC", 6+1},
        };

        return (from row in input select scores[$"{row[2]}{row[0]}"]).Sum();
    }
}

