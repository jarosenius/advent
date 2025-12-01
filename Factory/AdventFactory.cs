using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Advent.Factory;

public class AdventFactory(IServiceProvider serviceProvider) : IAdventFactory
{
    public async Task<Advent> CreateForYearAsync(int year)
    {
        if (!Advent.SupportedYears.TryGetValue(year, out var adventType))
            return null;

        var advent = (Advent)serviceProvider.GetRequiredKeyedService(typeof(Advent), adventType);
        await advent.SetupDays();

        return advent;
    }
}