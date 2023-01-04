using System.Collections.Generic;
using System.Linq;

namespace Advent.y2022;

[AoC(2022)]
public class Day05 : Day
{
    public Day05() : base(5, 2022)
    {

    }

    public override object Part1(List<string> input)
    {
        var crates = input.TakeWhile(i => i != string.Empty);
        var stacks = crates.Last().Chunk(4).Select(c => new Stack<char>()).ToArray();
        var cratesWithLetter = crates.Reverse().Skip(1).SelectMany(c => stacks.Zip(c.Chunk(4)));
        cratesWithLetter.Where(c => c.Second[1] != ' ').ForEach(c => c.First.Push(c.Second[1]));

        var instructions = input.Skip(crates.Count() + 1).Select(i => ParseMove(i, stacks));
        instructions.ForEach(instr => Enumerable.Range(0, instr.CratesToMove).ForEach(_ => instr.To.Push(instr.From.Pop())));

        return string.Join("", stacks.Select(s => s.Pop()));
    }
    public override object Part2(List<string> input)
    {
        return 0;
    }

    private MoveInstruction ParseMove(string move, IEnumerable<Stack<char>> stacks)
    {
        var numbers = move.Replace("move ", "").Replace("from ", "").Replace("to ", "").Trim().Split(" ").Select(int.Parse).ToArray();
        var numberToMove = numbers[0];
        var from = numbers[1];
        var to = numbers[2];

        return new MoveInstruction(numberToMove, stacks.ElementAt(from - 1), stacks.ElementAt(to - 1));
    }

    private record MoveInstruction(int CratesToMove, Stack<char> From, Stack<char> To);
}

