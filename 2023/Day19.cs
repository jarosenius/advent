using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day19 : Day
{
    private const string Accepted = "A";
    private const string Rejected = "R";
    private const string WorkflowStart = "in";
    public Day19() : base(19, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var workflows = input.TakeWhile(i => i != string.Empty).Select(Workflow.Create).ToDictionary(w => w.Name, w => w);
        var parts = input.Skip(workflows.Count+1).Select(Part.Create).ToList();
        var validParts = parts.Where(p => EvaluatePart(p, workflows, WorkflowStart)).Sum(p => p.Sum);
        return validParts;
    }
    public override object Part2(List<string> input)
    {
        var workflows = input.TakeWhile(i => i != string.Empty).Select(Workflow.Create).ToDictionary(w => w.Name, w => w);
        var ranges = ImmutableList.Create(
            new Range(1, 4000),
            new Range(1, 4000),
            new Range(1, 4000),
            new Range(1, 4000)
        );
        return GetValidCombinations(workflows, ranges, WorkflowStart);
    }   

    private static bool EvaluatePart(Part part, Dictionary<string, Workflow> workflows, string workflow)
    {
        if(workflow == Rejected)
            return false;
        else if(workflow == Accepted)
            return true;
        else
            return EvaluatePart(part, workflows, workflows[workflow].Execute(part));
    }


    private static ulong GetValidCombinations(Dictionary<string, Workflow> workflows, ImmutableList<Range> ranges, string workflow)
    {
        if(workflow == Accepted)
            return ranges.Aggregate((ulong)1, (total, next) => total * (ulong)next.ValidValues);
        if(workflow == Rejected)
            return 0;
        if(ranges.Any(r => r.Invalid))
            return 0;

        ulong result = 0;
        var wf = workflows[workflow];

        var tempRanges = ranges;
        foreach (var rule in wf.Rules)
        {
            var rangeIndex = rule.AppliesTo switch 
            {
                "a" => 0,
                "x" => 1,
                "m" => 2,
                "s" => 3,
                _ => throw new ArgumentException($"'s' have to be one of 'a', 'x', 'm' or 's'. Was: {rule.AppliesTo}")
            };
            var splitValue = rule.IsLessThanOperator ? rule.Limit : rule.Limit+1;
            var (lower, higher) = tempRanges[rangeIndex].Split(splitValue);
            var newRange = rule.IsLessThanOperator ? lower : higher;
            result += GetValidCombinations(workflows, tempRanges.SetItem(rangeIndex, newRange), rule.Target);
            tempRanges = tempRanges.SetItem(rangeIndex, rule.IsLessThanOperator ? higher : lower);
        }

        return result + GetValidCombinations(workflows, tempRanges, wf.Default); 
    }

    private record Part(Dictionary<string, int> Values)
    {
        public long Sum => Values.Sum(v => v.Value);
        public static Part Create(string input)
        {
            var values = input[1..^1].Split(',').Select(s => s.Split('=')).Select(kv => new KeyValuePair<string, int>(kv[0], int.Parse(kv[1]))).ToDictionary();
            return new Part(values);
        }
    }

    private record Workflow(string Name, List<Rule> Rules, string Default)
    {
        public string Execute(Part part)
        {
            foreach (var rule in Rules)
            {
                if(rule.Apply(part.Values[rule.AppliesTo]))
                    return rule.Target;
            }

            return Default;
        }

        public static Workflow Create(string input)
        {
            var parts = input.Split('{', '}');
            var rulesRaw = parts[1].Split(',');
            return new Workflow(parts[0], rulesRaw.SkipLast(1).Select(Rule.Create).ToList(), rulesRaw[^1]);
        }
    }

    private class Rule(string appliesTo, string target, int limit, bool lt)
    {
        private readonly int limit = limit;
        private readonly bool lt = lt;
        public string AppliesTo { get; } = appliesTo;
        public string Target { get; } = target;
        public int Limit { get; } = limit;
        public bool IsLessThanOperator { get; } = lt;
        public bool Apply(int i) => lt ? i < limit : i > limit;
        public static Rule Create(string input)
        {
            var targetStart = input.IndexOf(':')+1;
            var limit = int.Parse(input[2..(targetStart-1)]);
            return new Rule(input[0..1], input[targetStart..], limit, input[1..2] == "<");
        }
    }

    private record Range(int From, int To)
    {
        public int ValidValues => To - From + 1;
        public bool Invalid => To < From;
        public (Range Lower, Range Higher) Split(int split)
        {
            return (new(From, Math.Min(split-1, To)), new(Math.Max(From, split), To));
        }
    };
}
