using System.Net.Mime;
using System.Text;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using CommandLine;

namespace Advent
{

    class Program
    {
        static int Latest = DateTime.Now.Day > 25 ? 25 : DateTime.Now.Day;
        public class Options
        {
            [Option('y', "year", Required = false, HelpText = "Set year to run. Defaults to last supported year.")]
            public int Year {get;set;} = Advent.SupportedYears.Max(s => s.Key);

            [Option('e', "useExample", Required = false, Default = false, HelpText = "Set to true to use example data.")]
            public bool UseExample {get;set;} = false;

            [Option('d', "day", Required = false, Default = 0, HelpText = "Day to run (1-25). 0 will run all days. -1 or invalid value will run the latest day.")]
            public int Day {get;set;} = 0;

            [Option('s', "setupYear", Required = false, HelpText = "Setup folders and files for a new year. Must be a number.")]
            public int? SetupYear {get;set;}
        }
        

        static async Task Main(string[] args)
        {
            var allDays = Enumerable.Range(1, 25).ToArray();

            Advent a = null;
            
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(options =>
            {
                if(options.SetupYear.HasValue)
                {     
                    return SetupNewYear(options.SetupYear.Value);
                }
                else
                {
                    Console.WriteLine($"Options: UseExample = {options.UseExample}, Year = {options.Year}, Day = {options.Day}");
                    a = Advent.CreateForYear(options.Year);
                    if(a == null)
                    {
                        Console.WriteLine($"Could not create an AoC. Supported years are {string.Join(", ", Advent.SupportedYears.Select(s => s.Key))}");
                        return Task.CompletedTask;
                    }

                    var daysToRun = new[]{Latest};
                    if(options.Day == 0)
                        daysToRun = allDays;
                    else if(options.Day > 0 && options.Day <= 25)
                        daysToRun = new[]{options.Day};

                    return a.Solve(options.UseExample, daysToRun);
                }
            });

            a?.PresentAll();
        }

        private static async Task SetupNewYear(int y)
        {
            Console.WriteLine($"Setting up new year {y}");
            Console.WriteLine(Directory.GetCurrentDirectory());
            if(Directory.Exists($"./{y}"))
            {
                Console.WriteLine($"Year {y} already exists. Exiting.");
                return;
            }
        
            var basePath = $"./{y}";
            var inputPath = $"{basePath}/input";
            var templatePath = $"./TemplateYear";

            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(inputPath);
            
            var days = Directory.GetFiles(templatePath, "Day*.cs", SearchOption.TopDirectoryOnly);
            await Parallel.ForEachAsync(days, async (day, c) =>
            {
                var content = await File.ReadAllTextAsync(day, Encoding.Default, c);
                content = content.Replace("Advent.y202x", $"Advent.y{y}").Replace(", 2021)", $", {y})");
                var name = new FileInfo(day).Name;
                var newPath = $"{basePath}/{name}";
                await File.WriteAllTextAsync(newPath, content, Encoding.Default, c);
            });

            var advent = await File.ReadAllTextAsync($"{templatePath}/Advent202x.cs", Encoding.Default);
            advent = advent.Replace("202x", y.ToString()).Replace(" base(2020)", $" base({y})").Replace("//[AoC(2020)]", $"[AoC({y})]");
            await File.WriteAllTextAsync($"{basePath}/Advent{y}.cs", advent, Encoding.Default);

            await Parallel.ForEachAsync(Enumerable.Range(1, 25), async (day, c) =>
            {
                var dayName = day.ToString().PadLeft(2, '0');
                await File.WriteAllTextAsync($"{inputPath}/{dayName}.txt", "", Encoding.Default, c);
                await File.WriteAllTextAsync($"{inputPath}/{dayName}_example.txt", "", Encoding.Default, c);
            });

            Console.WriteLine($"Finished setting up year {y}.");
        }
    }
}
