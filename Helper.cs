using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advent
{
    public static class Helper
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
    }
}
