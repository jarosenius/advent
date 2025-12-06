using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.y2025;

[AoC(2025)]
public class Day06() : Day(6, 2025)
{
    public override object Part1(List<string> input) => ParseProblems(input, false).Sum(p => p.Calculate());

    public override object Part2(List<string> input) => ParseProblems(input, true).Sum(p => p.Calculate());

    private static List<Problem> ParseProblems(List<string> input, bool cephalopodMath)
    {
        return [.. from column in SplitIntoColumns(input, cephalopodMath)
                let operation = column[^1].Trim()
                let numbers = column.Take(column.Count - 1).Select(c => long.Parse(c.Trim())).ToList()
                where numbers.Count > 0
                select new Problem(numbers, operation)];
    }

    private static List<List<string>> SplitIntoColumns(List<string> input, bool cephalopodMath)
    {
        if (input.Count == 0)
            return [];

        var columns = CreateColumns(input);
        return cephalopodMath
            ? [.. columns.Select(GetCephalopodMathColumn)]
            : columns;
    }

    private static List<string> GetCephalopodMathColumn(List<string> column)
    {
        var op = column[^1].Trim();
        var newColumn = new List<string>();
        var maxLength = column.Max(r => r.Trim().Length);
        var withoutOp = column.Take(column.Count - 1).ToList();
        for (var i = 0; i < maxLength; i++)
        {
            newColumn.Add(new string(withoutOp.Select(r => r[i]).ToArray()));
        }
        return [.. newColumn, op];
    }

    private static List<List<string>> CreateColumns(List<string> input)
    {
        if (input.Count == 0)
            return [];

        var length = input[0].Length;
        var result = new List<List<string>>();
        var d = new ConcurrentDictionary<int, StringBuilder>();

        for (var i = 0; i < length; i++)
        {
            var chars = input.Select(r => r[i]).ToArray();
            if (chars.All(c => c == ' '))
            {
                if (d.Count > 0)
                {
                    result.Add(d.OrderBy(kv => kv.Key).Select(kv => kv.Value.ToString()).ToList());
                    d.Clear();
                }
                continue;
            }

            for (var j = 0; j < chars.Length; j++)
            {
                d.AddOrUpdate(j, _ =>
                {
                    var sb = new StringBuilder();
                    sb.Append(chars[j]);
                    return sb;
                }, (_, existing) =>
                {
                    existing.Append(chars[j]);
                    return existing;
                });
            }

        }
        result.Add(d.OrderBy(kv => kv.Key).Select(kv => kv.Value.ToString()).ToList());

        return result;
    }

    private record Problem(List<long> Numbers, string Operation)
    {
        public long Calculate()
        {
            if (Numbers.Count == 0)
                return 0;

            var result = Numbers[0];
            for (var i = 1; i < Numbers.Count; i++)
            {
                result = Operation == "+" ? result + Numbers[i] : result * Numbers[i];
            }
            return result;
        }
    }
}

