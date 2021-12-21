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
            var players = input.Select(l => int.Parse(l.Split(":")[1])).ToArray();
            return PlayPart2(players[0], players[1], 21);
        }

        private (int P1Score, int P2Score, int DiceThrows) Play(int p1pos, int p2pos, int target)
        {
            (int Pos, int Score, int Dice, int DiceThrows) TakeTurn(int pos, int score, int dice, int diceThrows)
            {
                int Throw()
                {
                    var result = dice++;
                    diceThrows++;
                    if (dice > 100) 
                        dice = 1;
                    return result;
                }
                pos += (Throw() + Throw() + Throw()) % 10;
                score += (pos -= pos > 10 ? 10 : 0);
                return (pos, score, dice, diceThrows);
            }

            var p1score = 0;
            var p2score = 0;
            var diceThrows = 0;
            var dice = 1;
            do
            {
                if(((p1pos, p1score, dice, diceThrows) = TakeTurn(p1pos, p1score, dice, diceThrows)).p1score >= target 
                || ((p2pos, p2score, dice, diceThrows) = TakeTurn(p2pos, p2score, dice, diceThrows)).p2score >= target)
                    break;
            } while (true);
            return (p1score, p2score, diceThrows);
        }

        private long PlayPart2(int p1pos, int p2pos, int target)
        {
            var outcomes = Enumerable.Range(1, 3).SelectMany(d1 => Enumerable.Range(1,3).SelectMany(d2 => Enumerable.Range(1,3).Select(d3 => d1+d2+d3))).ToArray();
            Dictionary<(int P1pos, int P2pos, int P1score, int P2score, int Result, bool IsPlayerOne), (long P1Wins, long P2Wins)> results = new(); 

            (long P1Wins, long P2Wins) DiracTurn((int P1pos, int P2pos, int P1score, int P2score, int Result, bool IsPlayerOne) round)
            {
                if(results.ContainsKey(round))
                    return results[round];
                var pos = round.IsPlayerOne ? round.P1pos : round.P2pos;
                var score = round.IsPlayerOne ? round.P1score : round.P2score;
                pos += round.Result;
                score += (pos -= pos > 10 ? 10 : 0);
                if(score >= target)
                    return (results[round] = round.IsPlayerOne ? (1, 0) : (0, 1));

                var current = round;

                if(round.IsPlayerOne) 
                    (round.P1pos, round.P1score) = (pos, score);
                else 
                    (round.P2pos, round.P2score) = (pos, score);

                round.IsPlayerOne = !round.IsPlayerOne;

                var outcome = outcomes.Select(o => DiracTurn((round.P1pos, round.P2pos, round.P1score, round.P2score, o, round.IsPlayerOne))).ToArray();
                var (p1wins, p2wins) = outcome.Aggregate((0L, 0L), (a, c) => (a.Item1+c.P1Wins, a.Item2+c.P2Wins));
                return results[current] = (p1wins, p2wins);
            }       
            
            var outcome = outcomes.Select(t => DiracTurn((p1pos, p2pos, 0, 0, t, true))).ToArray();
            var (p1wins, p2wins) = outcome.Aggregate((0L, 0L), (a, c) => (a.Item1+c.P1Wins, a.Item2+c.P2Wins));
            return p1wins > p2wins ? p1wins : p2wins;
        }
    }
}
