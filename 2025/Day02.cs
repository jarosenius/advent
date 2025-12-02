using System.Collections.Generic;
using System.Linq;

namespace Advent.y2025;

[AoC(2025)]
public class Day02() : Day(2, 2025)
{
    public override object Part1(List<string> input) => input[0].SplitByFirstThenBySecondAndParseToLong(",", "-").Sum(p => new ProductIdRange(p[0], p[1]).SumOfInvalidIds(false));
    public override object Part2(List<string> input) => input[0].SplitByFirstThenBySecondAndParseToLong(",", "-").Sum(p => new ProductIdRange(p[0], p[1]).SumOfInvalidIds(true));

    private record ProductIdRange(long Start, long End)
    {
        public long SumOfInvalidIds(bool allRepeatingPatterns)
        {
            var sum = 0L;
            for (var i = Start; i <= End; i++)
            {
                if (IsRepeating(i, allRepeatingPatterns))
                    sum += i;
            }
            return sum;
        }

        private static bool IsRepeating(long number, bool anyRepetition)
        {
            var str = number.ToString();
            var length = str.Length;

            if (!anyRepetition && length % 2 != 0)
                return false;

            var maxLength = length / 2;

            for (var patternLength = 1; patternLength <= maxLength; patternLength++)
            {
                if (length % patternLength != 0)
                    continue;

                if (!anyRepetition && patternLength != maxLength)
                    continue;

                var match = true;
                for (var i = patternLength; i < length; i++)
                {
                    if (str[i] == str[i % patternLength])
                        continue;
                    match = false;
                    break;
                }

                if (match)
                    return true;
            }

            return false;
        }
    }
}

