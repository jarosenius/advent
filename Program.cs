using System;
using System.Threading.Tasks;
using System.Linq;
using Advent.y2021;
using Advent.y2020;

namespace Advent
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var allDays = Enumerable.Range(1, 25).ToArray();
            var latest = DateTime.Now.Day;
            latest = latest > 25 ? 25 : latest;

            //var a = new Advent2020();
            var a = new Advent2021();
            
            //a.IsFirstRun = false;

            await a.Solve(false, latest);
            a.PresentAll();
        }
    }
}
