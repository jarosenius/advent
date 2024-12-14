using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Common;
using SixLabors.ImageSharp;

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
        //var wh = UsedExampleData ? (11, 7) : (101, 103);
        //if(!UsedExampleData)
        //    GenerateImages(wh.Item1, wh.Item2, GetRobots(input), 10000);
        Console.WriteLine("I don't know how I should approach this as I don't know what I'm looking for. Generating images and scanning through them manually.");
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
        var (q1, q2, q3, q4) = CountRobotsInQuadrants(width, height, GetUpdateRobotPositions(width, height, n, robots));
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

    private static void GenerateImages(int width, int height, List<(Coordinate Position, Coordinate Velocity)> robots, int iterations)
    {
        for (int n = 0; n < iterations; n++)
        {
            for (var i = 0; i < robots.Count; i++)
            {
                var (p, v) = robots[i];

                var px = (p.X + v.X) % width;
                var py = (p.Y + v.Y) % height;

                if (px < 0)
                    px += width;
                if (py < 0)
                    py += height;

                robots[i] = (new Coordinate(px, py), v);
            }

            var grid = new int[width, height];
            robots.ForEach(r => grid[r.Position.X, r.Position.Y]++);

            CreateImage(grid, width, height, n+1);
            
        }
    }

    private static void CreateImage(int[,] grid, int width, int height, int iteration)
    {
        using var image = new Image<SixLabors.ImageSharp.PixelFormats.L8>(width, height);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                image[x, y] = new SixLabors.ImageSharp.PixelFormats.L8(grid[x, y] > 0 ? (byte)255 : (byte)0);
        image.Save($"../../aoc/2024/14/robot_positions_{iteration:D5}.png");
    }
}

