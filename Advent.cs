using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Advent
{
    public abstract class Advent
    {
        public readonly int YEAR;
        public bool IsFirstRun = true;
        public Advent(int year)
        {
            YEAR = year;
        }
        public Dictionary<int, Day> Days = new Dictionary<int, Day>();

        public void AddDay(Day day)
        {
            if(day.DAY<=0 || day.DAY > 25 || YEAR != day.YEAR)
                return;

            Days[day.DAY] = day;
        }

        public async Task Solve(bool useExampleData, params int[] daysToSolve)
        {
            var tasks = new List<Task>();

            foreach(var d in daysToSolve)
            {
                if(Days.TryGetValue(d, out var day) == false)
                {
                    Console.WriteLine($"Could not find day {d}");
                    continue;
                }
                if(IsFirstRun)
                {
                    Console.WriteLine($"First run since startup. Running day {d} twice.");
                    await day.Solve(useExampleData);
                    IsFirstRun = false;
                }

                tasks.Add(day.Solve(useExampleData));
            }

            await Task.WhenAll(tasks);
        }

        public void PresentResults(params int[] daysToPresent)
        {
            foreach(var d in daysToPresent)
            {
                if(Days.TryGetValue(d, out var day) == false || day.HasSolution == false)
                    continue;

                day.PresentResult();
            }
        }

        public void PresentAll()
        {
            var days = Days.Where(d => d.Value.HasSolution).ToList();
            days.ForEach(d => d.Value.PresentResult());
        }
    }
}