using System;
using System.Collections.Generic;
using System.Linq;
namespace Advent.y2021
{
    public class Day16 : Day
    {
        public Day16() : base(16, 2021)
        {
            
        }

        public override long Part1(List<string> input)
        {
            var binString = string.Join("", input.First().Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            return DecodePacket(binString).Packet.VersionSum;
        }

        public override long Part2(List<string> input)
        {
            var binString = string.Join("", input.First().Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            return DecodePacket(binString).Packet.Solve();
        }
        private (Packet Packet, int Offset) DecodePacket(string packet)
        {
            List<Packet> res = new();
            var version = (int)Utils.ConvertFromBinaryString(packet[..3]);            
            var type = (int)Utils.ConvertFromBinaryString(packet[3..6]);     
            
            var i = 6;
            if(type == 4) // Packnet contains number
            {
                string current;
                List<string> nums = new();
                do
                {
                    current = packet[i..(i+=5)];
                    nums.Add(current[1..]);
                } while (current[0] != '0');
                var num = Utils.ConvertFromBinaryString(string.Join("",nums));
                return (new Packet(version, type, num, new()), i);
            }
            else // Packet contains operator
            {
                var lengthTypeId = packet[i++];
                if(lengthTypeId == '0')
                {
                    var length = (int)Utils.ConvertFromBinaryString(packet[i..(i+=15)]);
                    while(length > 0)
                    {
                        var (p, ii) = DecodePacket(packet[i..(i+length)]);
                        res.Add(p);
                        i+=ii;
                        length-=ii;
                    }
                }
                else
                {
                    var packets = Utils.ConvertFromBinaryString(packet[i..(i+=11)]);
                    for (int j = 0; j < packets; j++)
                    {
                        var (p, ii) = DecodePacket(packet[i..]);
                        res.Add(p);
                        i+=ii;
                    }
                }
            }

            return (new Packet(version, type, 0, res), i);
        }
        private record Packet(int Version, int IType, long Value, List<Packet> subExpr)
        {
            public long VersionSum => Version + subExpr.Sum(s => s.VersionSum);

            public long Solve()
            {
                switch (IType)
                {
                    case 0:
                        return subExpr.Sum(s => s.Solve());
                    case 1:
                        return subExpr.Aggregate(1L, (r, packet) => r * packet.Solve());
                    case 2:
                        return subExpr.Min(s => s.Solve());
                    case 3:
                        return subExpr.Max(s => s.Solve());
                    case 4:
                        return Value;
                    case 5:
                        return subExpr[0].Solve() > subExpr[1].Solve() ? 1 : 0;
                    case 6:
                        return subExpr[0].Solve() < subExpr[1].Solve() ? 1 : 0;
                    case 7:
                        return subExpr[0].Solve() == subExpr[1].Solve() ? 1 : 0;
                    default:
                        throw new Exception();
                }
            }
        }
    }
}
