using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day04
	{
		[Theory]
		[InlineData("ecl:gry", Passport.Fields.ecl, "gry")]
		[InlineData("pid:860033327", Passport.Fields.pid, "860033327")]
		[InlineData("eyr:2020", Passport.Fields.eyr, "2020")]
		[InlineData("hcl:#fffffd", Passport.Fields.hcl, "#fffffd")]
		public void ParsePassportFieldTest(string input, Passport.Fields expectedPassportField, string expectedValue)
		{
			var (passportField, value) = Passport.ParsePassportField(input);

			Assert.Equal(expectedPassportField, passportField);
			Assert.Equal(expectedValue, value);
		}

		[Theory]
		[InlineData(@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm", 8)]
		public void CountPassportFields(string input, int expected)
		{
			var fields = input.Split(new[] { " ", Environment.NewLine, }, count: int.MaxValue, options: StringSplitOptions.None);

			Assert.Equal(expected, fields.Length);
		}

		[Theory]
		[InlineData(true, @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm")]
		[InlineData(false, @"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929")]
		[InlineData(true, @"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm")]
		[InlineData(false, @"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in")]
		public void ParsePassportTests(bool expected, string passportString)
		{
			var passport = Passport.Parse(passportString);

			var actual = passport.IsValid();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in", 4)]
		public void CountPassports(string s, int expected)
		{
			string separator = Environment.NewLine + Environment.NewLine;

			Assert.Equal(
				expected,
				s.Split(separator).Length);
		}

		[Theory]
		[InlineData("day04.txt", 260)]
		public async Task CountPassportFromFile(string filename, int expected)
		{
			var count = 0;
			var passports = filename.ReadGroupsAsync();

			await foreach (var passport in passports)
			{
				count++;
				var fields = passport.Split(new[] { " ", Environment.NewLine }, count: int.MaxValue, options: StringSplitOptions.RemoveEmptyEntries);

				Assert.NotNull(fields);
				Assert.NotEmpty(fields);
				Assert.DoesNotContain(default, fields);
				Assert.InRange(fields.Length, 4, 8);

				Assert.All(fields, s => Assert.Matches(@"[a-z]{3}:[#\w]+", s));
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("day04.txt", 222)]
		public async Task Part1(string filename, int expected)
		{
			var count = 0;
			var passportsStrings = filename.ReadGroupsAsync();

			await foreach (var passportString in passportsStrings)
			{
				var passport = Passport.Parse(passportString);

				if (passport.IsValid()) count++;
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData(@"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926", false)]
		[InlineData(@"iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946", false)]
		[InlineData(@"hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277", false)]
		[InlineData(@"hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007", false)]
		[InlineData(@"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f", true)]
		[InlineData(@"eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm", true)]
		[InlineData(@"hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022", true)]
		[InlineData(@"iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719", true)]
		public void StrictlyValidPassports(string s, bool expected)
		{
			var passport = Passport.Parse(s);

			Assert.Equal(expected, passport.IsStrictlyValid());
		}

		[Theory]
		[InlineData("day04.txt", 140)]
		public async Task Part2(string filename, int expected)
		{
			var count = 0;
			var passportsStrings = filename.ReadGroupsAsync();

			await foreach (var passportString in passportsStrings)
			{
				var passport = Passport.Parse(passportString);

				if (passport.IsStrictlyValid())
				{
					var before = passport[Passport.Fields.hgt];
					var after = passport.HeightCm + " " + passport.HeightIn;
					count++;
				}
			}

			Assert.Equal(expected, count);
		}
	}

	public class Passport : Dictionary<Passport.Fields, string>
	{
		public Passport(IEnumerable<KeyValuePair<Fields, string>> kvps)
			: base(kvps)
		{ }

		public short? BirthYear => this.TryGetValue(Fields.byr, out var s) && Regex.IsMatch(s, "^[0-9]{4}$") ? short.Parse(s) : default(short?);
		//public int? CountryId => this.TryGetValue(PassportFields.cid, out var s) ? int.Parse(s) : default(int?);
		public short? ExpirationYear => this.TryGetValue(Fields.eyr, out var s) && Regex.IsMatch(s, "^[0-9]{4}$") ? short.Parse(s) : default(short?);
		public string? EyeColor => this.TryGetValue(Fields.ecl, out var s) && Regex.IsMatch(s, "^(amb|blu|brn|grn|gry|hzl|oth)$") ? s : default;
		public string? HairColor => this.TryGetValue(Fields.hcl, out var s) && Regex.IsMatch(s, "^#([0-9a-f]){6}$") ? s : default;
		public byte? HeightCm => this.TryGetValue(Fields.hgt, out var s) && Regex.IsMatch(s, "^[0-9]+cm") ? byte.Parse(s.Replace("cm", string.Empty)) : default(byte?);
		public byte? HeightIn => this.TryGetValue(Fields.hgt, out var s) && Regex.IsMatch(s, "^[0-9]+in") ? byte.Parse(s.Replace("in", string.Empty)) : default(byte?);
		public short? IssueYear => this.TryGetValue(Fields.iyr, out var s) && Regex.IsMatch(s, "^[0-9]{4}$") ? short.Parse(s) : default(short?);
		public string? PassportId => this.TryGetValue(Fields.pid, out var s) && Regex.IsMatch(s, "^[0-9]{9}$") ? s : default;

		public static Passport Parse(string s)
		{
			var strings = s.Split(new[] { " ", Environment.NewLine, }, count: int.MaxValue, options: StringSplitOptions.RemoveEmptyEntries);
			var kvps = strings.Select(ParsePassportField);
			var passport = new Passport(kvps);
			return passport;
		}

		public static KeyValuePair<Fields, string> ParsePassportField(string s)
		{
			var kvp = s.Split(':');
			var key = Enum.Parse<Fields>(kvp[0]);
			var value = kvp[1];
			return new KeyValuePair<Fields, string>(key, value);
		}

		public bool IsValid()
		{
			var total = this.Keys
				.Aggregate((product, next) => product |= next);

			return Fields.All == total
				|| (Fields.All & ~Fields.cid) == total;
		}

		public bool IsStrictlyValid()
		{
			var total = this.Keys
				.Aggregate((product, next) => product |= next);

			if (Fields.All != total
				&& (Fields.All & ~Fields.cid) != total)
			{
				return false;
			}

			if (BirthYear!.Value < 1920 || BirthYear!.Value > 2002) return false;
			if (IssueYear!.Value < 2010 || IssueYear!.Value > 2020) return false;
			if (ExpirationYear!.Value < 2020 || ExpirationYear!.Value > 2030) return false;
			if (HeightCm.HasValue && (HeightCm.Value < 150 || HeightCm > 193)) return false;
			if (HeightIn.HasValue && (HeightIn.Value < 59 || HeightIn > 76)) return false;
			if (!HeightCm.HasValue && !HeightIn.HasValue) return false;
			if (string.IsNullOrWhiteSpace(HairColor)) return false;
			if (string.IsNullOrWhiteSpace(EyeColor)) return false;
			if (string.IsNullOrWhiteSpace(PassportId)) return false;

			return true;
		}

		[Flags]
		public enum Fields : byte
		{
			None = 0,
			byr = 1,
			cid = 2,
			ecl = 4,
			eyr = 8,
			hcl = 16,
			hgt = 32,
			iyr = 64,
			pid = 128,
			All = byr | cid | ecl | eyr | hcl | hgt | iyr | pid,
		}
	}
}
