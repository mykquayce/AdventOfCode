namespace AdventOfCode2022.Tests;

public class Day06 : Base
{
	[Theory]
	[InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
	[InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
	[InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
	[InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
	[InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
	public void SolveExample1(string input, int expected)
	{
		var actual = FindStartOfPacket(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(1_198)]
	public async Task SovlePart1(int expected)
	{
		var input = await base.GetInputAsync().SingleAsync();
		var actual = FindStartOfPacket(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
	[InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
	[InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23)]
	[InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
	[InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
	public void SolveExample2(string input, int expected)
	{
		var actual = FindStartOfMessage(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(3_120)]
	public async Task SovlePart2(int expected)
	{
		var input = await base.GetInputAsync().SingleAsync();
		var actual = FindStartOfMessage(input);
		Assert.Equal(expected, actual);
	}

	private static int FindStartOfPacket(string buffer) => FindMarker(buffer, size: 4) + 4;
	private static int FindStartOfMessage(string buffer) => FindMarker(buffer, size: 14) + 14;

	private static int FindMarker(string buffer, int size)
	{
		for (var a = 0; a < buffer.Length - size; a++)
		{
			var sample = buffer[a..(a + size)];
			if (sample.Distinct().Count() < size) { continue; }
			return a;
		}

		throw new Exception();
	}
}
