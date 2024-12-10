using System.Collections.Generic;
using System.Linq;

namespace Advent.y2024;

[AoC(2024)]
public class Day09() : Day(9, 2024)
{
    public override object Part1(List<string> input)
    {
        var diskmap = CreateDiskMap(input);
        MoveFiles(diskmap);
        return CalculateCheckSum(diskmap);
    }

    public override object Part2(List<string> input)
    {
        var diskmap = CreateDiskMap(input);
        MoveFiles(diskmap, false);
        return CalculateCheckSum(diskmap);
    }

    private static List<Block> CreateDiskMap(List<string> input)
    {
        var id = 0;
        var diskmap = new List<Block>();
        if (input[0].Length % 2 == 1)
            input[0] = input[0] + "0";

        input[0].Chunk(2).ForEach(c => {
            diskmap.Add(new Block(id++, c[0] - '0'));
            diskmap.Add(new Block(-1, c[1] - '0'));
        });

        return diskmap;
    }

    private static void MoveFiles(List<Block> diskmap, bool allowFragmented = true)
    {
        for (var i = 0; i < diskmap.Count; i++)
        {
            var current = diskmap[i];
            if (current.Id != -1)
                continue;

            for (var j = diskmap.Count - 1; j > i; j--)
            {
                var last = diskmap[j];
                if (last.Id != -1)
                {
                    var stop = true;
                    if(current.BlockSize == last.BlockSize)
                    {
                        current.Id = last.Id;
                        last.Id = -1;
                    }
                    else if(current.BlockSize > last.BlockSize)
                    {
                        current.BlockSize -= last.BlockSize;
                        diskmap.Insert(i, new Block(last.Id, last.BlockSize));
                        last.Id = -1;
                    }
                    else if(allowFragmented)
                    {
                        last.BlockSize -= current.BlockSize;
                        current.Id = last.Id;
                    }
                    else
                        stop = false;
                    
                    if(stop)
                        break;
                }
            }
        }
    }

    private static long CalculateCheckSum(List<Block> diskmap)
    {
        return diskmap.Aggregate((0L, 0), (data, m) =>
                    (m.Id == -1
                        ? data.Item1
                        : data.Item1 + CalculateBlock(data.Item2, m),
                        data.Item2 + m.BlockSize))
                    .Item1;
    }

    private static long CalculateBlock(long start, Block block) => Enumerable.Range(0, block.BlockSize).Sum(i => (start+i) * block.Id);

    private record Block(int Id, int BlockSize = 1)
    {
        public int Id = Id;
        public int BlockSize = BlockSize;
    }
}
