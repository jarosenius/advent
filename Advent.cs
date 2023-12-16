using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Advent.Http;

namespace Advent
{
    public abstract class Advent
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

        public static Advent CreateForYear(int year, AocClient client)
        {
            if (SupportedYears.TryGetValue(year, out var advent))
            {
                return (Advent)Activator.CreateInstance(advent, client);
            }
            return null;
        }

        public static Day CreateDay(Type t)
        {
            return (Day)Activator.CreateInstance(t);
        }

        public readonly int YEAR;
        public bool IsFirstRun = true;


        public Advent(int year, AocClient client)
        {
            YEAR = year;
            GetDaysForYear(YEAR)
            .Select(CreateDay)
            .ToList()
            .ForEach(d => AddDay(d, client));
        }

        public Dictionary<int, Day> Days = [];

        public void AddDay(Day day, AocClient aocClient)
        {
            if (day.DAY <= 0 || day.DAY > 25 || YEAR != day.YEAR)
                return;
            Days[day.DAY] = day;
            
            if(YEAR <= DateTime.Now.Year && day.DAY <= DateTime.Now.Day)
            {
                var inputPath = Utils.GetInputForDay(day.DAY, YEAR);
                if(!File.Exists(inputPath))
                {
                    Utils.FetchInputForDayAsync(YEAR, day.DAY, aocClient).Wait();
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
