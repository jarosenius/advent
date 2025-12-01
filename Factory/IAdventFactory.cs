using System.Threading.Tasks;

namespace Advent.Factory;

public interface IAdventFactory
{
    Task<Advent> CreateForYearAsync(int year);
}