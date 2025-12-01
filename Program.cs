using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Advent.Http;
using CommandLine;
using Microsoft.Extensions.Configuration;

namespace Advent
{

    public class Program
    {
        private static int Latest = DateTime.Now.Day > 25 ? 25 : DateTime.Now.Day;
        private static string TemplatePath = $"./Templates";

        public class Options
        {
            [Option('y', "year", Required = false, HelpText = "Set year to run. Defaults to last supported year.")]
            public int Year { get; set; } = Advent.SupportedYears.Max(s => s.Key);

            [Option('e', "useExample", Required = false, Default = false, HelpText = "Set to true to use example data.")]
            public bool UseExample { get; set; } = false;

            [Option('d', "day", Required = false, Default = 0, HelpText = "Day to run (1-25). 0 will run all days. -1 or invalid value will run the latest day.")]
            public int Day { get; set; } = 0;

            [Option('s', "setupYear", Required = false, HelpText = "Setup folders and files for a new year. Must be a number.")]
            public int? SetupYear { get; set; }

            [Option('c', "createDay", Required = false, Default = false, HelpText = "Set to true generate files for today.")]
            public bool CreateDay { get; set; } = false;

            [Option('x', "skipMissing", Required = false, Default = false, HelpText = "Skip creation of missing days.")]
            public bool SkipMissing { get; set; } = false;
        }


        static async Task Main(string[] args)
        {
            Advent a = null;
            var config = new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets<Program>().Build();

            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options =>
            {
                var client = new AocClient(config["AOC_TOKEN"]);

                if (options.SetupYear.HasValue)
                {
                    await SetupNewYear(options.SetupYear.Value, client);
                }
                else if (options.CreateDay)
                {
                    await SetupDayForYear(options.Year, options.Day > 0 ? options.Day : DateTime.Today.Day, client);
                }
                else
                {
                    Console.WriteLine($"Options: UseExample = {options.UseExample}, Year = {options.Year}, Day = {options.Day}");
                    a = await Advent.CreateForYear(options.Year, client);
                    if (a == null)
                    {
                        Console.WriteLine($"{options.Year} was not found among '{string.Join(", ", Advent.SupportedYears.Select(s => s.Key))}'.");
                        Console.WriteLine("Setting up classes. Please re-run");
                        await SetupNewYear(options.Year, client);
                        return;
                    }

                    var daysToRun = new[] { Latest };
                    if (options.Day == 0)
                        daysToRun = Enumerable.Range(1, 25).ToArray();
                    else if (options.Day is > 0 and <= 25)
                        daysToRun = [options.Day];

                    if (options.Year == DateTime.Now.Year && DateTime.Now.Month == 12 && DateTime.Now.Day < 25)
                        daysToRun = daysToRun.TakeWhile(d => d <= DateTime.Now.Day).ToArray();

                    if (options.Year > DateTime.Now.Year)
                    {
                        Console.WriteLine("Cannot run solutions from the future.");
                        daysToRun = [];
                    }

                    var daysNotCreated = daysToRun.Where(d => !a.HasDay(d));
                    if (daysNotCreated.Any() && !options.SkipMissing)
                    {
                        Console.WriteLine($"The solution for the given year does not contain solutions for the following days: [{string.Join(",", daysNotCreated)}].");
                        Console.WriteLine("Setting up classes. Please re-run");
                        daysNotCreated.ForEach(async d => await SetupDayForYear(options.Year, d, client));
                        return;
                    }
                    else if (daysToRun.Length == 0)
                    {
                        Console.WriteLine($"There are no solution for the given year that can be run.");
                        return;
                    }
                    await a.SolveAsync(options.UseExample, daysToRun);
                }
            });

            a?.PresentAll();
        }

        private static string SetupPaths(int year)
        {
            var basePath = $"./{year}";
            var inputPath = $"{basePath}/input";
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            if (!Directory.Exists(inputPath))
                Directory.CreateDirectory(inputPath);
            return basePath;
        }

        private static async Task SetupNewYear(int year, AocClient client)
        {
            Console.WriteLine($"Setting up new year {year}");
            Console.WriteLine(Directory.GetCurrentDirectory());
            if (Directory.Exists($"./{year}"))
            {
                Console.WriteLine($"Year {year} already exists. Exiting.");
                return;
            }

            var basePath = $"./{year}";
            var inputPath = $"{basePath}/input";

            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(inputPath);

            await Parallel.ForEachAsync(Enumerable.Range(1, 25), async (d, _) => await SetupDayForYear(year, d, client));

            var advent = await File.ReadAllTextAsync($"{TemplatePath}/Advent.txt", Encoding.Default);
            advent = advent.Replace("__YEAR__", $"{year}");
            await File.WriteAllTextAsync($"{basePath}/Advent{year}.cs", advent, Encoding.Default);

            Console.WriteLine($"Finished setting up year {year}.");
        }

        private static async Task SetupDayForYear(int year, int d, AocClient client)
        {
            var dayTemplate = await File.ReadAllTextAsync(Path.Combine(TemplatePath, "Day.txt"));
            var basePath = SetupPaths(year);

            var inputPath = $"{basePath}/input";

            var day = d.ToString().PadLeft(2, '0');
            var content = dayTemplate
                .Replace("__YEAR__", $"{year}")
                .Replace("__DAY__", $"{d}")
                .Replace("__DAYPAD__", $"{day}");
            var name = new FileInfo(day).Name;
            var newPath = $"{basePath}/Day{name}.cs";
            var examplePath = $"{inputPath}/{day}_example.txt";

            await Task.WhenAll(
                File.WriteAllTextAsync(newPath, content, Encoding.Default),
                Utils.FetchInputForDayAsync(year, d, client),
                Path.Exists(examplePath) ? Task.CompletedTask : File.WriteAllTextAsync(examplePath, string.Empty, Encoding.Default));
        }
    }
}
