using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day15 : Day
{
    public Day15() : base(15, 2023)
    {

    }

    public override object Part1(List<string> input) => input[0].Split(',').Aggregate(0L, (sum, str) => sum += Hash(str));

    public override object Part2(List<string> input) => input[0].Split(',')
        .Aggregate(new Dictionary<int, OrderedDictionary>(), (m, str) => PerformInstruction(str, m))
        .Sum(d => CalculateBoxValue(d.Key+1, d.Value));

    private static long CalculateBoxValue(int box, OrderedDictionary map) => map.Values.Cast<int>().Select((v, i) => box * (i+1) * v).Sum();

    private static Dictionary<int, OrderedDictionary> PerformInstruction(string str, Dictionary<int, OrderedDictionary> map)
    {
        var (label, operation, lensValue) = str[^1] == '-' ? (str[..^1], str[^1], 0) : (str[..^2], str[^2], str[^1] - '0');
        var key = Hash(label);
        switch(operation)
        {
            case '=':
                if(!map.ContainsKey(key))
                    map[key] = [];
                map[key][label] = lensValue;
                break;
            case '-':
                if(!map.TryGetValue(key, out var value))
                    break;
                value.Remove(label);
                break;
            default:
                throw new ArgumentException("Invalid operation");
        }
        return map;
    }

    private static int Hash(string str) => str.Aggregate(0, (value, c) => {
            value += c;
            value *= 17;
            value %= 256;
            return value;
    });    
}
