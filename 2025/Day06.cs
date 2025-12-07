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
        var withoutOp = column.Take(column.Count - 1).ToList();
        var maxLength = withoutOp.Max(r => r.Trim().Length);
        var newColumn = new List<string>(maxLength + 1);

        var sb = new StringBuilder(withoutOp.Count);
        for (var i = 0; i < maxLength; i++)
        {
            foreach (var trimmed in withoutOp.Select(row => row.Trim()))
            {
                sb.Append(i < trimmed.Length ? trimmed[i] : ' ');
            }
            newColumn.Add(sb.ToString());
            sb.Clear();
        }

        newColumn.Add(op);
        return newColumn;
    }

    private static List<List<string>> CreateColumns(List<string> input)
    {
        if (input.Count == 0)
            return [];

        var length = input[0].Length;
        var result = new List<List<string>>();
        var column = new StringBuilder[input.Count];

        for (var i = 0; i < length; i++)
        {
            var chars = input.Select(r => r[i]).ToArray();
            if (chars.All(c => c == ' '))
            {
                if (column[0] != null)
                {
                    result.Add([.. column.Select(sb => sb?.ToString() ?? "")]);
                    column = new StringBuilder[input.Count];
                }
            }

            for (var j = 0; j < chars.Length; j++)
            {
                column[j] ??= new StringBuilder();
                if (i < input[j].Length)
                    column[j].Append(input[j][i]);
            }

        }

        if (column[0] != null)
            result.Add([.. column.Select(sb => sb?.ToString() ?? "")]);

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

