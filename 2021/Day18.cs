using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Advent.y2021
{
    [AoC(2021)]
    public class Day18 : Day
    {
        public Day18() : base(18, 2021)
        {

        }

        public override object Part1(List<string> input)
        {
            var json = input.Select(s => JsonConvert.DeserializeObject<JArray>(s)).Select(j => SnailNum.Create(j)).ToArray();
            var a = json.Skip(1).Aggregate(json[0], (r, s) => r + s);
            return a.Magnitude;
        }
        public override object Part2(List<string> input)
        {
            long best = 0;
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input.Count; j++)
                {
                    if (i == j)
                        continue;
                    var sum = SnailNum.Create(JsonConvert.DeserializeObject<JArray>(input[i])) + SnailNum.Create(JsonConvert.DeserializeObject<JArray>(input[j]));
                    if (sum.Magnitude > best)
                        best = sum.Magnitude;
                }
            }
            return best;
        }

        private class SnailNum
        {
            public int? RegularLeft { get; set; }
            public int? RegularRight { get; set; }
            public SnailNum Left { get; private set; }
            public SnailNum Right { get; private set; }
            public SnailNum Parent { get; set; }
            public int NumParents => Parent == null ? 0 : Parent.NumParents + 1;
            public long Magnitude =>
                (3 * (RegularLeft.HasValue ? RegularLeft.Value : Left.Magnitude))
                + (2 * (RegularRight.HasValue ? RegularRight.Value : Right.Magnitude));


            public SnailNum(SnailNum left, SnailNum right)
            {
                Left = left;
                Left.Parent = this;
                Right = right;
                Right.Parent = this;
            }

            public SnailNum(int left, int right)
            {
                RegularLeft = left;
                RegularRight = right;
            }

            public SnailNum(SnailNum left, int right)
            {
                Left = left;
                Left.Parent = this;
                RegularRight = right;
            }
            public SnailNum(int left, SnailNum right)
            {
                RegularLeft = left;
                Right = right;
                Right.Parent = this;
            }

            public static SnailNum Create(JToken input)
            {
                var left = input.ElementAt(0);
                var right = input.ElementAt(1);
                if (left.Type == JTokenType.Integer && right.Type == JTokenType.Integer)
                    return new SnailNum(left.Value<int>(), right.Value<int>());
                else if (left.Type == JTokenType.Integer)
                    return new SnailNum(left.Value<int>(), SnailNum.Create(right));
                else if (right.Type == JTokenType.Integer)
                    return new SnailNum(SnailNum.Create(left), right.Value<int>());
                return new SnailNum(SnailNum.Create(left), SnailNum.Create(right));
            }
            public static SnailNum operator +(SnailNum a, SnailNum b)
            {
                return Reduce(new SnailNum(a, b));
            }

            public override string ToString()
            {
                return $"[{Left?.ToString() ?? RegularLeft.Value.ToString()},{Right?.ToString() ?? RegularRight.Value.ToString()}]";
            }

            public void Replace(SnailNum replace, int val)
            {
                if (Left == replace)
                {
                    RegularLeft = val;
                    Left = null;
                }
                else if (Right == replace)
                {
                    RegularRight = val;
                    Right = null;
                }
            }

            private static SnailNum Reduce(SnailNum snail)
            {
                var modified = false;
                do
                {
                    var exploded = false;
                    do
                    {
                        exploded = snail.Explode();
                    } while (exploded);

                    var split = snail.Split();
                    modified = exploded || split;
                } while (modified);

                return snail;
            }

            private bool Explode()
            {
                if (NumParents >= 4)
                {
                    Parent.AddToFirstOnLeft(RegularLeft.Value, this, true);
                    Parent.AddToFirstOnRight(RegularRight.Value, this, false);
                    Parent.Replace(this, 0);
                    return true;
                }

                return (Left?.Explode() ?? false) || (Right?.Explode() ?? false);
            }

            private bool Split()
            {
                if (RegularLeft.HasValue && RegularLeft.Value >= 10)
                {
                    var leftValue = RegularLeft.Value / 2;
                    var rightValue = RegularLeft.Value / 2 + RegularLeft.Value % 2;
                    Left = new SnailNum(leftValue, rightValue);
                    Left.Parent = this;
                    RegularLeft = null;
                    return true;
                }
                else if (Left != null && Left.Split())
                    return true;

                if (RegularRight.HasValue && RegularRight.Value >= 10)
                {
                    var leftValue = RegularRight.Value / 2;
                    var rightValue = RegularRight.Value / 2 + RegularRight.Value % 2;
                    Right = new SnailNum(leftValue, rightValue);
                    Right.Parent = this;
                    RegularRight = null;
                    return true;
                }
                else if (Right != null && Right.Split())
                    return true;

                return false;
            }

            private bool AddToFirstOnRight(int value, SnailNum source, bool srcIsLeft)
            {
                if (RegularRight.HasValue)
                {
                    RegularRight += value;
                    return true;
                }
                else if (Right != null && source != Right
                    && (srcIsLeft
                        ? Right.AddToFirstOnRight(value, this, srcIsLeft)
                        : Right.AddToFirstOnLeft(value, this, srcIsLeft)))
                {
                    return true;
                }
                else if (Parent != null)
                {
                    return Parent.AddToFirstOnRight(value, this, srcIsLeft);
                }
                return false;
            }

            private bool AddToFirstOnLeft(int value, SnailNum source, bool srcIsLeft)
            {
                if (RegularLeft.HasValue)
                {
                    RegularLeft += value;
                    return true;
                }
                else if (Left != null && source != Left
                    && (srcIsLeft == false
                        ? Left.AddToFirstOnLeft(value, this, srcIsLeft)
                        : Left.AddToFirstOnRight(value, this, srcIsLeft)))
                {
                    return true;
                }
                else if (Parent != null)
                {
                    return Parent.AddToFirstOnLeft(value, this, srcIsLeft);
                }
                return false;
            }
        }
    }
}
