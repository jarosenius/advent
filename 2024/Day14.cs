using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Advent.Common;

namespace Advent.y2024;

[AoC(2024)]
public class Day14() : Day(14, 2024)
{
    public override object Part1(List<string> input)
    {
        var wh = UsedExampleData ? (11, 7) : (101, 103);
        return CalculateSafetyFactor(wh.Item1, wh.Item2, 100, GetRobots(input));
    }
    
    public override object Part2(List<string> input)
    {
        if(UsedExampleData)
        {
            Console.WriteLine("Cannot use example data for this part");
            return -1;
        }
        var wh = (101, 103);
        var robots = GetRobots(input).ToImmutableList();
        var current = CalculateSafetyFactor(wh.Item1, wh.Item2, 0, [.. robots]);
        for (var i = 1; i < 20000; i++)
        {
            var copy = robots.ToList();
            var now = CalculateSafetyFactor(wh.Item1, wh.Item2, i, copy);

            /*
            * Based on discussions in the AoC megathread
            * If we have a low safety factor or if all robots are in unique positions then we have the tree.
            */
            if((double)now/current < 0.25 || copy.DistinctBy(kvp => kvp.Item1).Count() == copy.Count) 
                return i;
        }
        return -1;
    }

    private static List<(Coordinate, Coordinate)> GetRobots(List<string> input)
    {
        return input.Select(r =>
        {
            var parts = r.Split(' ').Select(p => p[2..].SplitByAndParseToInt(",")).ToList();
            return (new Coordinate(parts[0][0], parts[0][1]), new Coordinate(parts[1][0], parts[1][1]));
        }).ToList();
    }

    public static int CalculateSafetyFactor(int width, int height, int n, List<(Coordinate Position, Coordinate Velocity)> robots)
    {
        var updated = GetUpdateRobotPositions(width, height, n, robots);
        var (q1, q2, q3, q4) = CountRobotsInQuadrants(width, height, updated);
        return q1 * q2 * q3 * q4;
    }

    private static int[,] GetUpdateRobotPositions(int width, int height, int n, List<(Coordinate Position, Coordinate Velocity)> robots)
    {
        for (var i = 0; i < robots.Count; i++)
        {
            var (p, v) = robots[i];

            var px = (p.X + n * v.X) % width;
            var py = (p.Y + n * v.Y) % height;

            if (px < 0)
                px += width;
            if (py < 0)
                py += height;

            robots[i] = (new Coordinate(px, py), v);
        }

        var grid = new int[width, height];
        robots.ForEach(r => grid[r.Position.X, r.Position.Y]++);
        return grid;
    }

    private static (int, int, int, int) CountRobotsInQuadrants(int width, int height, int[,] grid)
    {
        var midX = width / 2;
        var midY = height / 2;
        int q1 = 0, q2 = 0, q3 = 0, q4 = 0;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (x == midX || y == midY)
                    continue;

                if (x < midX && y < midY)
                    q1 += grid[x, y];
                else if (x >= midX && y < midY)
                    q2 += grid[x, y];
                else if (x < midX && y >= midY)
                    q3 += grid[x, y];
                else if (x >= midX && y >= midY)
                    q4 += grid[x, y];
            }
        }
        return (q1, q2, q3, q4);
    }
}
