using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day19 : Day
{
    public Day19() : base(19, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var workflows = input.TakeWhile(i => i != string.Empty).Select(Workflow.Create).ToDictionary(w => w.Name, w => w);
        var parts = input.Skip(workflows.Count+1).Select(Part.Create).ToList();
        var validParts = parts.Where(p => Execute(p, workflows, "in")).Sum(p => p.Sum);
        return validParts;
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }
    
    private static bool Execute(Part part, Dictionary<string, Workflow> workflows, string workflow)
    {
        if(workflow == "R")
            return false;
        else if(workflow == "A")
            return true;
        else
            return Execute(part, workflows, workflows[workflow].Execute(part));
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
            var nameEnd = input.IndexOf('{');
            var rulesRaw = input[(nameEnd+1)..^1].Split(',');
            return new Workflow(input[0..nameEnd], rulesRaw[0..^1].Select(Rule.Create).ToList(), rulesRaw[^1]);
        }
    }

    private class Rule(string appliesTo, Func<int, bool> evaluation, string target)
    {
        private readonly Func<int, bool> evaluation = evaluation;
        public string AppliesTo { get; } = appliesTo;
        public string Target { get; } = target;

        public bool Apply(int i) => evaluation(i);
        public static Rule Create(string input)
        {
            var targetStart = input.IndexOf(':')+1;
            var limit = int.Parse(input[2..(targetStart-1)]);
            return new Rule(input[0..1], input[1..2] == "<" ? i => i < limit : i => i > limit, input[targetStart..]);
        }
    }
}
