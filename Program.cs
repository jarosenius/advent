using System;
using System.Threading.Tasks;
using System.Linq;
using Advent.y2021;
using Advent.y2020;
using CommandLine;

namespace Advent
{

    class Program
    {
        static int Latest = DateTime.Now.Day > 25 ? 25 : DateTime.Now.Day;
        public class Options
        {
            [Option('y', "year", Required = false, Default = 2021, HelpText = "Set year to run.")]
            public int Year {get;set;} = 2021;

            [Option('e', "useExample", Required = false, Default = false, HelpText = "Set to true to use example data.")]
            public bool UseExample {get;set;} = false;

            [Option('d', "day", Required = false, Default = 0, HelpText = "Day to run (1-25). 0 will run all days. -1 or invalid value will run the latest day.")]
            public int Day {get;set;} = 0;
        }
        

        static async Task Main(string[] args)
        {
            var allDays = Enumerable.Range(1, 25).ToArray();

            Advent a = null;
            
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(options =>
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
            });

            a?.PresentAll();
        }
    }
}
