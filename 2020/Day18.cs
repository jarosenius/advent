using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.y2020
{
    [AoC(2020)]
    public class Day18 : Day
    {
        public Day18() : base(18, 2020)
        {

        }

        public override object Part1(List<string> input)
        {

            return input.Where(i => string.IsNullOrEmpty(i) == false).Select(i => long.Parse(Parse(i))).Aggregate(0L, (total, next) => total += next);
        }
        public override object Part2(List<string> input)
        {

            return input.Where(i => string.IsNullOrEmpty(i) == false).Select(i => long.Parse(Parse2(i))).Aggregate(0L, (total, next) => total += next);
        }

        static string Parse(string expression)
        {
            const string reg = @"(\d+) *([\+\*]) *(\d+)";
            Regex regex = new Regex(reg, RegexOptions.Compiled);
            while (true)
            {
                if (expression.Contains("("))
                {
                    int leftSide = expression.IndexOf('(');
                    int rightSide = -1;
                    int level = 0;
                    for (int i = leftSide; i < expression.Length; i++)
                    {
                        if (expression[i] == '(')
                        {
                            level++;
                        }
                        else if (expression[i] == ')')
                        {
                            level--;
                        }
                        if (level == 0)
                        {
                            rightSide = i;
                            break;
                        }
                    }
                    if (rightSide != -1 && level == 0)
                    {
                        var rep = Parse(expression.Substring(leftSide + 1, rightSide - leftSide - 1));
                        expression = expression.Substring(0, leftSide) + rep + expression.Substring(rightSide + 1);
                    }
                }
                else
                {

                    var match = regex.Match(expression);
                    if (match.Success)
                    {
                        long res = 0;
                        switch (match.Groups[2].Value)
                        {
                            case "+":
                                res = long.Parse(match.Groups[1].Value) + long.Parse(match.Groups[3].Value);
                                break;
                            case "*":
                                res = long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[3].Value);
                                break;
                            default:
                                break;
                        }
                        expression = regex.Replace(expression, res.ToString(), 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return expression;
        }

        static string Parse2(string expression)
        {
            string r1 = @"(\d+) *([\+]) *(\d+)";
            string r2 = @"(\d+) *([\*]) *(\d+)";
            var addRegex = new Regex(r1, RegexOptions.Compiled);
            var multRegex = new Regex(r2, RegexOptions.Compiled);
            while (true)
            {
                if (expression.Contains("("))
                {
                    int leftSide = expression.IndexOf('(');
                    int rightSide = -1;
                    int level = 0;
                    for (int i = leftSide; i < expression.Length; i++)
                    {
                        if (expression[i] == '(')
                        {
                            level++;
                        }
                        else if (expression[i] == ')')
                        {
                            level--;
                        }
                        if (level == 0)
                        {
                            rightSide = i;
                            break;
                        }
                    }
                    if (rightSide != -1 && level == 0)
                    {
                        var rep = Parse2(expression.Substring(leftSide + 1, rightSide - leftSide - 1));
                        expression = expression.Substring(0, leftSide) + rep + expression.Substring(rightSide + 1);
                    }
                }
                else
                {

                    var match = addRegex.Match(expression);
                    var match2 = multRegex.Match(expression);
                    if (match.Success)
                    {
                        long res = 0;
                        switch (match.Groups[2].Value)
                        {
                            case "+":
                                res = long.Parse(match.Groups[1].Value) + long.Parse(match.Groups[3].Value);
                                break;
                            default:
                                break;
                        }
                        expression = addRegex.Replace(expression, res.ToString(), 1);
                    }
                    else if (match2.Success)
                    {
                        long res = 0;
                        switch (match2.Groups[2].Value)
                        {
                            case "*":
                                res = long.Parse(match2.Groups[1].Value) * long.Parse(match2.Groups[3].Value);
                                break;
                            default:
                                break;
                        }
                        expression = multRegex.Replace(expression, res.ToString(), 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return expression;
        }
    }
}
