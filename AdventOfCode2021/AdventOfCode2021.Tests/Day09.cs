using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day09
{
	[Theory]
	[InlineData(@"2199943210
3987894921
9856789892
8767896789
9899965678", new[] { "1,0", "9,0", "2,2", "6,4", })]
	public void ParseInput(string input, string[] expected)
	{
		var actual = new List<string>();
		var heightMap = HeightMap<byte>.Parse(input, default);

		foreach (var (point, value) in heightMap)
		{
			var neighbors = heightMap.GetNeighbors(point);
			var isLowest = neighbors.All(kvp => kvp.Value > value);
			if (isLowest) actual.Add($"{point.X},{point.Y}");
		}

		Assert.Equal(expected, actual);
	}
	[Theory]
	[InlineData(@"2199943210
3987894921
9856789892
8767896789
9899965678", 15)]
	public void Test1(string input, int expected)
	{
		var heightMap = HeightMap<byte>.Parse(input, default);
		var lowestPoints = heightMap.GetLowestPoints();
		var riskLevel = lowestPoints.Select(kvp => kvp.Value + 1).Sum();
		Assert.Equal(expected, riskLevel);
	}

	[Theory]
	[InlineData("Day09.txt", 577)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var input = await fileName.ReadFileAsync();
		var heightMap = HeightMap<byte>.Parse(input, default);
		var lowestPoints = heightMap.GetLowestPoints();
		var riskLevel = lowestPoints.Select(kvp => kvp.Value + 1).Sum();
		Assert.Equal(expected, riskLevel);
	}

	[Theory]
	[InlineData(@"2199943210
3987894921
9856789892
8767896789
9899965678", "1,0", new[] { "0,0", "1,0", "0,1", })]
	public void GetBasinTests(string input, string startCsv, string[] expected)
	{
		var point = startCsv.Split(',').Select(int.Parse).ToPoint();
		var heightMap = HeightMap<byte>.Parse(input, default);
		var basin = heightMap.GetBasin(point);

		var actual = basin.Keys.Select(point => $"{point.X},{point.Y}");
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"2199943210
3987894921
9856789892
8767896789
9899965678", new[] { 3, 9, 14, 9, })]
	public void GetBasinsTests(string input, int[] expected)
	{
		var heightMap = HeightMap<byte>.Parse(input, default);
		var basins = heightMap.GetBasins();
		var actual = basins.Select(basin => basin.Count);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"2199943210
3987894921
9856789892
8767896789
9899965678", 1_134)]
	public void Test2(string input, int expected)
	{
		var heightMap = HeightMap<byte>.Parse(input, default);
		var basins = heightMap.GetBasins();
		var counts = basins.Select(basin => basin.Count).ToList();
		var top3 = counts.OrderByDescending(i=>i).Take(3).ToList();
		var product = top3.Product();
		var actual = basins.Select(basin => basin.Count).OrderByDescending(i => i).Take(3).Product();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day09.txt", 1_069_200)]
	public async Task SolvePart2(string fileName, int expected)
	{
		var input = await fileName.ReadFileAsync();
		var heightMap = HeightMap<byte>.Parse(input, default);
		var basins = heightMap.GetBasins();
		var actual = basins.Select(basin => basin.Count).OrderByDescending(i => i).Take(3).Product();
		Assert.Equal(expected, actual);
	}
}

public record HeightMap<T>(IReadOnlyDictionary<Point, T> Heights)
	: IParseable<HeightMap<T>>, IReadOnlyDictionary<Point, T>
	where T : INumber<T>//, IParseable<T>
{
	public T this[int x, int y] => this[new Point(x: x, y: y)];

	public IEnumerable<KeyValuePair<Point, T>> GetNeighbors(Point point)
	{
		return from kvp in this
			   where kvp.Key != point
			   let horizontal = Math.Abs(kvp.Key.X - point.X)
			   where horizontal < 2
			   let vertical = Math.Abs(kvp.Key.Y - point.Y)
			   where vertical < 2
			   where horizontal == 1 && vertical == 0
				  || horizontal == 0 && vertical == 1
			   select kvp;
	}

	public IEnumerable<KeyValuePair<Point, T>> GetLowestPoints()
	{
		foreach (var kvp in this)
		{
			var neighbors = GetNeighbors(kvp.Key);
			var isLowest = neighbors.All(a => a.Value > kvp.Value);
			if (isLowest) yield return kvp;
		}
	}

	public IReadOnlyDictionary<Point, T> GetBasin(Point point)
	{
		var dictionary = new Dictionary<Point, T>();

		void recurse(Point p)
		{
			foreach (var kvp in from kvp in GetNeighbors(p)
								where kvp.Value < T.Create(9)
								select kvp)
			{
				if (dictionary.TryAdd(kvp.Key, kvp.Value))
				{
					recurse(kvp.Key);
				}
			}
		}

		recurse(point);

		return dictionary;
	}

	public IEnumerable<IReadOnlyDictionary<Point, T>> GetBasins()
	{
		foreach (var (point, _) in GetLowestPoints())
		{
			yield return GetBasin(point);
		}
	}

	#region iparseable implementation
	public static HeightMap<T> Parse(string s, IFormatProvider? provider)
		=> TryParse(s, provider, out var result) ? result : throw new Exception();

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out HeightMap<T> result)
	{
		var dictionary = new Dictionary<Point, T>();
		var y = 0;
		foreach (var line in s!.Split(Environment.NewLine))
		{
			var x = 0;
			foreach (var @char in line)
			{
				var key = new Point(x: x, y: y);
				var ok = T.TryParse(@char.ToString(), provider, out var value);
				if (!ok)
				{
					result = default!;
					return false;
				}
				dictionary.Add(key, value);
				x++;
			}
			y++;
		}
		result = new HeightMap<T>(dictionary);
		return true;
	}
	#endregion iparseable implementation

	#region ireadonlydictionary implementation
	public T this[Point key] => Heights[key];
	public IEnumerable<Point> Keys => Heights.Keys;
	public IEnumerable<T> Values => Heights.Values;
	public int Count => Heights.Count;
	public bool ContainsKey(Point key) => Heights.ContainsKey(key);
	public IEnumerator<KeyValuePair<Point, T>> GetEnumerator() => Heights.GetEnumerator();
	public bool TryGetValue(Point key, [MaybeNullWhen(false)] out T value) => Heights.TryGetValue(key, out value);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	#endregion ireadonlydictionary implementation
}
