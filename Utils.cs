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

        public static IEnumerable<T> GetNeighbors<T>(this IEnumerable<IEnumerable<T>> a, int xPos, int yPos, bool includeDiagonals)
        {
            var neighbors = new List<T>()
            {
                a.ElementAt(xPos, yPos-1),
                a.ElementAt(xPos+1, yPos),
                a.ElementAt(xPos, yPos+1),
                a.ElementAt(xPos-1, yPos),
            };
            if(includeDiagonals)
            {
                neighbors.Add(a.ElementAt(xPos-1, yPos-1));
                neighbors.Add(a.ElementAt(xPos+1, yPos-1));
                neighbors.Add(a.ElementAt(xPos+1, yPos+1));
                neighbors.Add(a.ElementAt(xPos-1, yPos+1));
            }
            

            return neighbors.Where(n => n != null);
        }

        public static T ElementAt<T>(this IEnumerable<IEnumerable<T>> a, int xPos, int yPos)
        {
            var minY = 0;
            var maxY = a.Count();
            var minX = 0;
            var maxX = a.ElementAt(0).Count();
            if(xPos < minX || xPos >= maxX || yPos < minY || yPos >= maxY)
                return default(T);
            return a.ElementAt(yPos).ElementAt(xPos);
        }

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, bool> condition )
        {
            var list = new List<T> { source.First() };
            foreach (T item in source.Skip(1))
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
    }
}
