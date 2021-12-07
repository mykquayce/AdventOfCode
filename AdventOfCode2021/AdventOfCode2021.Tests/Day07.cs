using Xunit;

namespace AdventOfCode2021.Tests;

public class Day07
{
	[Theory]
	[InlineData(new[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14, }, 2)]
	[InlineData(new[] { 16, 1, 2, 0, 4, 7, 1, 2, 14, }, 2)]
	public void MedianTests(int[] inputs, int expected)
	{
		var actual = inputs.Median();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14, }, new[] { 2, 2, })]
	[InlineData(new[] { 16, 1, 2, 0, 4, 7, 1, 2, 14, }, new[] { 2, })]
	public void MiddleValuesTests(int[] inputs, int[] expected)
	{
		var actual = inputs.MiddleValues().ToList();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("16,1,2,0,4,2,7,1,2,14", 2, 37)]
	public void Test1(string input, int expectedMedian, int expectedFuel)
	{
		var positions = input.Split(',').Select(int.Parse).ToList();
		var median = positions.Median();
		Assert.Equal(expectedMedian, median);
		var differences = positions.Select(value => Math.Abs(value - median)).ToList();
		var sum = differences.Sum();
		Assert.Equal(expectedFuel, sum);
	}

	[Theory]
	[InlineData("Day07.txt", 344_138)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var input = await fileName.ReadFileAsync();
		var positions = input.Split(',').Select(int.Parse).ToList();
		var median = positions.Median();
		var differences = positions.Select(value => Math.Abs(value - median)).ToList();
		var sum = differences.Sum();
		Assert.Equal(expected, sum);
	}

	[Theory]
	[InlineData("16,1,2,0,4,2,7,1,2,14", 2, 206)]
	[InlineData("16,1,2,0,4,2,7,1,2,14", 5, 168)]
	public void Test2(string input, int finish, int fuel)
	{
		var positions = input.Split(',').Select(int.Parse).ToList();
		var differences = positions.Select(value => Math.Abs(value - finish).Triangular()).ToList();
		var sum = differences.Sum();
		Assert.Equal(fuel, sum);
	}

	[Theory]
	[InlineData(1, 1)]
	[InlineData(2, 3)]
	[InlineData(3, 6)]
	[InlineData(4, 10)]
	public void Test3(int distance, int expected)
	{
		var actual = distance.Triangular();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day07.txt", 94_862_124)]//too high
	public async void SolvePart2(string fileName, int expected)
	{
		var contents = await fileName.ReadFileAsync();
		var positions = contents.Split(',').Select(ushort.Parse).ToList();
		int min = positions.Min(), max = positions.Max();
		var actual = int.MaxValue;

		for (var destination = min; destination <= max; destination++)
		{
			var differences = positions.Select(value => Math.Abs(value - destination).Triangular()).ToList();
			var fuel = differences.Sum();
			if (actual > fuel) actual = fuel;
		}
		Assert.Equal(expected, actual);
	}
}
