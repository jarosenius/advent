using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Advent
{
    public abstract class Day(int day, int year)
    {
        public readonly int DAY = day;
        public readonly int YEAR = year;

        public string[] Input { get; private set; }
        public string[] ExampleInput { get; private set; }

        public TimeSpan TimeToExecutePart1 { get; private set; }
        public TimeSpan TimeToExecutePart2 { get; private set; }
        public object ResultPart1 { get; private set; }
        public object ResultPart2 { get; private set; }
        public bool UsedExampleData { get; private set; }
        public bool HasSolution { get; private set; }

        public async Task<(object Part1Result, double Part1Ms, object Part2Result, double Part2Ms)> Solve(bool example)
        {
            UsedExampleData = example;
            ExampleInput = await File.ReadAllLinesAsync(Utils.GetExampleInputForDay(DAY, YEAR));
            Input = await File.ReadAllLinesAsync(Utils.GetInputForDay(DAY, YEAR));

            if (ExampleInput.Length == 0 || ExampleInput.Length == 1 && string.IsNullOrWhiteSpace(ExampleInput[0]))
                return (0, 0, 0, 0);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            ResultPart1 = Part1(example ? [.. ExampleInput] : [.. Input]);
            sw.Stop();
            TimeToExecutePart1 = sw.Elapsed;
            sw.Restart();
            ResultPart2 = Part2(example ? [.. ExampleInput] : [.. Input]);
            sw.Stop();
            TimeToExecutePart2 = sw.Elapsed;
            HasSolution = ResultPart1 != null && long.TryParse(ResultPart1.ToString(), out var res1) && res1 != 0 || ResultPart2 != null && long.TryParse(ResultPart2.ToString(), out var res2) && res2 != 0;
            return (ResultPart1, TimeToExecutePart1.TotalMilliseconds, ResultPart2, TimeToExecutePart2.TotalMilliseconds);
        }

        public void PresentResult()
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Day {DAY:D2} ({YEAR})".PadRight(48));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{(UsedExampleData ? "[EXAMPLE]" : "[LIVE]"),13}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ║");
            Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");

            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Part 1: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{ResultPart1,-28}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" ".PadRight(12));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{FormatTime(TimeToExecutePart1),13}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ║");

            Console.Write("║ ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Part 2: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{ResultPart2,-28}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" ".PadRight(12));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{FormatTime(TimeToExecutePart2),13}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" ║");

            Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = originalColor;
        }

        private static string FormatTime(TimeSpan time)
        {
            if (time.TotalSeconds >= 1)
                return $"{time.TotalSeconds:F2}s";
            if (time.TotalMilliseconds >= 1)
                return $"{time.TotalMilliseconds:F2}ms";
            return $"{time.TotalMicroseconds:F2}μs";
        }

        public abstract object Part1(List<string> input);
        public abstract object Part2(List<string> input);
    }
}
