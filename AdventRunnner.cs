using System;
using System.Linq;
using System.Threading.Tasks;
using Advent.Factory;
using Advent.Http;

namespace Advent;

public class AdventRunner(AocClient client, IAdventFactory adventFactory)
{
    private static int Latest => DateTime.Now.Day > 25 ? 25 : DateTime.Now.Day;

    public async Task RunAsync(Program.Options options)
    {
        if (options.SetupYear.HasValue)
        {
            await AdventSetupHelpers.SetupNewYearAsync(options.SetupYear.Value, client);
        }
        else if (options.CreateDay)
        {
            await AdventSetupHelpers.SetupDayForYearAsync(
                options.Year,
                options.Day > 0 ? options.Day : DateTime.Today.Day,
                client);
        }
        else
        {
            Console.WriteLine($"Options: UseExample = {options.UseExample}, Year = {options.Year}, Day = {options.Day}");
            var advent = await adventFactory.CreateForYearAsync(options.Year);

            if (advent == null)
            {
                Console.WriteLine($"{options.Year} was not found among '{string.Join(", ", Advent.SupportedYears.Select(s => s.Key))}'.");
                Console.WriteLine("Setting up classes. Please re-run");
                await AdventSetupHelpers.SetupNewYearAsync(options.Year, client);
                return;
            }

            var daysToRun = GetDaysToRun(options, advent);

            if (daysToRun.Length == 0)
            {
                Console.WriteLine("There are no solutions for the given year that can be run.");
                return;
            }

            await advent.SolveAsync(options.UseExample, daysToRun);
            advent.PresentAll();
        }
    }

    private static int[] GetDaysToRun(Program.Options options, Advent advent)
    {
        var daysToRun = options.Day switch
        {
            0 => [.. Enumerable.Range(1, 25)],
            > 0 and <= 25 => [options.Day],
            _ => new[] { Latest }
        };

        if (options.Year == DateTime.Now.Year && DateTime.Now.Month == 12 && DateTime.Now.Day < 25)
            daysToRun = [.. daysToRun.TakeWhile(d => d <= DateTime.Now.Day)];

        if (options.Year > DateTime.Now.Year)
        {
            Console.WriteLine("Cannot run solutions from the future.");
            return [];
        }

        var daysNotCreated = daysToRun.Where(d => !advent.HasDay(d)).ToList();
        if (daysNotCreated.Count == 0 || options.SkipMissing)
            return daysToRun;

        Console.WriteLine($"The solution for the given year does not contain solutions for the following days: [{string.Join(",", daysNotCreated)}].");
        Console.WriteLine("Setting up classes. Please re-run");
        return [];

    }
}