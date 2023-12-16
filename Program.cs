﻿using System;
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

            [Option('f', "fetchInput", Required = false, Default = false, HelpText = "Fetch the input for the specified day. The environment variable 'AOC_TOKEN' have to be set.")]
            public bool FetchInput { get; set; } = false;
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
                    a = Advent.CreateForYear(options.Year);
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

                    if(options.FetchInput)
                    {
                        if(a.YEAR > DateTime.Now.Year || a.YEAR < 2015)
                            throw new ArgumentException("Cannot fetch input for games that do not exist");
                        var daysToFetch = options.Day is >=1 and <=25 ? [options.Day] : a.Days.Select(d => d.Key);
                        if(a.YEAR == DateTime.Now.Year)
                            daysToFetch = daysToFetch.TakeWhile(d => d <= DateTime.Now.Day);

                        var client = new AocClient(config["AOC_TOKEN"]);
                        var basePath = SetupPaths(options.Year);
                        foreach (var day in daysToFetch)
                        {
                            await FetchInputForDayAsync(a.YEAR, day, basePath, client);
                        }

                    }

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

        private static async Task FetchInputForDayAsync(int year, int day, string basePath, AocClient client)
        {
            var d = day.ToString().PadLeft(2, '0');
            var inputPath = $"{basePath}/input/{d}.txt";
            var input = await client.FetchInputAsync(year, day);
            input = input.TrimEnd();
            await File.WriteAllTextAsync(inputPath, input, Encoding.Default, CancellationToken.None);
            Console.WriteLine($"Saved input for day {d} to {inputPath}");
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

            var advent = await File.ReadAllTextAsync($"{TemplatePath}/Advent202x.txt", Encoding.Default);
            advent = advent.Replace("202x", year.ToString()).Replace(" base(2020)", $" base({year})").Replace("[AoC(2020)]", $"[AoC({year})]");
            await File.WriteAllTextAsync($"{basePath}/Advent{year}.cs", advent, Encoding.Default);

            Console.WriteLine($"Finished setting up year {year}.");
        }

        private static async Task SetupDayForYear(int year, int d, string dayTemplate, string basePath, CancellationToken c)
        {
            var inputPath = $"{basePath}/input";

            var day = d.ToString().PadLeft(2, '0');
            var content = dayTemplate
                .Replace("Advent.y202x", $"Advent.y{year}")
                .Replace("1, 2021)", $"{d}, {year})")
                .Replace("(2021)", $"({year})")
                .Replace("DayXY", $"Day{day}");
            var name = new FileInfo(day).Name;
            var newPath = $"{basePath}/Day{name}.cs";

            await Task.WhenAll(
                File.WriteAllTextAsync(newPath, content, Encoding.Default, c),
                File.WriteAllTextAsync($"{inputPath}/{day}.txt", "", Encoding.Default, c),
                File.WriteAllTextAsync($"{inputPath}/{day}_example.txt", "", Encoding.Default, c));
        }
    }
}
