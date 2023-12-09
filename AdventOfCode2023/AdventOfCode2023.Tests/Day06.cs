using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode2023.Tests;

public class Day06
{
	[Theory]
	[InlineData(7, new[] { 0, 6, 10, 12, 12, 10, 6, 0, })]
	[InlineData(15, new[] { 0, 14, 26, 36, 44, 50, 54, 56, 56, 54, 50, 44, 36, 26, 14, 0, })]
	[InlineData(30, new[] { 0, 29, 56, 81, 104, 125, 144, 161, 176, 189, 200, 209, 216, 221, 224, 225, 224, 221, 216, 209, 200, 189, 176, 161, 144, 125, 104, 81, 56, 29, 0, })]
	public void GetTestsTests(int time, int[] expected)
	{
		var actual = GetAllTimes(time).ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
Time:      7  15   30
Distance:  9  40  200
""", new byte[3] { 7, 15, 30, }, new byte[3] { 9, 40, 200, })]
	public void ParseInputTests(string input, byte[] expectedTimes, byte[] expectedDistances)
	{
		var o = Input<byte>.Parse(input, null);

		Assert.NotEqual(default, o);
		Assert.Equal(expectedDistances, o.Distances);
		Assert.Equal(expectedTimes, o.Times);
	}

	[Theory]
	[InlineData("""
Time:      7  15   30
Distance:  9  40  200
""", new int[3] { 4, 8, 9, })]
	public void SolveExample1(string input, int[] expected)
	{
		var o = Input<byte>.Parse(input, null);
		var actual = o.GetSolutionsCounts().ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(1_660_968)]
	public void SolvePart1(int expected)
	{
		var input = File.ReadAllText(path: Path.Combine(".", "Data", "day06.txt"));
		var o = Input<short>.Parse(input, null);
		var actual = o.GetSolutionsCounts().Aggregate(seed: 1, (i, a) => i * a);
		Assert.Equal(expected, actual);
	}

	private static IEnumerable<T> GetAllTimes<T>(T total)
		where T : INumber<T>
	{
		for (var a = T.Zero; a < total + T.One; a++)
		{
			yield return a * (total - a);
		}
	}

	private readonly record struct Input<T>(T[] Times, T[] Distances)
		: IParsable<Input<T>>
		where T : INumber<T>
	{
		public IEnumerable<int> GetSolutionsCounts()
		{
			for (var a = 0; a < Distances.Length; a++)
			{
				var time = Times[a];
				var distance = Distances[a];

				yield return GetAllTimes(time).Count(t => t > distance);
			}
		}

		public static Input<T> Parse(string s, IFormatProvider? provider)
		{
			var (times, distances) = from line in s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
									 let numbers = from @string in line[11..].Split(' ', StringSplitOptions.RemoveEmptyEntries)
												   let number = T.Parse(@string, null)
												   select number
									 select numbers.ToArray();

			return new(times, distances);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Input<T> result)
		{
			throw new NotImplementedException();
		}
	}
}
