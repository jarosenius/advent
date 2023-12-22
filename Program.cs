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
        }


        static async Task Main(string[] args)
        {
            Advent a = null;
            var config = new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets<Program>().Build();

            
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async options => 
            {
                if (options.SetupYear.HasValue)
                {
                    await SetupNewYear(options.SetupYear.Value);
                }
                else if (options.CreateDay)
                {
                    var dayTemplate = File.ReadAllText(Path.Combine(TemplatePath, "Day.txt"));
                    var basePath = SetupPaths(options.Year);

                    await SetupDayForYear(options.Year, options.Day > 0 ? options.Day : DateTime.Today.Day, dayTemplate, basePath, new CancellationToken());
                }
                else
                {
                    Console.WriteLine($"Options: UseExample = {options.UseExample}, Year = {options.Year}, Day = {options.Day}");
                    var client = new AocClient(config["AOC_TOKEN"]);
                    a = Advent.CreateForYear(options.Year, client);
                    if (a == null)
                    {
                        Console.WriteLine($"Could not create an AoC. Supported years are {string.Join(", ", Advent.SupportedYears.Select(s => s.Key))}");
                        await Task.CompletedTask;
                    }

                    var daysToRun = new[] { Latest };
                    if (options.Day == 0)
                        daysToRun = a.Days.Select(d => d.Key).ToArray();
                    else if (options.Day > 0 && options.Day <= 25)
                        daysToRun = [options.Day];

                    if(options.Year == DateTime.Now.Year && DateTime.Now.Month == 12 && DateTime.Now.Day < 25)
                        daysToRun = daysToRun.TakeWhile(d => d <= DateTime.Now.Day).ToArray();

                    
                    if (!daysToRun.All(d => a.HasDay(d)))
                    {
                        Console.WriteLine("The solution for the given year does not contain all the days that were requested.");
                        await Task.CompletedTask;
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

        private static async Task SetupNewYear(int year)
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

            var dayTemplate = await File.ReadAllTextAsync(Path.Combine(TemplatePath, "Day.txt"));
            await Parallel.ForEachAsync(Enumerable.Range(1, 25), async (d, c) => await SetupDayForYear(year, d, dayTemplate, basePath, c));

            var advent = await File.ReadAllTextAsync($"{TemplatePath}/Advent.txt", Encoding.Default);
            advent = advent.Replace("__YEAR__", $"{year}");
            await File.WriteAllTextAsync($"{basePath}/Advent{year}.cs", advent, Encoding.Default);

            Console.WriteLine($"Finished setting up year {year}.");
        }

        private static async Task SetupDayForYear(int year, int d, string dayTemplate, string basePath, CancellationToken c)
        {
            var inputPath = $"{basePath}/input";

            var day = d.ToString().PadLeft(2, '0');
            var content = dayTemplate
                .Replace("__YEAR__", $"{year}")
                .Replace("__DAY__", $"{d}")
                .Replace("__DAYPAD__", $"{day}");
            var name = new FileInfo(day).Name;
            var newPath = $"{basePath}/Day{name}.cs";

            await Task.WhenAll(
                File.WriteAllTextAsync(newPath, content, Encoding.Default, c),
                File.WriteAllTextAsync($"{inputPath}/{day}_example.txt", "", Encoding.Default, c));
        }
    }
}
