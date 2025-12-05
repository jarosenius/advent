using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Advent.Http;

namespace Advent
{
    public static class Utils
    {
        public static string GetInputForDay(int day, int year)
        {
            var d = day.ToString().PadLeft(2, '0');
            return $"./{year}/input/{d}.txt";
        }

        public static string GetExampleInputForDay(int day, int year)
        {
            var d = day.ToString().PadLeft(2, '0');
            return $"./{year}/input/{d}_example.txt";
        }

        public static long ConvertFromBinaryString(string bin)
        {
            return Convert.ToInt64(bin, 2);
        }

        public static string InvertBinaryString(string bin)
        {
            return bin.Aggregate("", (r, c) => c == '1' ? r += "0" : r += "1");
        }

        public static IEnumerable<T> SliceColumn<T>(this T[][] matrix, int column)
        {
            for (var x = 0; x < matrix.Length; x++)
            {
                yield return matrix[x][column];
            }
        }

        public static IEnumerable<T> SliceColumn<T>(this IEnumerable<IEnumerable<T>> list, int column)
        {
            for (var x = 0; x < list.Count(); x++)
            {
                yield return list.ElementAt(x).ElementAt(column);
            }
        }

        public static IEnumerable<int> ReadLinesAsInt(this IEnumerable<string> lines)
        {
            return lines.Select(int.Parse);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

        public static int[][] SplitByFirstThenBySecondAndParseToInt(
            this string line,
            string splitFirst,
            string splitSecond
        )
        {
            return [.. line.Split(splitFirst, StringSplitOptions.RemoveEmptyEntries).Select(p => p.SplitByAndParseToInt(splitSecond))];
        }

        public static long[][] SplitByFirstThenBySecondAndParseToLong(
            this string line,
            string splitFirst,
            string splitSecond
        )
        {
            return [.. line.Split(splitFirst, StringSplitOptions.RemoveEmptyEntries).Select(p => p.SplitByAndParse(splitSecond, long.Parse))];
        }

        public static int[] SplitByAndParseToInt(this string line, string split)
        {
            return line.SplitByAndParse(split, int.Parse);
        }
        public static long[] SplitByAndParseToLong(this string line, string split)
        {
            return line.SplitByAndParse(split, long.Parse);
        }
        public static T[] SplitByAndParse<T>(this string line, string split, Func<string, T> parse)
        {
            return [.. line.Split(split, StringSplitOptions.RemoveEmptyEntries).Select(parse)];
        }

        public static T[][] CreateMapWithSize<T>(int width, int length)
        {
            var grid = new T[length][];
            for (var i = 0; i < grid.Length; i++)
            {
                grid[i] = new T[width];
            }
            return grid;
        }

        public static IEnumerable<T> GetNeighbors<T>(
            this IEnumerable<IEnumerable<T>> a,
            int xPos,
            int yPos,
            bool includeDiagonals
        )
        {
            var neighbors = new List<T>()
            {
                a.ElementAt(xPos, yPos - 1),
                a.ElementAt(xPos + 1, yPos),
                a.ElementAt(xPos, yPos + 1),
                a.ElementAt(xPos - 1, yPos),
            };
            if (includeDiagonals)
            {
                neighbors.Add(a.ElementAt(xPos - 1, yPos - 1));
                neighbors.Add(a.ElementAt(xPos + 1, yPos - 1));
                neighbors.Add(a.ElementAt(xPos + 1, yPos + 1));
                neighbors.Add(a.ElementAt(xPos - 1, yPos + 1));
            }

            return neighbors.Where(n => n != null);
        }

        public static T ElementAt<T>(this IEnumerable<IEnumerable<T>> a, int xPos, int yPos)
        {
            var minY = 0;
            var maxY = a.Count();
            var minX = 0;
            var maxX = a.ElementAt(0).Count();
            if (xPos < minX || xPos >= maxX || yPos < minY || yPos >= maxY)
                return default(T);
            return a.ElementAt(yPos).ElementAt(xPos);
        }

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(
            this IEnumerable<T> source,
            Func<T, bool> condition
        )
        {
            var toKeep = source.SkipWhile(i => !condition(i));
            var list = new List<T> { toKeep.First() };
            foreach (var item in toKeep.Skip(1))
            {
                if (condition(item) == false)
                {
                    yield return list;
                    list = new List<T>();
                    continue;
                }

                list.Add(item);
            }
            yield return list;
        }

        public static MatchedContent[] Matches(
            this IEnumerable<string> source,
            string pattern,
            RegexOptions regexOptions = RegexOptions.None
        ) => source.Matches(new Regex(pattern, regexOptions));

        public static MatchedContent[] Matches(this IEnumerable<string> source, Regex regex) =>
            (
                from y in Enumerable.Range(0, source.Count())
                from m in regex.Matches(source.ElementAt(y))
                select new MatchedContent(m.Value, m.Index, y)
            ).ToArray();

        public record MatchedContent(string Content, int XPos, int YPos)
        {
            public bool IsNextTo(MatchedContent n) =>
                Math.Abs(n.YPos - YPos) <= 1
                && XPos <= n.XPos + n.Content.Length
                && n.XPos <= XPos + Content.Length;
        }

        public static long GreatestCommonFactor(this long a, long b) => b == 0 ? a : b.GreatestCommonFactor(a % b);
        public static long LeastCommonMultiple(this long a, long b) => a * b / a.GreatestCommonFactor(b);
        public static long[] DiffEveryOther(this IEnumerable<long> list) =>
            [.. list.Zip(list.Skip(1)).Select(p => p.Second - p.First)];


        public static async Task FetchExampleInputForDayAsync(int year, int day, AocClient client)
            => await FetchInputForDayAsync(year, day, GetExampleInputForDay, client.FetchExampleInputAsync, false);

        public static async Task FetchInputForDayAsync(int year, int day, AocClient client)
         => await FetchInputForDayAsync(year, day, GetInputForDay, client.FetchInputAsync);

        private static async Task FetchInputForDayAsync(int year, int day, Func<int, int, string> getInputPath, Func<int, int, Task<string>> fetchInput, bool checkForExisting = true)
        {
            var path = getInputPath(day, year);
            var input = await fetchInput(year, day);
            input = input.TrimEnd();
            if (checkForExisting == false || !string.IsNullOrWhiteSpace(input))
            {
                await File.WriteAllTextAsync(path, input, Encoding.Default, CancellationToken.None);
                Console.WriteLine($"Saved input for day {day} to {path}");
            }
        }

        public static int Mod(this int value, int modulus)
        {
            var result = value % modulus;
            return result < 0 ? result + modulus : result;
        }

        public static long Mod(this long value, long modulus)
        {
            var result = value % modulus;
            return result < 0 ? result + modulus : result;
        }


    }
}
