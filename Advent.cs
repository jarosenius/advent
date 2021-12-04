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
            Days.TryGetValue(daysToSolve.First(), out var day);
            await day?.Solve(true); // First run is a bit slower so run it once before the real run.

            await Parallel.ForEachAsync(daysToSolve, async (d, c) => 
            {
                if(Days.TryGetValue(d, out var day) == false)
                {
                    Console.WriteLine($"Could not find day {d}");
                    await Task.CompletedTask;
                }

                await day.Solve(useExampleData);
            });
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