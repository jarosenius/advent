using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day16 : Day
    {
        public Day16() : base(16, 2020)
        {

        }

        public override object Part1(List<string> input)
        {
            var limits = new Dictionary<string, (int Min, int Max)[]>();
            var limitsRaw = new List<string>();
            var ticketRaw = new List<string>();
            var neighborRaw = new List<string>();

            input.TakeWhile(s => string.IsNullOrEmpty(s) == false).ToList().ForEach(limitsRaw.Add);
            input.Skip(limitsRaw.Count + 1).TakeWhile(s => string.IsNullOrEmpty(s) == false).ToList().ForEach(ticketRaw.Add);
            input.Skip(limitsRaw.Count + ticketRaw.Count + 2).ToList().ForEach(neighborRaw.Add);

            limitsRaw.ForEach(x => AddLimit(x, limits));
            var ticket = ticketRaw.ElementAt(1).Split(",").Select(long.Parse);
            var neighbors = neighborRaw.Skip(1).Select(n => n.Split(",")).Select(x => x.Select(y => int.Parse(y)));

            return Part1(limits, neighbors, ticket);
        }
        public override object Part2(List<string> input)
        {
            var limits = new Dictionary<string, (int Min, int Max)[]>();
            var limitsRaw = new List<string>();
            var ticketRaw = new List<string>();
            var neighborRaw = new List<string>();

            input.TakeWhile(s => string.IsNullOrEmpty(s) == false).ToList().ForEach(limitsRaw.Add);
            input.Skip(limitsRaw.Count + 1).TakeWhile(s => string.IsNullOrEmpty(s) == false).ToList().ForEach(ticketRaw.Add);
            input.Skip(limitsRaw.Count + ticketRaw.Count + 2).ToList().ForEach(neighborRaw.Add);

            limitsRaw.ForEach(x => AddLimit(x, limits));
            var ticket = ticketRaw.ElementAt(1).Split(",").Select(long.Parse);
            var neighbors = neighborRaw.Skip(1).Select(n => n.Split(",")).Select(x => x.Select(y => int.Parse(y)));

            return Part2(limits, neighbors, ticket); ;
        }

        static long Part1(Dictionary<string, (int Min, int Max)[]> limits, IEnumerable<IEnumerable<int>> neighbors, IEnumerable<long> ticket)
        {
            return InvalidTickets(limits, neighbors, ticket);
        }
        static long Part2(Dictionary<string, (int Min, int Max)[]> limits, IEnumerable<IEnumerable<int>> neighbors, IEnumerable<long> ticket)
        {
            return GetDepartures(limits, neighbors, ticket).Aggregate((x, y) => x * y);
        }
        static bool TicketValid(IEnumerable<(int Min, int Max)> allCriterion, IEnumerable<int> ticket) => ticket.All(x => allCriterion.ToList().Any(y => x >= y.Min && x <= y.Max));
        static bool AllNumbersMatch(string key, IEnumerable<int> tickets, Dictionary<string, (int Min, int Max)[]> limits) => tickets.All(x => limits[key].Any(y => x >= y.Min && x <= y.Max));
        static long InvalidTickets(Dictionary<string, (int Min, int Max)[]> limits, IEnumerable<IEnumerable<int>> neighbors, IEnumerable<long> ticket)
        {
            var result = 0;
            var flattened = limits.SelectMany(x => x.Value);
            foreach (var n in neighbors)
            {
                foreach (var v in n)
                {
                    if (!flattened.Any(x => v >= x.Min && v <= x.Max))
                        result += v;
                }

            }
            return result;
        }

        static void AddLimit(string line, Dictionary<string, (int Min, int Max)[]> limits)
        {
            var parts = line.Split(" ");
            var key = string.Join(" ", parts[..^3]);
            var value = new (int, int)[] { ProcessLimits(parts[^1]), ProcessLimits(parts[^3]) };
            limits.Add(key, value);
        }
        static (int Min, int Max) ProcessLimits(string input)
        {
            var delimited = input.Split("-");
            return (int.Parse(delimited[0]), int.Parse(delimited[1]));
        }


        static IEnumerable<long> GetDepartures(Dictionary<string, (int Min, int Max)[]> limits, IEnumerable<IEnumerable<int>> neighbors, IEnumerable<long> ticket)
        {
            var matchedLimits = new HashSet<string>();
            var result = new List<int>();

            var validTickets = neighbors.Where(x => TicketValid(limits.SelectMany(x => x.Value).ToList(), x)).ToList();
            var converted = Converted(validTickets);

            var matching = converted.Select((a, i) => (limits.Keys.Where(x => AllNumbersMatch(x, a, limits)), i)).OrderBy(x => x.Item1.Count()).ToList();

            foreach (var c in converted)
            {
                var match = matching.First();

                if (match.Item1.First().Contains("depart"))
                {
                    result.Add(match.Item2);
                }

                matchedLimits.Add(match.Item1.First());
                matching = matching.Select(x => (Fields: x.Item1.Where(y => !matchedLimits.Contains(y)), x.Item2)).Where(x => x.Fields.Count() > 0).OrderBy(x => x.Fields.Count()).ToList();
            }

            return result.Select(x => ticket.ElementAt(x));
        }
        static IEnumerable<IEnumerable<int>> Converted(IEnumerable<IEnumerable<int>> tickets)
        {
            var result = new List<IEnumerable<int>>();
            for (int i = 0; i < tickets.ElementAt(0).Count(); i++)
            {
                result.Add(tickets.Select(x => x.ElementAt(i)).ToList());
            }
            return result;
        }
    }
}
