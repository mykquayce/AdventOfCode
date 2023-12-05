using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode2023.Tests;

public class Day05
{
	[Theory]
	[InlineData("50 98 2")]
	[InlineData("52 50 48")]
	public void ParseMapTests(string input)
	{
		var map = Map<byte>.Parse(input, null);

		Assert.NotEmpty(map);
		Assert.Equal(map.Keys.Count(), map.Values.Count());
		Assert.DoesNotContain(default, map.Keys);
		Assert.DoesNotContain(default, map.Values);
		Assert.Equal(map.Keys.Count(), map.Keys.Distinct().Count());
		Assert.Equal(map.Values.Count(), map.Values.Distinct().Count());
	}

	[Theory]
	[InlineData(@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
", new byte[4] { 82, 43, 86, 35, })]
	public void SolveExmaple1(string input, byte[] expected)
	{
		var o = Input<byte>.Parse(input, null);
		var actual = o.DoConversion().ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(
		new byte[4] { 79, 14, 55, 13, },
		new byte[27] { 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, })]
	public void SeedsParseTests(byte[] input, byte[] expected)
	{
		var query = from kvp in input.Chunk(size: 2)
					let start = kvp[0]
					let count = kvp[1]
					let range = new Range<byte>(start, count)
					select range;

		var ranges = query.ToArray();

		Assert.All(expected, i => ranges.Any(r => r.Contains(i)));
	}

	[Theory, InlineData(51_752_125)]
	public void SolvePart1(long expected)
	{
		var path = Path.Combine(".", "Data", "day05.txt");
		var text = File.ReadAllText(path);
		var input = Input<long>.Parse(text, null);
		var values = input.DoConversion();
		var actual = values.Min();
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(10, 10, new int[10] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, })]
	public void NumbersTests(int start, int count, int[] expected)
	{
		var actual = Numbers(start, count).ToArray();
		Assert.Equal(expected, actual);
	}

	private static IEnumerable<T> Numbers<T>(T start, T count) where T : INumber<T>
	{
		for (var a = T.Zero; a < count; a++) { yield return start + a; }
	}

	[Theory]
	[InlineData(10, 10, 5, false)]
	[InlineData(10, 10, 9, false)]
	[InlineData(10, 10, 10, true)]
	[InlineData(10, 10, 19, true)]
	[InlineData(10, 10, 20, false)]
	public void TryGetTests(int start, int count, int value, bool expected)
	{
		var map = new Map<int>(int.MinValue, start, count);
		var actual = map.TryGetValue(value, out _);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
", 46)]
	public void SolveExmaple1InReverse(string input, byte expected)
	{
		var actual = byte.MaxValue;
		var o = InputRevised<byte>.Parse(input, null);

		for (byte a = 0; a < byte.MaxValue; a++)
		{
			var seed = o.DoConversionBackwards(a);
			var found = o.SeedRanges.Any(r => r.Contains(seed));
			if (found)
			{
				actual = a;
				break;
			}
		}

		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(12_634_633)] // wrong (too high)
	public void SolvePart2(ulong expected)
	{
		var actual = ulong.MaxValue;
		var path = Path.Combine(".", "Data", "day05.txt");
		var text = File.ReadAllText(path);
		var o = InputRevised<ulong>.Parse(text, null);

		for (var a = 0UL; a < ulong.MaxValue; a++)
		{
			var seed = o.DoConversionBackwards(a);
			var found = o.SeedRanges.Any(r => r.Contains(seed));
			if (found)
			{
				actual = a;
				break;
			}
		}

		Assert.Equal(expected, actual);
	}

	private readonly record struct Input<T>(IReadOnlyCollection<T> Seeds, IReadOnlyCollection<IReadOnlyCollection<Map<T>>> Mappings)
		: IParsable<Input<T>>
		where T : INumber<T>
	{
		public IEnumerable<T> DoConversion()
		{
			var values = new List<T>(Seeds);

			for (var a = 0; a < values.Count; a++)
			{
				foreach (var mapping in Mappings)
				{
					foreach (var map in mapping)
					{
						if (map.TryGetValue(values[a], out var @new))
						{
							values[a] = @new;
							break;
						}
					}
				}

				yield return values[a];
			}
		}

		public T DoConversionBackwards(T result)
		{
			foreach (var mapping in Mappings.Reverse())
			{
				foreach (var map in mapping)
				{
					if (map.DestinationStart <= result && result <= (map.DestinationStart + map.Count))
					{
						result = map.SourceStart + (result - map.DestinationStart);
						break;
					}
				}
			}
			return result;
		}

		public static Input<T> Parse(string s, IFormatProvider? provider)
		{
			var sections = s.Split(Environment.NewLine + Environment.NewLine);

			var seeds = sections[0][7..]
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => T.Parse(s, null))
				.ToArray();

			var mappings = new List<IReadOnlyCollection<Map<T>>>();

			foreach (var section in sections.Skip(1))
			{
				var lines = section.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

				var maps = lines.Skip(1)
					.Select(s => Map<T>.Parse(s, null))
					.ToArray();

				mappings.Add(maps);
			}

			return new(seeds, mappings);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Input<T> result)
		{
			throw new NotImplementedException();
		}
	}

	private readonly record struct InputRevised<T>(IReadOnlyCollection<Range<T>> SeedRanges, IReadOnlyCollection<IReadOnlyCollection<Map<T>>> Mappings)
		: IParsable<InputRevised<T>>
		where T : INumber<T>
	{
		public T DoConversionBackwards(T result)
		{
			foreach (var mapping in Mappings.Reverse())
			{
				foreach (var map in mapping)
				{
					if (map.DestinationStart <= result && result <= (map.DestinationStart + map.Count))
					{
						result = map.SourceStart + (result - map.DestinationStart);
						break;
					}
				}
			}
			return result;
		}

		public static InputRevised<T> Parse(string s, IFormatProvider? provider)
		{
			var sections = s.Split(Environment.NewLine + Environment.NewLine);

			var seeds = sections[0][7..]
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => T.Parse(s, null))
				.Chunk(size: 2)
				.Select(a => new Range<T>(Start: a[0], Count: a[1]))
				.ToArray();

			var mappings = new List<IReadOnlyCollection<Map<T>>>();

			foreach (var section in sections.Skip(1))
			{
				var lines = section.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

				var maps = lines.Skip(1)
					.Select(s => Map<T>.Parse(s, null))
					.ToArray();

				mappings.Add(maps);
			}

			return new(seeds, mappings);


			throw new NotImplementedException();
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out InputRevised<T> result)
		{
			throw new NotImplementedException();
		}
	}

