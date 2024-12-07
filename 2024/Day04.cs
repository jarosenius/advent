using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day04() : Day(4, 2024)
{
    public override object Part1(List<string> input)
    {
        var map = input.Select(row => row.ToCharArray()).ToArray();
        var toFind = new char[] { 'M', 'A', 'S' };
        return map.SelectMany((row, y) => row.Select((cell, x) => cell == 'X' 
            ? Direction.DirectionsWithDiagonals.Count(d => CheckDirection(map.ElementAt, x, y, d, toFind)) 
            : 0)).Sum();
    }

    public override object Part2(List<string> input)
    {
        var map = input.Select(row => row.ToCharArray()).ToArray();
        return map.SelectMany((row, y) => row.Select((cell, x) => cell == 'A' && IsXMas(x, y, map) ? 1 : 0)).Sum();
    }

    private static bool IsXMas(int xPos, int yPos, char[][] map)
    {
        return CheckDirections(map, xPos, yPos, ['S'], ['S'], ['M'], ['M']) ||
            CheckDirections(map, xPos, yPos, ['M'], ['M'], ['S'], ['S']) ||
            CheckDirections(map, xPos, yPos, ['M'], ['S'], ['M'], ['S']) ||
            CheckDirections(map, xPos, yPos, ['S'], ['M'], ['S'], ['M']);
    }

    private static bool CheckDirections(char[][] map, int xPos, int yPos, char[] expectedUpRight, char[] expectedDownRight, char[] expectedUpLeft, char[] expectedDownLeft)
    {
        return CheckDirection(map.ElementAt, xPos, yPos, Direction.UpRight, expectedUpRight) &&
            CheckDirection(map.ElementAt, xPos, yPos, Direction.DownRight, expectedDownRight) &&
            CheckDirection(map.ElementAt, xPos, yPos, Direction.UpLeft, expectedUpLeft) &&
            CheckDirection(map.ElementAt, xPos, yPos, Direction.DownLeft, expectedDownLeft);
    }

    private static bool CheckDirection(Func<int, int, char> getElement, int xPos, int yPos, Coordinate direction, char[] toFind) =>
        Enumerable.Range(1, toFind.Length).Select(i => getElement(xPos + i * direction.X, yPos + i * direction.Y)).SequenceEqual(toFind);
}

