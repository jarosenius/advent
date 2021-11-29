using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