	private readonly record struct Map<T>(T DestinationStart, T SourceStart, T Count)
		: IParsable<Map<T>>, IReadOnlyDictionary<T, T>
		where T : INumber<T>
	{
		#region ireadonlydictionary implementation
		public T this[T key] => TryGetValue(key, out var value) ? key : throw new KeyNotFoundException();
		public IEnumerable<T> Keys => Numbers(SourceStart, Count);
		public IEnumerable<T> Values => Numbers(DestinationStart, Count);
		int IReadOnlyCollection<KeyValuePair<T, T>>.Count => int.CreateChecked(Count);
		public bool ContainsKey(T key) => TryGetValue(key, out _);
		public IEnumerator<KeyValuePair<T, T>> GetEnumerator()
		{
			T source = SourceStart, destination = DestinationStart;
			return Numbers(T.Zero, Count).Select(i => new KeyValuePair<T, T>(source + i, destination + i)).GetEnumerator();
		}

		public bool TryGetValue(T key, [MaybeNullWhen(false)] out T value)
		{
			var ok = key >= SourceStart && key < (SourceStart + Count);
			value = ok ? (key - SourceStart) + DestinationStart : default;
			return ok;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion ireadonlydictionary implementation

		#region iparsable implementation
		public static Map<T> Parse(string s, IFormatProvider? provider)
		{
			var values = s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => T.Parse(s, null))
				.ToArray();

			return new(values[0], values[1], values[2]);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Map<T> result)
		{
			throw new NotImplementedException();
		}
		#endregion iparsable implementation
	}

	public readonly record struct Range<T>(T Start, T Count)
		: IParsable<Range<T>>
		where T : INumber<T>
	{
		public bool Contains(T value) => Start <= value && value < (Start + Count);

		public static Range<T> Parse(string s, IFormatProvider? provider)
		{
			var (start, count) = s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(s => T.Parse(s, null));
			return new(start, count);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Range<T> result)
		{
			throw new NotImplementedException();
		}
	}
}

public static class Extensions
{
	public static void Deconstruct<T>(this IEnumerable<T> values, out T first, out T second)
	{
		using var enumerator = values.GetEnumerator();
		first = get();
		second = get();

		T get() => enumerator.MoveNext() ? enumerator.Current : throw new Exception();
	}
}
