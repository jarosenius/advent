using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2024;

[AoC(2024)]
public class Day11() : Day(11, 2024)
{
    public override object Part1(List<string> input) => Blink(input[0].SplitByAndParseToInt(" "), 25);
    public override object Part2(List<string> input) => Blink(input[0].SplitByAndParseToInt(" "), 75);

    private static long Blink(int[] data, int blinks)
    {
        var stones = new ConcurrentDictionary<long, long>(
            data.Select(d => KeyValuePair.Create((long)d, 1L)));

        while(blinks-- > 0)
        {
            var updated = new ConcurrentDictionary<long, long>();
            if(stones.TryRemove(0, out var zero))
                Update(updated, 1, zero);
            
            stones.Where(s => s.Key.ToString().Length % 2 == 0).ToList().ForEach(s => {
                if(stones.TryRemove(s.Key, out var value))
                {
                    var key = s.Key.ToString();
                    var half = key.Length/2;
                    Update(updated, int.Parse(key[..half]), value);
                    Update(updated, int.Parse(key[half..]), value);
                }
            });

            stones.ForEach(s => {
                if(stones.TryRemove(s.Key, out var value))
                    Update(updated, s.Key*2024, value);
            });

            stones = updated;
        }
        
        return stones.Values.Sum();
    }

    private static long Update(ConcurrentDictionary<long, long> dictionary, long key, long value) => dictionary.AddOrUpdate(key, value, (_, v) => v+value);
}
