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
            [Option('e', "useExample", Required = false, Default = false, HelpText = "Set to true to use example data")]
            public bool UseExample {get;set;} = false;

            [Option('d', "day", Required = false, Default = 0, HelpText = "Day to run (1-25). 0 will run all days. -1 or invalid value will run the latest day.")]
            public int Day {get;set;} = 0;
        }
        

        static async Task Main(string[] args)
        {
            var allDays = Enumerable.Range(1, 25).ToArray();

            var a = new Advent2021();
            
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(options =>
            {
                Console.WriteLine($"Options: UseExample = {options.UseExample}, Day = {options.Day}");

                var daysToRun = new[]{Latest};
                if(options.Day == 0)
                    daysToRun = allDays;
                else if(options.Day > 0 && options.Day <= 25)
                    daysToRun = new[]{options.Day};

                return a.Solve(options.UseExample, daysToRun);
            });
            a.PresentAll();
            
        }
    }
}
