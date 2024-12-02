using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Advent.y2020
{
    [AoC(2020)]
    public class Day04() : Day(4, 2020)
    {
        public override object Part1(List<string> input)
        {
            var passports = new List<List<Tuple<string, string>>>();
            var passport = new List<Tuple<string, string>>();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    passports.Add(passport);
                    passport = new List<Tuple<string, string>>();
                }
                else
                {
                    var fields = line.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    passport.AddRange(fields.Select(f => f.Split(':')).Select(g => Tuple.Create(g[0], g[1])).ToList());
                }
            }
            // Last one 
            passports.Add(passport);

            var expectedFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            var optionalFields = new[] { "cid" };

            return Day4_1(passports, expectedFields, optionalFields);
        }
        public override object Part2(List<string> input)
        {
            var passports = new List<List<Tuple<string, string>>>();
            var passport = new List<Tuple<string, string>>();
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    passports.Add(passport);
                    passport = new List<Tuple<string, string>>();
                }
                else
                {
                    var fields = line.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    passport.AddRange(fields.Select(f => f.Split(':')).Select(g => Tuple.Create(g[0], g[1])).ToList());
                }
            }
            // Last one 
            passports.Add(passport);

            var expectedFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            var optionalFields = new[] { "cid" };

            return Day4_2(passports, expectedFields);
        }

        static long Day4_1(List<List<Tuple<string, string>>> passports, IEnumerable<string> required, IEnumerable<string> optional)
        {
            var valid = 0;
            passports.ForEach(passport =>
            {
                if (ArePassportFieldsValid(required, passport))
                {
                    valid++;
                }
            });
            return valid;
        }

        private static bool ArePassportFieldsValid(IEnumerable<string> required, IEnumerable<Tuple<string, string>> passport)
        {
            var keys = passport.Select(p => p.Item1);
            return required.All(r => keys.Contains(r));
        }

        static long Day4_2(List<List<Tuple<string, string>>> passports, IEnumerable<string> required)
        {
            return passports.Where(p => ArePassportFieldsValid(required, p)).Count(IsPassportDataValid);
        }

        private static bool IsPassportDataValid(List<Tuple<string, string>> passport)
        {
            return passport.All(IsFieldValid);
        }

        private static bool IsFieldValid(Tuple<string, string> field)
        {
            var validEyeColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            switch (field.Item1)
            {
                case "cid":
                    return true;
                case "byr":
                    return int.TryParse(field.Item2, out var byr) && byr >= 1920 && byr <= 2020;
                case "iyr":
                    return int.TryParse(field.Item2, out var iyr) && iyr >= 2010 && iyr <= 2020;
                case "eyr":
                    return int.TryParse(field.Item2, out var eyr) && eyr >= 2020 && eyr <= 2030;
                case "hgt":
                    if (field.Item2.Contains("cm") && field.Item2.Contains("in"))
                        return false;
                    else if (field.Item2.EndsWith("cm")
                            && (int.TryParse(field.Item2.Substring(0, field.Item2.Length - 2), out var hgtCm)
                                && hgtCm >= 150
                                && hgtCm <= 193))
                        return true;
                    else if (field.Item2.EndsWith("in")
                            && (int.TryParse(field.Item2.Substring(0, field.Item2.Length - 2), out var hgtIn)
                                && hgtIn >= 59
                                && hgtIn <= 76))
                        return true;
                    break;
                case "hcl":
                    if (field.Item2.Length != 7 && field.Item2.StartsWith("#") == false)
                        return false;
                    else
                        return int.TryParse(field.Item2.Substring(1, 6), System.Globalization.NumberStyles.HexNumber, null, out var hcl);
                case "ecl":
                    return validEyeColors.Contains(field.Item2);
                case "pid":
                    if (field.Item2.Count() != 9)
                        return false;
                    else
                        return int.TryParse(field.Item2, out var pid) && pid > 0 && pid <= 999999999;
            }
            return false;
        }
    }
}
