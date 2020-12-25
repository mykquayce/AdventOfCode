using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day19
	{
		[Theory]
		[InlineData(@"0: 1 2
1: ""a""
2: 1 3 | 3 1
3: ""b""", 1, "a")]
		[InlineData(@"0: 1 2
1: ""a""
2: 1 3 | 3 1
3: ""b""", 2, "ab", "ba")]
		[InlineData(@"0: 1 2
1: ""a""
2: 1 3 | 3 1
3: ""b""", 0, "aab", "aba")]
		[InlineData(@"0: ""a""
1: 0 0", 0, "a")]
		[InlineData(@"0: ""a""
1: 0 0", 1, "aa")]
		[InlineData(@"0: ""a""
1: ""b""
2: 0 1", 2, "ab")]
		[InlineData(@"0: ""a""
1: ""b""
2: 0 0 | 1 1", 2, "aa", "bb")]
		public void BuildDictionaryTests(string input, byte index, params string[] expected)
		{
			var rules = new SatelliteRules(input);
			var values = rules.GetValues(index);
			Assert.Equal(expected, values);

		}

		[Theory]
		[InlineData(@"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""", new[] { "ababbb", "bababa", "abbbab", "aaabbb", "aaaabbb", }, 2)]
		public void Example1(string input, string[] messages, int expected)
		{
			var rules = new SatelliteRules(input);
			var values = rules.GetValues(0);
			var intersect = messages.Intersect(values);
			Assert.Equal(expected, intersect.Count());
		}

		[Theory]
		[InlineData("day19.txt", 115)]
		public async Task Part1(string filename, int expected)
		{
			var text = await filename.ReadAllTextAsync();
			var halves = text.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var input = halves[0];
			var messages = halves[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var rules = new SatelliteRules(input);
			var values = rules.GetValues(0);
			var intersect = messages.Intersect(values);
			Assert.Equal(expected, intersect.Count());
		}

		[Theory]
		[InlineData("day19.txt", -1)]
		public async Task Part2(string filename, int expected)
		{
			var text = await filename.ReadAllTextAsync();
			var halves = text.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var input = halves[0];
			var messages = halves[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var rules = new SatelliteRules(input)
			{
				[8] = "42 | 42 8",
				[11] = "42 31 | 42 11 31",
			};
		}

		[Theory]
		[InlineData(@"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba", 3, 12)]
		public void Example2(string input, int expectedBefore, int expectedAfter)
		{
			var halves = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var rules = new SatelliteRules(halves[0]);
			var messages = halves[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var values = rules.GetValues().ToList();

			Assert.Equal(expectedBefore, messages.Intersect(values).Count());

			rules[8] = "42 | 42 8";
			rules[11] = "42 31 | 42 11 31";

			values = rules.GetValues().ToList();

			Assert.Equal(expectedAfter, messages.Intersect(values).Count());
		}
	}

	public class SatelliteRules : Dictionary<byte, string>
	{
		public SatelliteRules(string input)
			: base(Parse(input))
		{ }

		public IEnumerable<string> GetValues()
		{
			return Keys.Select(GetValues).SelectMany(s => s);
		}

		private readonly static ConcurrentStack<byte> _stack = new ConcurrentStack<byte>();

		public IEnumerable<string> GetValues(byte index)
		{
			foreach (var or in this[index].Split('|'))
			{
				var ands = or.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				var lists = Enumerable.Range(0, ands.Length)
					.Select(_ => (IList<string>)new List<string>())
					.ToList();

				for (var a = 0; a < ands.Length; a++)
				{
					var and = ands[a];

					if (byte.TryParse(and, out var nextIndex))
					{
						foreach (var value in GetValues(nextIndex)) lists[a].Add(value);
					}
					else
					{
						lists[a].Add(and);
					}
				}

				foreach (var combination in lists.GetArraysCombinations())
				{
					yield return string.Concat(combination);
				}
			}
		}

		public static IEnumerable<KeyValuePair<byte, string>> Parse(string text)
		{
			var lines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var halves = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
				var key = byte.Parse(halves[0]);
				var value = halves[1].Trim().Trim('"');
				yield return new KeyValuePair<byte, string>(key, value);
			}
		}
	}
}
