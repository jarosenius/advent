using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Advent;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdventYears(this IServiceCollection services)
    {
        var adventTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Advent)) && !t.IsAbstract);

        foreach (var type in adventTypes)
        {
            services.AddKeyedSingleton(typeof(Advent), type, type);
        }

        return services;
    }
}