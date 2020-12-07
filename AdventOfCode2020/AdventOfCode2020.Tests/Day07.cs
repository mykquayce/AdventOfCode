using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day07
	{
		private const string _exampleRulesText = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.
";

		[Theory]
		[InlineData("shiny gold", "bright white", "dark orange", "light red", "muted yellow")]
		public void Example1(string start, params string[] expected)
		{
			var rules = Rules.Parse(_exampleRulesText);

			var parents = rules.GetAllParents(start).Dedupe().OrderBy(o => o).ToArray();

			Assert.Equal(expected, parents);
		}

		[Theory]
		[InlineData(_exampleRulesText)]
		public void ParseRulesTest(string text)
		{
			var rules = Rules.Parse(text);

			Assert.NotEmpty(rules);
			Assert.Equal(9, rules.Count);

			foreach (var (parent, children) in rules)
			{
				Assert.NotNull(parent);
				Assert.NotEmpty(parent);
				Assert.Contains(" ", parent);
				Assert.Matches(@"^\w+ \w+$", parent);

				foreach (var child in children)
				{
					Assert.NotNull(child);
					Assert.NotEmpty(child);
					Assert.Contains(" ", child);
					Assert.Matches(@"^\w+ \w+$", child);
				}
			}
		}

		[Theory]
		[InlineData(
			"light red bags contain 1 bright white bag, 2 muted yellow bags.",
			"light red", "bright white", "muted yellow", "muted yellow")]
		[InlineData(
			"dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
			"dark orange", "bright white", "bright white", "bright white", "muted yellow", "muted yellow", "muted yellow", "muted yellow")]
		[InlineData(
			"bright white bags contain 1 shiny gold bag.",
			"bright white", "shiny gold")]
		[InlineData(
			"muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
			"muted yellow", "shiny gold", "shiny gold", "faded blue", "faded blue", "faded blue", "faded blue", "faded blue", "faded blue", "faded blue", "faded blue", "faded blue")]
		[InlineData(
			"shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
			"shiny gold", "dark olive", "vibrant plum", "vibrant plum")]
		[InlineData(
			"dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
			"dark olive", "faded blue", "faded blue", "faded blue", "dotted black", "dotted black", "dotted black", "dotted black")]
		[InlineData(
			"vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
			"vibrant plum", "faded blue", "faded blue", "faded blue", "faded blue", "faded blue", "dotted black", "dotted black", "dotted black", "dotted black", "dotted black", "dotted black")]
		[InlineData(
			"faded blue bags contain no other bags.",
			"faded blue")]
		[InlineData(
			"dotted black bags contain no other bags.",
			"dotted black")]
		public void ParseRuleTest(string before, string expectedParent, params string[] expectedChildren)
		{
			var (parent, children) = Rules.ParseRule(before);
			Assert.Equal(expectedParent, parent);
			Assert.Equal(expectedChildren, children);
		}

		[Theory]
		[InlineData("day07.txt", "shiny gold", 268)]
		public async Task Part1(string filename, string start, int expected)
		{
			var asyncLines = filename.ReadLinesAsync();
			var rules = await Rules.ParseAsync(asyncLines);

			Assert.NotNull(rules);
			Assert.NotEmpty(rules);
			Assert.All(rules.Keys, Assert.NotNull);
			Assert.All(rules.Keys, Assert.NotEmpty);

			var count = rules.GetAllParents(start).Dedupe().Count();

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData(_exampleRulesText, "shiny gold", 32)]
		[InlineData(@"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.", "shiny gold", 126)]
		public void Example2(string text, string start, int expected)
		{
			var rules = Rules.Parse(text);

			var count = rules.GetAllChildren(start).Count();

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("day07.txt", "shiny gold", 7_867)]
		public async Task Part2(string filename, string start, int expected)
		{
			var asyncLines = filename.ReadLinesAsync();
			var rules = await Rules.ParseAsync(asyncLines);

			Assert.NotNull(rules);
			Assert.NotEmpty(rules);
			Assert.All(rules.Keys, Assert.NotNull);
			Assert.All(rules.Keys, Assert.NotEmpty);

			var count = rules.GetAllChildren(start).Count();

			Assert.Equal(expected, count);
		}
	}

	public class Rules : IReadOnlyDictionary<string, ICollection<string>>
	{
		private const RegexOptions _regexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline;
		private readonly static Regex _ruleRegex = new(@"^(\w+ \w+) bags contain (.+)\.$", _regexOptions);
		private readonly static Regex _childrenRegex = new(@"(\d+) (\w+ \w+) bags?", _regexOptions);

		private readonly IDictionary<string, ICollection<string>> _dictionary;

		public Rules(IDictionary<string, ICollection<string>> dictionary)
		{
			_dictionary = dictionary;
		}

		#region IReadOnlyDictionary implementation
		public ICollection<string> this[string key] => _dictionary[key];
		public IEnumerable<string> Keys => _dictionary.Keys;
		public IEnumerable<ICollection<string>> Values => _dictionary.Values;
		public int Count => _dictionary.Count;
		public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
		public IEnumerator<KeyValuePair<string, ICollection<string>>> GetEnumerator() => _dictionary.GetEnumerator();
		public bool TryGetValue(string key, [MaybeNullWhen(false)] out ICollection<string> value) => _dictionary.TryGetValue(key, out value);
		IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
		#endregion IReadOnlyDictionary implementation

		public static Rules Parse(string text) => Parse(text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));

		public static Rules Parse(IEnumerable<string> lines)
		{
			var dictionary = lines
				.Select(ParseRule)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			return new Rules(dictionary);
		}

		public async static Task<Rules> ParseAsync(IAsyncEnumerable<string> lines)
		{
			var dictionary = await lines
				.Select(ParseRule)
				.ToDictionaryAsync(kvp => kvp.Key, kvp => kvp.Value);

			return new Rules(dictionary);
		}

		public static KeyValuePair<string, ICollection<string>> ParseRule(string s)
		{
			var ruleMatch = _ruleRegex.Match(s);

			var parent = ruleMatch.Groups[1].Value;
			var childrenString = ruleMatch.Groups[2].Value;

			var children = ParseChildren(childrenString).ToList();

			return new KeyValuePair<string, ICollection<string>>(parent, children);
		}

		private static IEnumerable<string> ParseChildren(string s)
		{
			var childrenMatches = _childrenRegex.Matches(s);

			foreach (Match childMatch in childrenMatches)
			{
				var count = int.Parse(childMatch.Groups[1].Value);
				var child = childMatch.Groups[2].Value;

				for (var a = 0; a < count; a++)
				{
					yield return child;
				}
			}
		}

		public IEnumerable<string> GetAllParents(string start)
		{
			foreach (var (parent, children) in _dictionary)
			{
				if (children.Contains(start))
				{
					yield return parent;

					foreach (var grandparent in GetAllParents(parent))
					{
						yield return grandparent;
					}
				}
			}
		}

		public IEnumerable<string> GetAllChildren(string start)
		{
			var children = _dictionary[start];

			foreach (var child in children)
			{
				yield return child;

				foreach (var grandChild in GetAllChildren(child))
				{
					yield return grandChild;
				}
			}
		}
	}
}
