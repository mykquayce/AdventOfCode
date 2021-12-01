using AdventOfCode2021.Tests.Models;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day01
{
	[Theory]
	[InlineData(7, 199, 200, 208, 210, 200, 207, 240, 269, 260, 263)]
	[InlineData(5, 607, 618, 618, 617, 647, 716, 769, 792)]
	public void Test1(int expected, params int[] depths)
	{
		var count = depths.Increasings().Count(c => c == ChangeTypes.Increased);

		Assert.Equal(expected, count);
	}

	[Theory]
	[InlineData("day01.txt", 1_521)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var depths = await fileName.ReadAndParseLinesAsync<int>().ToListAsync();
		var count = depths.Increasings().Count(c => c == ChangeTypes.Increased);
		Assert.Equal(expected, count);
	}

	[Theory]
	[InlineData(
		new[] { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263, },
		new[] { 607, 618, 618, 617, 647, 716, 769, 792, })]
	public void Test2(int[] depths, int[] expected)
	{
		var windows = depths.ToWindows(size: 3);
		var actual = windows.Select(window => window.Sum());
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("day01.txt", 1_543)]
	public async Task SolvePart2(string fileName, int expected)
	{
		var depths = fileName.ReadAndParseLinesAsync<int>();
		var windows = depths.ToWindowsAsync(size: 3);
		var sums = windows.Select(window => window.Sum());
		var count = await sums.IncreasingsAsync().CountAsync(c => c == ChangeTypes.Increased);
		Assert.Equal(expected, count);
	}
}
