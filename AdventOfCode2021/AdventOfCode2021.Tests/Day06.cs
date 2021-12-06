using Xunit;

namespace AdventOfCode2021.Tests;

public class Day06
{
	[Theory]
	[InlineData(3, 1, 2)]
	[InlineData(3, 2, 1)]
	[InlineData(3, 3, 0)]
	[InlineData(3, 4, 6)]
	[InlineData(3, 5, 5)]
	public void Test1(int value, int days, int expected)
	{
		while (days-- > 0)
		{
			value = (value + 6) % 7;
		}

		Assert.Equal(expected, value);
	}

	[Theory]
	[InlineData(new[] { 3, }, 1, new[] { 2, })]
	[InlineData(new[] { 3, }, 2, new[] { 1, })]
	[InlineData(new[] { 3, }, 3, new[] { 0, })]
	[InlineData(new[] { 3, }, 4, new[] { 6, 8, })]
	[InlineData(new[] { 3, }, 5, new[] { 5, 7, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 1, new[] { 2, 3, 2, 0, 1, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 2, new[] { 1, 2, 1, 6, 0, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 3, new[] { 0, 1, 0, 5, 6, 7, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 4, new[] { 6, 0, 6, 4, 5, 6, 7, 8, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 5, new[] { 5, 6, 5, 3, 4, 5, 6, 7, 7, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 6, new[] { 4, 5, 4, 2, 3, 4, 5, 6, 6, 7, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 7, new[] { 3, 4, 3, 1, 2, 3, 4, 5, 5, 6, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 8, new[] { 2, 3, 2, 0, 1, 2, 3, 4, 4, 5, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 9, new[] { 1, 2, 1, 6, 0, 1, 2, 3, 3, 4, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 10, new[] { 0, 1, 0, 5, 6, 0, 1, 2, 2, 3, 7, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 11, new[] { 6, 0, 6, 4, 5, 6, 0, 1, 1, 2, 6, 7, 8, 8, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 12, new[] { 5, 6, 5, 3, 4, 5, 6, 0, 0, 1, 5, 6, 7, 7, 7, 8, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 13, new[] { 4, 5, 4, 2, 3, 4, 5, 6, 6, 0, 4, 5, 6, 6, 6, 7, 7, 8, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 14, new[] { 3, 4, 3, 1, 2, 3, 4, 5, 5, 6, 3, 4, 5, 5, 5, 6, 6, 7, 7, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 15, new[] { 2, 3, 2, 0, 1, 2, 3, 4, 4, 5, 2, 3, 4, 4, 4, 5, 5, 6, 6, 7, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 16, new[] { 1, 2, 1, 6, 0, 1, 2, 3, 3, 4, 1, 2, 3, 3, 3, 4, 4, 5, 5, 6, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 17, new[] { 0, 1, 0, 5, 6, 0, 1, 2, 2, 3, 0, 1, 2, 2, 2, 3, 3, 4, 4, 5, 7, 8, })]
	[InlineData(new[] { 3, 4, 3, 1, 2, }, 18, new[] { 6, 0, 6, 4, 5, 6, 0, 1, 1, 2, 6, 0, 1, 1, 1, 2, 2, 3, 3, 4, 6, 7, 8, 8, 8, 8, })]
	public void Test2(IList<int> fishes, int days, IEnumerable<int> expected)
	{
		fishes = fishes.ToList();

		while (days-- > 0)
		{
			DoCycle(ref fishes);
			/*var count = fishes.Count;

			for (var a = 0; a < count; a++)
			{
				if (fishes[a] == 0)
				{
					fishes = fishes.Append(8).ToList();
				}

				fishes[a] = fishes[a] <= 0 ? 6 : fishes[a] - 1;
			}*/
		}

		Assert.Equal(expected, fishes);
	}

	private static void DoCycle(ref IList<int> fishes)
	{
		var count = fishes.Count;
		for (var a = 0; a < count; a++)
		{
			if (fishes[a] == 0)
			{
				fishes = fishes.Append(8).ToList();
			}

			fishes[a] = fishes[a] <= 0 ? 6 : fishes[a] - 1;
		}
	}

	[Theory]
	[InlineData(0, 6)]
	[InlineData(1, 0)]
	[InlineData(2, 1)]
	[InlineData(3, 2)]
	[InlineData(4, 3)]
	[InlineData(5, 4)]
	[InlineData(6, 5)]
	[InlineData(7, 6)]
	[InlineData(8, 7)]
	public void OneCycleTests(int start, int expected)
	{
		var actual = start <= 0 ? 6 : start - 1;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("3,4,3,1,2", 18, 26)]
	[InlineData("3,4,3,1,2", 80, 5_934)]
	//[InlineData("3,4,3,1,2", 256, 26_984_457_539)]
	public void Test3(string csv, int days, long expected)
	{
		IList<int> fishes = csv.Split(',').Select(int.Parse).ToList();
		while (days-- > 0) DoCycle(ref fishes);
		Assert.Equal(expected, fishes.Count);
	}

	[Theory]
	[InlineData("Day06.txt", 80, 345_793)]
	public async Task SolvePart1(string fileName, int days, int expected)
	{
		var csv = await fileName.ReadFileAsync();
		IList<int> fishes = csv.Split(',').Select(int.Parse).ToList();
		while (days-- > 0) DoCycle(ref fishes);
		Assert.Equal(expected, fishes.Count);
	}

	[Theory]
	[InlineData(new[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, }, 1, new[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, })]
	[InlineData(new[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, }, 2, new[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, })]
	[InlineData(new[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, }, 3, new[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, })]
	[InlineData(new[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, }, 4, new[] { 0, 0, 0, 0, 0, 0, 1, 0, 1, })]
	public void Test4(IList<int> fishes, int days, int[] expected)
	{
		while (days-- > 0)
		{
			DoCycle2(ref fishes);
		}

		Assert.Equal(expected, fishes);
	}

	private static void DoCycle2<T>(ref IList<T> fishes)
		where T : INumber<T>
	{
		var zeroes = fishes[0];
		fishes[0] = T.Zero;

		for (var a = 1; a < fishes.Count; a++)
		{
			if (fishes[a] > T.Zero)
			{
				fishes[a - 1] += fishes[a];
				fishes[a] = T.Zero;
			}
		}

		fishes[6] += zeroes;
		fishes[8] += zeroes;
	}

	[Theory]
	[InlineData("3,4,3,1,2", new[] { 0, 1, 1, 2, 1, 0, 0, 0, 0, })]
	public void ParseTests(string input, int[] expected)
	{
		var fishes = Parse<int>(input);
		Assert.Equal(expected, fishes);
	}

	private static IList<T> Parse<T>(string s)
		where T : INumber<T>
	{
		var fishes = new T[9] { T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, T.Zero, };
		foreach (var fish in s.Split(',').Select(byte.Parse))
		{
			fishes[fish]++;
		}
		return fishes;
	}

	[Theory]
	[InlineData("3,4,3,1,2", 18, 26)]
	[InlineData("3,4,3,1,2", 80, 5_934)]
	[InlineData("3,4,3,1,2", 256, 26_984_457_539)]
	public void Test5(string csv, int days, long expected)
	{
		var fishes = Parse<long>(csv);
		while (days-- > 0) DoCycle2(ref fishes);
		Assert.Equal(expected, fishes.Sum());
	}

	[Theory]
	[InlineData("3,4,3,1,2", 1, "1,1,2,1,0,0,0,0,0")]
	[InlineData("3,4,3,1,2", 2, "1,2,1,0,0,0,1,0,1")]
	[InlineData("3,4,3,1,2", 3, "2,1,0,0,0,1,1,1,1")]
	[InlineData("3,4,3,1,2", 4, "1,0,0,0,1,1,3,1,2")]
	public void Test6(string before, int days, string expected)
	{
		var fishes = Parse<int>(before);
		while (days-- > 0) DoCycle2(ref fishes);
		Assert.Equal(expected, string.Join(',', fishes));
	}

	[Theory]
	[InlineData("3,4,3,1,2", 1, 5)]
	[InlineData("3,4,3,1,2", 2, 6)]
	[InlineData("3,4,3,1,2", 3, 7)]
	[InlineData("3,4,3,1,2", 4, 9)]
	[InlineData("3,4,3,1,2", 5, 10)]
	[InlineData("3,4,3,1,2", 6, 10)]
	[InlineData("3,4,3,1,2", 7, 10)]
	[InlineData("3,4,3,1,2", 8, 10)]
	public void Test7(string before, int days, int expected)
	{
		var fishes = Parse<int>(before);
		while (days-- > 0) DoCycle2(ref fishes);
		Assert.Equal(expected, fishes.Sum());
	}

	[Theory]
	[InlineData(new[] { 0, 1, 1, 2, 1, 0, 0, 0, 0, }, 5)]
	public void SumTests(int[] values, int expected)
	{
		var actual = values.Sum();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day06.txt", 256, 1_572_643_095_893)]
	public async Task SolvePart2(string fileName, int days, ulong expected)
	{
		var csv = await fileName.ReadFileAsync();
		var fishes = Parse<ulong>(csv);
		while (days-- > 0) DoCycle2(ref fishes);
		Assert.Equal(expected, fishes.Sum());
	}
}
