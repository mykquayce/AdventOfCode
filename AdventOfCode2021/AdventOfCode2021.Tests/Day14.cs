using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day14
{
	[Theory]
	[InlineData(@"CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", "NN", 1, new[] { "CN", "NC", })]
	[InlineData(@"CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", "NNCB", 1, new[] { "BC", "CH", "CN", "HB", "NB", "NC", })]
	[InlineData(@"CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", "NNCB", 2, new[] { "BB", "BB", "BC", "BC", "BH", "CB", "CB", "CC", "CN", "HC", "NB", "NB", })]
	[InlineData(@"CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", "NNCB", 3, new[] { "BB", "BB", "BB", "BB", "BC", "BC", "BC", "BH", "BN", "BN", "CC", "CH", "CH", "CN", "CN", "HB", "HB", "HB", "HH", "NB", "NB", "NB", "NB", "NC", })]
	[InlineData(@"CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", "NNCB", 4, new[] { "BB", "BB", "BB", "BB", "BB", "BB", "BB", "BB", "BB", "BC", "BC", "BC", "BC", "BH", "BH", "BH", "BN", "BN", "BN", "BN", "BN", "BN", "CB", "CB", "CB", "CB", "CB", "CC", "CC", "CN", "CN", "CN", "HC", "HC", "HC", "HH", "HN", "NB", "NB", "NB", "NB", "NB", "NB", "NB", "NB", "NB", "NC", "NH", })]
	public void RuleDictionaryTests(string input, string template, int generations, string[] expected)
	{
		var rules = RuleDictionary.Parse(input, default);
		var templateDictionary = rules.GetTemplateDictionary();
		templateDictionary.ApplyTemplate(template);
		templateDictionary.Process(generations);
		var actual = from kvp in templateDictionary
					 let pair = kvp.Key
					 let count = kvp.Value
					 from key in Enumerable.Repeat(pair, (int)count)
					 orderby key
					 select key;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", 10, 1_749L, 161L)]
	[InlineData(@"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C", 40, 219_203_956_9602L, 3_849_876_073L)]
	public void BiggestSmallestTests(string input, int generations, long expectedMostCommon, long expectedLeastCommon)
	{
		var (template, rulesCsv) = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

		var rules = RuleDictionary.Parse(rulesCsv!, default);
		var templateDictionary = rules.GetTemplateDictionary();
		templateDictionary.ApplyTemplate(template!);
		templateDictionary.Process(generations);
		var counts = templateDictionary.GetCounts();
		var mostCommon = counts.Values.OrderByDescending(l => l).First();
		var leastCommon = counts.Values.OrderBy(l => l).First();
		Assert.Equal(expectedMostCommon, mostCommon);
		Assert.Equal(expectedLeastCommon, leastCommon);
	}

	[Theory]
	[InlineData("Day14.txt", 10, 3_411L)]
	[InlineData("Day14.txt", 40, 7_477_815_755_570L)]
	public async Task Solve(string fileName, int generations, long expected)
	{
		var input = await fileName.ReadFileAsync();
		var (template, rulesCsv) = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
		var rules = RuleDictionary.Parse(rulesCsv!, default);
		var templateDictionary = rules.GetTemplateDictionary();
		templateDictionary.ApplyTemplate(template!);
		templateDictionary.Process(generations);
		var counts = templateDictionary.GetCounts();
		var mostCommon = counts.Values.OrderByDescending(l => l).First();
		var leastCommon = counts.Values.OrderBy(l => l).First();
		Assert.Equal(expected, mostCommon - leastCommon);
	}
}

public record RuleDictionary(IReadOnlyDictionary<string, ICollection<string>> Rules)
	: IParseable<RuleDictionary>, IReadOnlyDictionary<string, ICollection<string>>
{
	public IEnumerable<char> Chars => Keys.SelectMany(s => s).Distinct().OrderBy(c => c);

	#region iparseable implementation
	public static RuleDictionary Parse(string s, IFormatProvider? provider)
		=> TryParse(s, provider, out var result) ? result : throw new Exception();

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out RuleDictionary result)
	{
		var dictionary = new Dictionary<string, ICollection<string>>();
		foreach (var (before, after) in from line in s!.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
										let pair = line.Split(" -> ").ToList()
										let before = pair[0]
										let after = SplitRule(pair[0], pair[1][0]).ToList()
										select (before, after))
		{
			dictionary.Add(before, after);
		}
		result = new(dictionary);
		return true;
	}
	#endregion iparseable implementation

	#region ireadonlydictionaryimplementation
	public IEnumerable<string> Keys => Rules.Keys;
	public IEnumerable<ICollection<string>> Values => Rules.Values;
	public int Count => Rules.Count;
	public ICollection<string> this[string key] => Rules[key];
	public bool ContainsKey(string key) => Rules.ContainsKey(key);
	public bool TryGetValue(string key, [MaybeNullWhen(false)] out ICollection<string> value) => Rules.TryGetValue(key, out value);
	public IEnumerator<KeyValuePair<string, ICollection<string>>> GetEnumerator() => Rules.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	#endregion ireadonlydictionaryimplementation

	public static IEnumerable<string> SplitRule(string rule, char middle)
	{
		yield return new(new[] { rule[0], middle, });
		yield return new(new[] { middle, rule[1], });
	}

	public TemplateDictionary GetTemplateDictionary()
	{
		var dictionary = Keys.ToDictionary(key => key, _ => 0L);
		return new(dictionary, this);
	}
}

