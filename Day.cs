using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent
{
    public abstract class Day
    {
        public readonly int DAY = 0; 
        public readonly int YEAR = 0; 
        
        public string[] Input { get; private set; }
        public string[] ExampleInput { get; private set; }

        public TimeSpan TimeToExecutePart1 { get; private set; }
        public TimeSpan TimeToExecutePart2 { get; private set; }
        public long ResultPart1 { get; private set; }
        public long ResultPart2 { get; private set; }
        public bool UsedExampleData { get; private set; }
        public bool HasSolution { get; private set; }

        public Day(int day, int year)
        {
            DAY = day;
            YEAR = year;
        }
        public async Task<(long Part1Result, double Part1Ms, long Part2Result, double Part2Ms)> Solve(bool example)
        {
            UsedExampleData = example;
            ExampleInput = await File.ReadAllLinesAsync(Helper.GetExampleInputForDay(DAY, YEAR));
            Input = await File.ReadAllLinesAsync(Helper.GetInputForDay(DAY, YEAR));

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            ResultPart1 = Part1(example ? ExampleInput : Input);
            sw.Stop();
            TimeToExecutePart1 = sw.Elapsed;
            sw.Restart();
            ResultPart2 = Part2(example ? ExampleInput : Input);
            sw.Stop();
            TimeToExecutePart2 = sw.Elapsed;
            HasSolution = true;
            return (ResultPart1, TimeToExecutePart1.TotalMilliseconds, ResultPart2, TimeToExecutePart2.TotalMilliseconds);
        }

        public void PresentResult()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"--- Running Day {DAY} ({YEAR}) using {(UsedExampleData ? "example" : "regular")} data");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Part 1: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{ResultPart1}".PadRight(15));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{TimeToExecutePart1.TotalMilliseconds}ms");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Part 2: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{ResultPart2}".PadRight(15));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{TimeToExecutePart2.TotalMilliseconds}ms");
            Console.ForegroundColor = color;
        }

        public abstract long Part1(IEnumerable<string> input);
        public abstract long Part2(IEnumerable<string> input);
    }
}
