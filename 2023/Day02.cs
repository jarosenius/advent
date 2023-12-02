using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day02 : Day
{
    public Day02() : base(2, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var games = input.Select(CreateGame);
        return games.Where(g => IsGamePossible(g, 12, 13, 14).Possible).Sum(g => g.Id);
    }
    public override object Part2(List<string> input)
    {
        var games = input.Select(CreateGame);
        return games.Select(g => {
            var (Red, Green, Blue) = MinCubes(g);
            return Red*Green*Blue;
        }).Sum();
    }

    private static (bool Possible, int Id) IsGamePossible(Game game, int red, int green, int blue)
    {
        var possible = game.Sets.All(g => g.Red <= red && g.Green <= green && g.Blue <= blue);
        return (possible, game.Id);
    }

    private static (int Red, int Green, int Blue) MinCubes(Game game)
    {
        var red = game.Sets.MaxBy(s => s.Red).Red;
        var green = game.Sets.MaxBy(s => s.Green).Green;
        var blue = game.Sets.MaxBy(s => s.Blue).Blue;
        return (red, green, blue);
    }

    private static Game CreateGame(string row)
    {
        var gameIdAndData = row.Split(": ");
        var gameId = int.Parse(gameIdAndData[0][5..]);
        var gameData = gameIdAndData[1].Split(';');
        var setData = gameData.Select(g => g.Trim().Split(',').Select(d => d.Trim()));
        var sets = setData.Select(CreateSet).ToList();

        return new Game(gameId, sets);
    }
    private static Set CreateSet(IEnumerable<string> setData)
    {
        var dict = new Dictionary<string, int>(){
            {"red", 0},
            {"green", 0},
            {"blue", 0}
            };
        setData.ForEach(d =>{
            var countAndColor = d.Split(' ');
            var count = int.Parse(countAndColor[0]);
            dict[countAndColor[1]] = count;
        });

        return new Set(dict["red"], dict["green"], dict["blue"]);
    }

    private record Game(int Id, List<Set> Sets);
    private record Set(int Red, int Green, int Blue);
}

