using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    public class Day05 : Day
    {
        public Day05() : base(5, 2020)
        {
            
        }

        public override long Part1(List<string> input)
        {
             
            var ids = input.Where(i => i.Count() == 10).Select(i => GetSeatId(i.Substring(0, 7), i.Substring(7), 0, 127, 0, 7)).ToList();
            return ids.Any() ? ids.Max() : 0;
        }
        public override long Part2(List<string> input)
        {
             
            var ids = input.Where(i => i.Count() == 10).Select(i => GetSeatId(i.Substring(0, 7), i.Substring(7), 0, 127, 0, 7)).ToList();
            var occupied = new int[127*8];
            ids.ForEach(id => {
                occupied[id] = 1;
            });
            var possible = occupied.Select((id, idx) => {
             if(idx != 0 && idx != occupied.Length-1 && occupied[idx] == 0 && occupied[idx-1] == 1 && occupied[idx+1]==1)
             {
                 return idx;
             }
             return -1;
            }).ToList();
            return possible.FirstOrDefault(i => i != -1);
        }

        static int GetSeatId(string row, string col, int rowMin, int rowMax, int colMin, int colMax)
        {
            if(row.Count() == 1)
            {
                rowMin = row.ElementAt(0) == 'B' ? rowMax : rowMin;
                rowMax = rowMin;

                if(col.Count() == 1)
                {
                    var seat = col.ElementAt(0) == 'L' ? colMin : colMax;
                    
                    return rowMin * 8 + seat;
                }
                var inBetweenCols = colMax - colMin;
                var newColMin = col.ElementAt(0) == 'R' ? colMax - (inBetweenCols/2) : colMin;
                var newColMax = col.ElementAt(0) == 'L' ? colMin + inBetweenCols/2 : colMax;

                return GetSeatId(row, col.Substring(1), rowMin, rowMax, newColMin, newColMax);
            }
            var inBetweenRows = rowMax - rowMin;
            var newRowMin = row.ElementAt(0) == 'B' ? rowMax - (inBetweenRows/2) : rowMin;
            var newRowMax = row.ElementAt(0) == 'F' ? rowMin + inBetweenRows/2 : rowMax;

            return GetSeatId(row.Substring(1), col, newRowMin, newRowMax, colMin, colMax);
        }
    }
}
