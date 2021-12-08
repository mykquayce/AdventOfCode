using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day08
{
	[Theory]
	[InlineData("be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe", 2)]
	[InlineData("edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc", 3)]
	[InlineData("fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg", 3)]
	[InlineData("fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb", 1)]
	[InlineData("aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea", 3)]
	[InlineData("fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb", 4)]
	[InlineData("dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe", 3)]
	[InlineData("bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef", 1)]
	[InlineData("egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb", 4)]
	[InlineData("gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce", 2)]
	public void Find1478Tests(string input, int expected)
	{
		var lengths = new[] { 2, 3, 4, 7, };
		var (_, output) = SignalData.Parse(input, default);
		var actual = output.Select(s => s.Length).Count(lengths.Contains);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day08.txt", 344)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var actual = 0;
		var lengths = new[] { 2, 3, 4, 7, };
		var data = fileName.ReadAndParseLinesAsync<SignalData>();
		await foreach (var (_, output) in data)
		{
			actual += output.Select(s => s.Length).Count(lengths.Contains);
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(
		"acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab",
		"abcdeg", "ab", "acdfg", "abcdf", "abef", "bcdef", "bcdefg", "abd", "abcdefg", "abcdef")]
	public void Test1(string input, params string[] expected)
	{
		var samples = SignalData.SamplesData.Parse(input, default);

		Assert.Equal(expected[0], samples[0]);
		Assert.Equal(expected[1], samples[1]);
		Assert.Equal(expected[2], samples[2]);
		Assert.Equal(expected[3], samples[3]);
		Assert.Equal(expected[4], samples[4]);
		Assert.Equal(expected[5], samples[5]);
		Assert.Equal(expected[6], samples[6]);
		Assert.Equal(expected[7], samples[7]);
		Assert.Equal(expected[8], samples[8]);
		Assert.Equal(expected[9], samples[9]);
	}

	[Theory]
	[InlineData("acdfg", "ab", false)]
	[InlineData("abcdf", "ab", true)]
	[InlineData("bcdef", "ab", false)]
	[InlineData("abef", "abcdef", true)]
	public void OverlapTests(string left, string right, bool expected)
	{
		bool actual = left.Overlaps(right);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf", 5_353)]
	[InlineData("be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe", 8_394)]
	[InlineData("edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc", 9_781)]
	[InlineData("fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg", 1_197)]
	[InlineData("fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb", 9_361)]
	[InlineData("aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea", 4_873)]
	[InlineData("fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb", 8_418)]
	[InlineData("dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe", 4_548)]
	[InlineData("bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef", 1_625)]
	[InlineData("egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb", 8_717)]
	[InlineData("gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce", 4_315)]
	public void Test2(string input, int expected)
	{
		var (samples, output) = SignalData.Parse(input, default);

		IReadOnlyDictionary<string, int> dictionary = samples.ReverseArray().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		var chars = output.Select(s => dictionary[s].ToString()[0]);
		var @string = string.Concat(chars);
		var actual = int.Parse(@string);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day08.txt", 1_048_410)]
	public async Task SolvePart2(string fileName, int expected)
	{
		var actual = 0;
		var data = fileName.ReadAndParseLinesAsync<SignalData>();
		await foreach (var (samples, output) in data)
		{
			IReadOnlyDictionary<string, int> dictionary = samples.ReverseArray().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			var chars = output.Select(s => dictionary[s].ToString()[0]);
			var @string = string.Concat(chars);
			var value = int.Parse(@string);
			actual += value;
		}
		Assert.Equal(expected, actual);
	}
}

public record SignalData(SignalData.SamplesData Samples, IReadOnlyList<string> Output)
	: IParseable<SignalData>
{
	public static SignalData Parse(string s, IFormatProvider? provider)
		=> TryParse(s, provider, out var signalData) ? signalData : throw new Exception();

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out SignalData result)
	{
		var (samplesCsv, outputCsv) = s!.Split('|', StringSplitOptions.RemoveEmptyEntries);
		var samples = SamplesData.Parse(samplesCsv!, default);
		var output = outputCsv!.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SystemExtensions.OrderString).ToList();
		result = new(samples, output);
		return true;
	}

	public record SamplesData(IReadOnlyList<string> Values)
		: IReadOnlyList<string>, IParseable<SamplesData>
	{
		#region ireadonlylist implementation
		public string this[int index] => Values[index];
		public int Count => Values.Count;
		public IEnumerator<string> GetEnumerator() => Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion ireadonlylist implementation

		#region iparseable implementation
		public static SamplesData Parse(string s, IFormatProvider? provider)
			=> TryParse(s, provider, out var samples) ? samples : throw new Exception();

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out SamplesData result)
		{
			var samples = s!.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(SystemExtensions.OrderString)
				.ToList();

			// 1 is the 2-length digit
			var one = samples.Single(s => s.Length == 2);
			// 4 is the 4-length digit
			var four = samples.Single(s => s.Length == 4);
			// 7 is the 3-length digit
			var seven = samples.Single(s => s.Length == 3);
			// 8 is teh 7-length digit
			var eight = samples.Single(s => s.Length == 7);
			// 6 is the 6-length digit that doesn't fully overlap 1
			var six = samples.Where(s => s.Length == 6).Single(s => !s.Overlaps(one));
			// 9 is the 6-length digit that fully overlaps 4
			var nine = samples.Where(s => s.Length == 6).Single(s => s.Overlaps(four));
			// 0 is the remaining 6-length digit
			var zero = samples.Where(s => s.Length == 6).Where(s => s != six).Single(s => s != nine);
			// 3 is the 5-length digit that fully overlaps 1
			var three = samples.Where(s => s.Length == 5).Single(s => s.Overlaps(one));
			// 5 is the 5-length digit that fully overlaps 6
			var five = samples.Where(s => s.Length == 5).Single(s => s.Overlaps(six));
			// 2 is the remaining 5-length digit
			var two = samples.Where(s => s.Length == 5).Where(s => s != three).Single(s => s != five);

			var values = new string[10] { zero, one, two, three, four, five, six, seven, eight, nine, };

			result = new(values);
			return true;
		}
		#endregion iparseable implementation
	}
}