public class TemplateDictionary : IDictionary<string, long>
{
	private IDictionary<string, long> _dictionary;
	private readonly RuleDictionary _rules;
	private string? _template;

	public TemplateDictionary(IDictionary<string, long> dictionary, RuleDictionary rules)
	{
		_dictionary = dictionary;
		_rules = rules;
	}

	public void ApplyTemplate(string template)
	{
		_template = template;
		foreach (var pair in GetPairs(template))
		{
			this[pair]++;
		}
	}

	public void Process(int generations)
	{
		while (generations-- > 0)
		{
			Process();
		}
	}

	public void Process()
	{
		var result = Keys.ToDictionary(key => key, _ => 0L);

		foreach (var (from, tos) in _rules)
		{
			var count = this[from];
			foreach (var to in tos)
			{
				result[to] += count;
			}
		}

		_dictionary = result;
	}

	public IReadOnlyDictionary<char, long> GetCounts()
	{
		var chars = _rules.Chars.ToDictionary(c => c, _ => 0L);

		foreach (var (pair, count) in this)
		{
			foreach (var @char in pair)
			{
				chars[@char] += count;
			}
		}

		// fix counts : halve all the values
		foreach (var key in chars.Keys) chars[key] /= 2;

		// fix counts : add one to the first and last chars from the initial template
		chars[_template![0]]++;
		chars[_template[^1]]++;

		return chars;
	}

	public static IEnumerable<string> GetPairs(string input)
	{
		for (var a = 0; a < input.Length - 1; a++)
		{
			yield return input[a..(a + 2)];
		}
	}

	#region idictionary implementation
	public long this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }
	public ICollection<string> Keys => _dictionary.Keys;
	public ICollection<long> Values => _dictionary.Values;
	public int Count => _dictionary.Count;
	public bool IsReadOnly => _dictionary.IsReadOnly;
	public void Add(string key, long value) => _dictionary.Add(key, value);
	public void Add(KeyValuePair<string, long> item) => _dictionary.Add(item);
	public void Clear() => _dictionary.Clear();
	public bool Contains(KeyValuePair<string, long> item) => _dictionary.Contains(item);
	public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
	public void CopyTo(KeyValuePair<string, long>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);
	public IEnumerator<KeyValuePair<string, long>> GetEnumerator() => _dictionary.GetEnumerator();
	public bool Remove(string key) => _dictionary.Remove(key);
	public bool Remove(KeyValuePair<string, long> item) => _dictionary.Remove(item);
	public bool TryGetValue(string key, [MaybeNullWhen(false)] out long value) => _dictionary.TryGetValue(key, out value);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	#endregion idictionary implementation
}
