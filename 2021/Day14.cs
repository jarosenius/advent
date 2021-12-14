using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace Advent.y2021
{
    public class Day14 : Day
    {
        public Day14() : base(14, 2021)
        {
            
        }
        public override long Part1(List<string> input)
        {
            return ApplyInsertions(10, input);
        }
        public override long Part2(List<string> input)
        {
            return ApplyInsertions(40, input);
        }

        private long ApplyInsertions(int numberOfInsertions, List<string> input)
        {
            var template = input.First();
            var pairInserts = input.Skip(2).Select(p => p.Split(" -> ")).ToDictionary(p => p[0], p => p[1]);
            ConcurrentDictionary<string, long> results = new();
            
            // Pair letters two by two and update number of occurrences
            template.Zip(template.Skip(1)).ForEach(p => results.AddOrUpdate($"{p.First}{p.Second}", 1, (k, v) => v+1));
            // Add the last letter to make sure we count it later.
            results.AddOrUpdate(template.Last().ToString(), 1, (k, v) => v+1);
            
            Enumerable.Range(0, numberOfInsertions).ForEach(i => 
            {
                ConcurrentDictionary<string, long> inserts = new();
                results.ForEach(kvp =>
                {
                    var (key, val) = kvp;
                    if(pairInserts.ContainsKey(key))
                    {
                        // Update number of occurrences for new pairs after insert with old values.
                        var insert = pairInserts[key];
                        inserts.AddOrUpdate(key[0] + insert, val, (k, v) => v+val);
                        inserts.AddOrUpdate(insert + key[1], val, (k, v) => v+val);
                    }
                    else // The last single letter won't match anyting in pairInserts
                        inserts.AddOrUpdate(key, 1, (key, v) => v+val);
                });
                results = inserts;
            });
            // Because we added the last letter, all first letters in the key will be the number of occurrences of that letter.
            var groups= results.GroupBy(kvp => kvp.Key[0], kvp => kvp.Value).ToDictionary(k => k.Key, k => k.Sum(c => (long)c));
            return groups.Values.Max() - groups.Values.Min();
        }
    }
}