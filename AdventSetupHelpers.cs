using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent.Http;

namespace Advent;

public static class AdventSetupHelpers
{
    private static string TemplatePath => "./Templates";

    public static async Task SetupNewYearAsync(int year, AocClient client)
    {
        Console.WriteLine($"Setting up new year {year}");

        if (Directory.Exists($"./{year}"))
        {
            Console.WriteLine($"Year {year} already exists. Exiting.");
            return;
        }

        var basePath = $"./{year}";
        var inputPath = $"{basePath}/input";

        Directory.CreateDirectory(basePath);
        Directory.CreateDirectory(inputPath);

        await Parallel.ForEachAsync(
            Enumerable.Range(1, 25),
            async (d, _) => await SetupDayForYearAsync(year, d, client));

        var adventTemplate = await File.ReadAllTextAsync($"{TemplatePath}/Advent.txt", Encoding.Default);
        adventTemplate = adventTemplate.Replace("__YEAR__", $"{year}");
        await File.WriteAllTextAsync($"{basePath}/Advent{year}.cs", adventTemplate, Encoding.Default);

        Console.WriteLine($"Finished setting up year {year}.");
    }

    public static async Task SetupDayForYearAsync(int year, int day, AocClient client)
    {
        SetupPaths(year);

        var basePath = $"./{year}";
        var inputPath = $"{basePath}/input";
        var dayPadded = day.ToString().PadLeft(2, '0');

        var dayFilePath = $"{basePath}/Day{dayPadded}.cs";
        var examplePath = $"{inputPath}/{dayPadded}_example.txt";

        if (File.Exists(dayFilePath))
        {
            Console.WriteLine($"Day {dayPadded} for year {year} already exists. Skipping.");
            return;
        }

        var dayTemplate = await File.ReadAllTextAsync(Path.Combine(TemplatePath, "Day.txt"));
        var content = dayTemplate
            .Replace("__YEAR__", $"{year}")
            .Replace("__DAY__", $"{day}")
            .Replace("__DAYPAD__", $"{dayPadded}");

        await Task.WhenAll(
            File.WriteAllTextAsync(dayFilePath, content, Encoding.Default),
            Utils.FetchInputForDayAsync(year, day, client),
            File.Exists(examplePath)
                ? Task.CompletedTask
                : File.WriteAllTextAsync(examplePath, string.Empty, Encoding.Default));
    }

    private static void SetupPaths(int year)
    {
        var basePath = $"./{year}";
        var inputPath = $"{basePath}/input";

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);
        if (!Directory.Exists(inputPath))
            Directory.CreateDirectory(inputPath);
    }
}