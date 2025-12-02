using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Advent.Factory;
using Advent.Http;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Advent
{

    public class Program
    {
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


        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async options =>
                {
                    var runner = host.Services.GetRequiredService<AdventRunner>();
                    await runner.RunAsync(options);
                });
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true)
                    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);
            })
                .ConfigureServices((_, services) =>
                {
                    services.AddAdventYears();
                    services.AddSingleton<IAdventFactory, AdventFactory>();
                    services.AddSingleton<AocClient>();
                    services.AddSingleton<AdventRunner>();
                });
    }
}
