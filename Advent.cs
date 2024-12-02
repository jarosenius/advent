using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Advent.Http;

namespace Advent
{
    public abstract class Advent(int year, AocClient client)
    {
        private static Dictionary<int, Type> supportedYears;
        public static Dictionary<int, Type> SupportedYears
        {
            get { return supportedYears ??= GetSupportedYears(); }
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

        public static async Task<Advent> CreateForYear(int year, AocClient client)
        {
            if (SupportedYears.TryGetValue(year, out var advent))
            {
                var instance = (Advent)Activator.CreateInstance(advent, client);
                await instance.SetupDays();
                return instance; 
            }
            return null;
        }

        public static Day CreateDay(Type t)
        {
            return (Day)Activator.CreateInstance(t);
        }

        public readonly int YEAR = year;
        public bool IsFirstRun = true;

        public async Task SetupDays()
        {
            var days = GetDaysForYear(YEAR)
            .Select(CreateDay)
            .Select(d => AddDay(d, client)).ToList();
            await Task.WhenAll(days);
        }

        public Dictionary<int, Day> Days = [];

        public async Task AddDay(Day day, AocClient aocClient)
        {
            if (day.DAY <= 0 || day.DAY > 25 || YEAR != day.YEAR)
                return;
            Days[day.DAY] = day;
            
            if(YEAR < DateTime.Now.Year || (YEAR == DateTime.Now.Year && day.DAY <= DateTime.Now.Day))
            {
                var inputPath = Utils.GetInputForDay(day.DAY, YEAR);
                if(!File.Exists(inputPath))
                {
                    await Utils.FetchInputForDayAsync(YEAR, day.DAY, aocClient);
                }
            }
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
