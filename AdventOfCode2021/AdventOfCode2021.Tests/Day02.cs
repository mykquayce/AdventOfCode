using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day02
{
	[Theory]
	[InlineData("forward 5", Directions.Forward, 5)]
	[InlineData("down 5", Directions.Down, 5)]
	[InlineData("forward 8", Directions.Forward, 8)]
	[InlineData("up 3", Directions.Up, 3)]
	[InlineData("down 8", Directions.Down, 8)]
	[InlineData("forward 2", Directions.Forward, 2)]
	public void ParseInputTests(string s, Directions expectedDirection, short expectedMagnitude)
	{
		var expected = new Vector(expectedDirection, expectedMagnitude);
		var actual = Vector.Parse(s);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(150, "forward 5", "down 5", "forward 8", "up 3", "down 8", "forward 2")]
	public void Test1(int expected, params string[] vectorsStrings)
	{
		int depth = 0, position = 0;
		var vectors = vectorsStrings.Select(Vector.Parse);

		foreach (var (direction, magnitude) in vectors)
		{
			position += direction switch
			{
				Directions.Forward => magnitude,
				_ => 0,
			};

			depth += direction switch
			{
				Directions.Down => magnitude,
				Directions.Up => -magnitude,
				_ => 0,
			};
		}

		var actual = position * depth;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day02.txt", 1_499_229)]
	public async Task SolvePart1(string fileName, int expected)
	{
		int depth = 0, position = 0;
		var vectors = fileName.ReadAndParseLinesAsync<Vector>();

		await foreach (var (direction, magnitude) in vectors)
		{
			position += direction switch
			{
				Directions.Forward => magnitude,
				_ => 0,
			};

			depth += direction switch
			{
				Directions.Down => magnitude,
				Directions.Up => -magnitude,
				_ => 0,
			};
		}

		var actual = position * depth;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(900, "forward 5", "down 5", "forward 8", "up 3", "down 8", "forward 2")]
	public void Test2(int expected, params string[] vectorsStrings)
	{
		int aim = 0, depth = 0, position = 0;
		var vectors = vectorsStrings.Select(Vector.Parse);

		foreach (var (direction, magnitude) in vectors)
		{
			aim += direction switch
			{
				Directions.Down => magnitude,
				Directions.Up => -magnitude,
				_ => 0,
			};

			position += direction switch
			{
				Directions.Forward => magnitude,
				_ => 0,
			};

			depth += direction switch
			{
				Directions.Forward => aim * magnitude,
				_ => 0,
			};
		}

		var actual = position * depth;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day02.txt", 1_340_836_560)]
	public async Task SolvePart2(string fileName, int expected)
	{
		int aim = 0, depth = 0, position = 0;
		var vectors = fileName.ReadAndParseLinesAsync<Vector>();

		await foreach (var (direction, magnitude) in vectors)
		{
			aim += direction switch
			{
				Directions.Down => magnitude,
				Directions.Up => -magnitude,
				_ => 0,
			};

			position += direction switch
			{
				Directions.Forward => magnitude,
				_ => 0,
			};

			depth += direction switch
			{
				Directions.Forward => aim * magnitude,
				_ => 0,
			};
		}

		var actual = position * depth;
		Assert.Equal(expected, actual);
	}
}

public record Vector(Directions Direction, short Magnitude)
	: IParseable<Vector>
{
	#region iparseable
	public static Vector Parse(string s) => Parse(s, CultureInfo.InvariantCulture);

	public static Vector Parse(string s, IFormatProvider? provider)
	{
		var array = s.Split(' ');
		var direction = Enum.Parse<Directions>(array[0], ignoreCase: true);
		var amount = short.Parse(array[1]);
		return new(direction, amount);
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Vector result)
	{
		var array = s!.Split(' ');
		if (Enum.TryParse<Directions>(array[0], ignoreCase: true, out var direstion)
			&& short.TryParse(array[1], NumberStyles.None, provider, out var magnitude))
		{
			result = new(direstion, magnitude);
			return true;
		}

		result = new(default, default);
		return false;
	}
	#endregion iparseable
}

[Flags]
public enum Directions : byte
{
	Down = 1,
	Forward = 2,
	Up = 4,
}
