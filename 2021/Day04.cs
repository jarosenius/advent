using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day04 : Day
    {
        public Day04() : base(4, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var bn = GetBoardsAndNumbers(input);
            return GetWinner(bn.Boards, bn.Numbers, false);
        }

        public override long Part2(List<string> input)
        {
            var bn = GetBoardsAndNumbers(input);
            return GetWinner(bn.Boards, bn.Numbers, true);
        }

        private int GetWinner(List<Board> boards, List<int> drawnNumbers, bool lastIsWinner = false)
        {
            var winners = new List<Board>();
            foreach (var n in drawnNumbers)
            {
                for (int i = 0; i < boards.Count; i++)
                {
                    if(boards[i].HasWon)
                        continue;

                    if(boards[i].Mark(n))
                    {
                        winners.Add(boards[i]);
                        if(lastIsWinner == false || winners.Count == boards.Count)
                            return boards[i].UnmarkedSum * n;
                    }
                }
            }

            return -1;
        }

        private (List<int> Numbers, List<Board> Boards) GetBoardsAndNumbers(IEnumerable<string> input)
        {
            var numbers = input.Take(1).SelectMany(s => s.SplitByAndParseToInt(",")).ToList();

            List<Board> boards = new();
            var i = 2;

            while(i < input.Count())
            {
                if(string.IsNullOrEmpty(input.ElementAt(i)))
                {
                    i++;
                }
                else
                {
                    boards.Add(Board.Create(input.Skip(i).Take(5)));
                    i+=5;
                }
            }
            return (numbers, boards);
        }

        private class Board
        {
            private readonly int Size;
            private readonly Cell[][] Cells;
            private Dictionary<int, Cell> CellsByNumber;
            public bool HasWon {get;private set;}
            public int UnmarkedSum => Cells.SelectMany(r => r.Select(c => c)).Where(c => c.Marked == false).Sum(c => c.Number);

            public Board(IEnumerable<IEnumerable<int>> rows)
            {
                Size = rows.Count();
                Cells = new Cell[Size][];
                for (int x = 0; x < Size; x++)
                {
                    Cells[x] = new Cell[Size];
                    for (int y = 0; y < Size; y++)
                    {
                        Cells[x][y] = new Cell(rows.ElementAt(x).ElementAt(y));
                    }
                }
                CellsByNumber = Cells.SelectMany(r => r.Select(c => c)).ToDictionary(c => c.Number);
            }

            public bool Mark(int number)
            {
                if(CellsByNumber.ContainsKey(number))
                {
                    CellsByNumber[number].Marked = true;
                    HasWon = CheckWin();
                    return HasWon;
                }
                return false;
            }

            public static Board Create(IEnumerable<string> input)
            {
                var rows = input.Select(s => s.SplitByAndParseToInt(" "));
                return new Board(rows);
            }

            private bool CheckWin()
            {
                return Enumerable.Range(0, Size).Any(i => Cells[i].All(r => r.Marked) || Cells.SliceColumn(i).All(c => c.Marked));
            }
        }

        private class Cell 
        {
            public int Number { get; set; }
            public bool Marked { get; set; }

            public Cell(int number, bool marked = false)
            {
                Number = number;
                Marked = marked;
            }
        }  
    }
}
