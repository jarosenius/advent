using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            return bin.Aggregate("", (r, c) => c == '1' ? r+="0" : r+="1");
        }

        public static IEnumerable<T> SliceColumn<T>(this T[][] matrix, int column)
        {
            for (var x = 0; x < matrix.Length; x++)
            {
                yield return matrix[x][column];
            }
        }
        public static IEnumerable<int> ReadLinesAsInt(this IEnumerable<string> lines)
        {
            return lines.Select(n => int.Parse(n));
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration)
            {
                action(item);
            }
        }

        public static int[][] SplitByFirstThenBySecondAndParseToInt(this string line, string splitFirst, string splitSecond)
        {
            return line.Split(splitFirst, StringSplitOptions.RemoveEmptyEntries).Select(p => p.SplitByAndParseToInt(splitSecond)).ToArray();
        }

        public static int[] SplitByAndParseToInt(this string line, string split)
        {
            return line.Split(split, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public static T[][] CreateMapWithSize<T>(int width, int length)
        {
            var grid = new T[length][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new T[width];
            }
            return grid;
        }
    }
}
