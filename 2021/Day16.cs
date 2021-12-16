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
            var binString = string.Join("", input.First().Select(c => GetBinary(c)));
            return DecodePacket(binString).Packet.VersionSum;
        }



        public override long Part2(List<string> input)
        {
            return 0;
        }
        private (Packet Packet, int Offset) DecodePacket(string packet)
        {
            List<Packet> res = new();
            var version = (int)Utils.ConvertFromBinaryString(packet[..3].PadLeft(4,'0'));            
            var type = (int)Utils.ConvertFromBinaryString(packet[3..6].PadLeft(4,'0'));     
            
            var i = 6;
            if(type == 4) // Packnet contains number
            {
                string current;
                List<string> nums = new();
                do
                {
                    current = packet.Substring(i, 5);
                    nums.Add(current[1..]);
                    i+=5;
                } while (current[0] != '0');
                var num = Utils.ConvertFromBinaryString(string.Join("",nums)).ToString();
                return (new Packet(version, type, num, new()), i);
            }
            else // Packet contains operator
            {
                var lengthTypeId = packet[i++];
                if(lengthTypeId == '0') // The next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
                {
                    var length = (int)Utils.ConvertFromBinaryString(packet.Substring(i, 15));
                    i+=15;
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
                    var packets = Utils.ConvertFromBinaryString(packet.Substring(i, 11));
                    i+=11;
                    for (int j = 0; j < packets; j++)
                    {
                        var (p, ii) = DecodePacket(packet[i..]);
                        res.Add(p);
                        i+=ii;
                    }
                }
            }

            return (new Packet(version, type, "X", res), i);
        }
        private record Packet(int Version, int IType, string Value, List<Packet> subExpr)
        {
            public long VersionSum => Version + subExpr.Sum(s => s.VersionSum);
        }
        private string GetBinary(char c)
        {
            switch (c)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'A':
                    return "1010";
                case 'B':
                    return "1011";
                case 'C':
                    return "1100";  
                case 'D':
                    return "1101";
                case 'E':
                    return "1110";
                case 'F':
                    return "1111";
                default:
                    return null;
            }
        }
    }
}
