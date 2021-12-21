using System;
using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day21 : Day
    {
        public Day21() : base(21, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var players = input.Select(l => int.Parse(l.Split(":")[1])).ToArray();
            var target = 1000;
            var (p1score, p2score, dieThrows) = Play(players[0], players[1], target);
            var loser = (p1score >= target ? p2score : p1score);
            return dieThrows * loser;
        }
        public override long Part2(List<string> input)
        {
            return 0;
        }

        private (int P1Score, int P2Score, int DieThrows) Play(int p1pos, int p2pos, int target)
        {
            var p1score = 0;
            var p2score = 0;
            var dieThrows = 0;
            var dice = 1;
            do
            {
                if(((p1pos, p1score, dice, dieThrows) = TakeTurn(p1pos, p1score, dice, dieThrows)).p1score >= target 
                || ((p2pos, p2score, dice, dieThrows) = TakeTurn(p2pos, p2score, dice, dieThrows)).p2score >= target)
                    break;
            } while (true);
            return (p1score, p2score, dieThrows);
        }

        (int Pos, int Score, int Dice, int DieThrows) TakeTurn(int pos, int score, int dice, int dieThrows)
        {
            int Throw()
			{
				var result = dice++;
				dieThrows++;
				if (dice > 100) 
                    dice = 1;
				return result;
			}
            pos += (Throw() + Throw() + Throw()) % 10;
            score += (pos -= pos > 10 ? 10 : 0);
            return (pos, score, dice, dieThrows);
        }
    }
}
