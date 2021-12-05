using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day05
{
	[Theory]
	[InlineData(@"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2", 10, 5)]
	public void Test1(string input, int expectedCount, int expectedOverlap)
	{
		var vents = (from ventString in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
					 select Vent.Parse(ventString, provider: default)
					).ToList();

		Assert.Equal(expectedCount, vents.Count);
		Assert.DoesNotContain(default, vents);
		Assert.All(vents, v => Assert.NotEqual(v.A, v.B));

		var actual = (from vent in vents
					  where vent.IsHorizontalOrVertial
					  from point in vent.GetPointsInbetween()
					  group point by point into gg
					  where gg.Count() > 1
					  select gg.Key
					).Count();

		Assert.Equal(expectedOverlap, actual);
	}

	[Theory]
	[InlineData("0,9 -> 5,9", "0,9", "1,9", "2,9", "3,9", "4,9", "5,9")]
	[InlineData("5,9 -> 0,9", "5,9", "4,9", "3,9", "2,9", "1,9", "0,9")]
	[InlineData("1,1 -> 3,3", "1,1", "2,2", "3,3")]
	//An entry like 9,7 -> 7,9 covers points 9,7, 8,8, and 7,9.
	[InlineData("9,7 -> 7,9", "9,7", "8,8", "7,9")]
	public void PointsBetween(string ventString, params string[] expected)
	{
		var vent = Vent.Parse(ventString, provider: default);

		var actual = vent.GetPointsInbetween().Select(p => $"{p.X},{p.Y}");
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day05.txt", 6_548)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var vents = await fileName.ReadAndParseLinesAsync<Vent>()
			.Where(v => v.IsHorizontalOrVertial)
			.ToListAsync();

		var actual = (from vent in vents
					  from point in vent.GetPointsInbetween()
					  group point by point into gg
					  where gg.Count() > 1
					  select gg.Key
					 ).Count();

		Assert.Equal(expected, actual);
	}
	[Theory]
	[InlineData(@"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2", 10, 12)]
	public void Test2(string input, int expectedCount, int expectedOverlap)
	{
		var vents = (from ventString in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
					 select Vent.Parse(ventString, provider: default)
					).ToList();

		Assert.Equal(expectedCount, vents.Count);
		Assert.DoesNotContain(default, vents);
		Assert.All(vents, v => Assert.NotEqual(v.A, v.B));

		var actual = (from vent in vents
					  from point in vent.GetPointsInbetween()
					  group point by point into gg
					  where gg.Count() > 1
					  select gg.Key
					).ToList();

		Assert.Equal(expectedOverlap, actual.Count);
	}

	[Theory]
	[InlineData("Day05.txt", 19_663)]
	public async Task SolvePart2(string fileName, int expected)
	{
		var vents = await fileName.ReadAndParseLinesAsync<Vent>()
			.ToListAsync();

		var actual = (from vent in vents
					  from point in vent.GetPointsInbetween()
					  group point by point into gg
					  where gg.Count() > 1
					  select gg.Key
					 ).Count();

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(7, 9, 7, 8, 9)]
	[InlineData(9, 7, 9, 8, 7)]
	public void GetValuesVetweenInclusiveTests(int a, int b, params int[] expected)
	{
		var actual = Vent.ValuesBetweenInclusive(a, b);
		Assert.Equal(expected, actual);
	}
}

public record Vent(Point A, Point B)
	: IParseable<Vent>
{
	public bool IsHorizontal => A.X == B.X;
	public bool IsVertical => A.Y == B.Y;
	public bool IsHorizontalOrVertial => IsHorizontal || IsVertical;

	public IEnumerable<Point> GetPointsInbetween()
	{
		var xs = ValuesBetweenInclusive(A.X, B.X).ToList();
		var ys = ValuesBetweenInclusive(A.Y, B.Y).ToList();
		var count = Math.Max(xs.Count, ys.Count);

		return Enumerable.Range(0, count)
			.Select(i => new Point(xs[i % xs.Count], ys[i % ys.Count]));
	}

	public static IEnumerable<int> ValuesBetweenInclusive(int a, int b)
	{
		if (a == b)
		{
			yield return a;
			yield break;
		}
		yield return a;
		var step = a < b ? 1 : -1;
		do
		{
			a += step;
			yield return a;
		}
		while (a != b);
	}

	#region iparseable implementation
	public static Vent Parse(string s, IFormatProvider? provider)
	{
		return TryParse(s, provider, out var vent) ? vent : throw new Exception();
	}

	private readonly static Regex _regex = new(@"^(\d+),(\d+) -> (\d+),(\d+)$");

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Vent result)
	{
		//0,9 -> 5,9
		var match = _regex.Match(s!);

		var values = Enumerable.Range(1, 4)
			.Select(i => match.Groups[i].Value)
			.Select(s => int.Parse(s, provider: provider))
			.ToList();

		var a = new Point(values[0], values[1]);
		var b = new Point(values[2], values[3]);
		result = new Vent(a, b);
		return true;
	}
	#endregion iparseable implementation
}
