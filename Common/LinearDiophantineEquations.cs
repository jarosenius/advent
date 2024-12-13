namespace Advent.Common
{
    public static class LinearDiophantineEquations
    {
        public static (long A, long B) Solve((long X, long Y) a, (long X, long Y) b, (long A, long B) Target)
        {
            // aX + bY = c
            // a.X*minA + b.X*Y = Prize.X
            // a.Y*minA + b.Y*Y = Prize.Y
            // minA = (Prize.X-b.X*Y) / a.X
            // a.Y * (Prize.X - b.X*Y)/a.X + b.Y*Y = Prize.Y
            // (a.Y*Prize.X - a.Y*b.X*Y) / a.X + b.Y*Y = Prize.Y
            // (a.Y*Prize.X - a.Y*b.X*Y) + b.Y*a.X*Y = Prize.Y * a.X
            // Y = (Prize.Y * a.X - Prize.X*a.Y) / (b.Y*a.X - b.X*a.Y)
            // X = Prize.X - b.X * Y
            // X = (Prize.X-b.X * Y) / a.X
            long Y = (Target.B * a.X - Target.A * a.Y) / (b.Y * a.X - b.X * a.Y);
            long X = (Target.A - b.X * Y) / a.X;

            if (X * a.X + Y * b.X == Target.A && X * a.Y + Y * b.Y == Target.B)
                return (X, Y);
            else
                return (-1, -1);
        }
    }
}
