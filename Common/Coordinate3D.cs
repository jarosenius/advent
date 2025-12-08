using System;

namespace Advent.Common;

public record Coordinate3D(int X, int Y, int Z)
{
    public static readonly Coordinate3D Zero = new(0, 0, 0);

    public int Distance(Coordinate3D c) => Math.Abs(X - c.X) + Math.Abs(Y - c.Y) + Math.Abs(Z - c.Z);

    public double EuclideanDistance(Coordinate3D c) =>
        Math.Sqrt(Math.Pow(X - c.X, 2) + Math.Pow(Y - c.Y, 2) + Math.Pow(Z - c.Z, 2));

    public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Coordinate3D operator -(Coordinate3D a) => new(-a.X, -a.Y, -a.Z);
    public static Coordinate3D operator *(int a, Coordinate3D b) => new(a * b.X, a * b.Y, a * b.Z);
    public static Coordinate3D operator *(Coordinate3D a, int b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Coordinate3D Parse(string input)
    {
        var parts = input.SplitByAndParseToInt(",");
        return new Coordinate3D(parts[0], parts[1], parts[2]);
    }
}