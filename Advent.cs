using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advent.y2020;
using Advent.y2021;

namespace Advent
{
    public abstract class Advent
    {
        private static Dictionary<int, Type> supportedYears;
        public static Dictionary<int, Type> SupportedYears
        {
            get { return supportedYears ?? (supportedYears = GetSupportedYears()); }
        }

        public static Dictionary<int, Type> GetSupportedYears()
        {
            var adventsWithYear =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(AoCAttribute), false)
                where
                    t != null
                    && t.IsSubclassOf(typeof(Advent))
                    && t.IsAbstract == false
                    && attributes.OfType<AoCAttribute>().Any()
                select new KeyValuePair<int, Type>(
                    attributes.Cast<AoCAttribute>().FirstOrDefault().Year,
                    t
                );

            return adventsWithYear.ToDictionary(x => x.Key, x => x.Value);
        }

        public static IEnumerable<Type> GetDaysForYear(int year)
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t != null && t.IsSubclassOf(typeof(Day)) && t.IsAbstract == false
                   let attributes = t.GetCustomAttributes(typeof(AoCAttribute), false)
                   where attributes.OfType<AoCAttribute>().Any(y => y.Year == year)
                   select t;
        }

        public static Advent CreateForYear(int year)
        {
            if (SupportedYears.TryGetValue(year, out var advent))
            {
                return (Advent)Activator.CreateInstance(advent);
            }
            return null;
        }

        public static Day CreateDay(Type t)
        {
            return (Day)Activator.CreateInstance(t);
        }

        public readonly int YEAR;
        public bool IsFirstRun = true;

        public Advent(int year)
        {
            YEAR = year;

            Advent.GetDaysForYear(YEAR)
            .Select(Advent.CreateDay)
            .ToList()
            .ForEach(d => AddDay(d));
        }

        public Dictionary<int, Day> Days = new Dictionary<int, Day>();

        public void AddDay(Day day)
        {
            if (day.DAY <= 0 || day.DAY > 25 || YEAR != day.YEAR)
                return;

            Days[day.DAY] = day;
        }

        public bool HasDay(int day) => Days.ContainsKey(day);

        public async Task SolveAsync(bool useExampleData, params int[] daysToSolve)
        {
            Days.TryGetValue(daysToSolve.First(), out var day);
            await day?.Solve(true); // First run is a bit slower so run it once before the real run.

            await Parallel.ForEachAsync(
                daysToSolve,
                async (d, c) =>
                {
                    if (Days.TryGetValue(d, out var day) == false)
                    {
                        Console.WriteLine($"Could not find day {d}");
                        await Task.CompletedTask;
                    }

                    await day.Solve(useExampleData);
                }
            );
        }

        public void PresentResults(params int[] daysToPresent)
        {
            foreach (var d in daysToPresent)
            {
                if (Days.TryGetValue(d, out var day) == false || day.HasSolution == false)
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
