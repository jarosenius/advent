using System.Collections.Generic;
using System.Linq;
using System;

namespace Advent.y2024;

[AoC(2024)]
public class Day17() : Day(17, 2024)
{
    public override object Part1(List<string> input)
    {
        var a = int.Parse(input[0].Split(": ")[1]);
        var program = input.ElementAt(4).Split(": ")[1].SplitByAndParse(",", long.Parse).ToList();
        var output = Execute(program, a, 0, 0);

        Console.WriteLine(string.Join(",", output));
        return -1;
    }
    public override object Part2(List<string> input)
    {
        var program = input.ElementAt(4).Split(": ")[1].SplitByAndParse(",", long.Parse).ToList();
        return GetPossibleAValues(program, 0, 0, 0, 0).Min();;
    }

    private static List<long> Execute(List<long> program, long a, long b, long c)
    {
        var output = new List<long>();
        var ip = 0L;
        while (ip < program.Count)
        {
            var opcode = program[(int)ip];
            var literal = program[(int)ip + 1];
            var combo = LiteralToCombo(literal, a, b, c);
            switch (opcode)
            {
                case 0:
                    a /= (long)Math.Pow(2, combo);
                    break;
                case 1:
                    b ^= literal;
                    break;
                case 2:
                    b = combo % 8;
                    break;
                case 3:
                    if (a != 0)
                    {
                        ip = literal;
                        continue;
                    }
                    break;
                case 4:
                    b ^= c;
                    break;
                case 5:
                    output.Add(combo % 8);
                    break;
                case 6:
                    b = a / (long)Math.Pow(2, combo);
                    break;
                case 7:
                    c = a / (long)Math.Pow(2, combo);
                    break;
                default:
                    break;
            }

            ip += 2;
        }

        return output;
    }

    private static long LiteralToCombo(long operand, long a, long b, long c) => operand switch {
        0 or 1 or 2 or 3 => operand,
        4 => a,
        5 => b,
        6 => c,
        _ => throw new ArgumentException("Invalid operand. 0-6 only")
    };

    private static List<long> GetPossibleAValues(List<long> program, long a, long b, long c, int depth)
    {
        var possibleA = new List<long>();
        if (depth >= program.Count) 
            return possibleA;
        a*=8;
        for(var i = 0; i < 8; i++)
        {
            var output = Execute(program, a + i, b, c);
            if (program[^(depth+1)..].SequenceEqual(output))
            {
                if (depth + 1 == program.Count) 
                    possibleA.Add(a + i);
                possibleA.AddRange(GetPossibleAValues(program, a + i, b, c, depth + 1));
            }
        }
        return possibleA;
    } 

}
