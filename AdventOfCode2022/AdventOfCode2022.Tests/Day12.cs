using System.Collections.Concurrent;
using System.Drawing;

namespace AdventOfCode2022.Tests;

public class Day12 : Base
{
	[Theory]
	[InlineData(31, "Sabqponm", "abcryxxl", "accszExk", "acctuvwj", "abdefghi")]
	public void ParseInputTests(int expected, params string[] lines)
	{
		var map = ParseInput(lines).ToDictionary();
		var actual = FindShortestRoute(map);
		Assert.Equal(expected, actual.Count - 1);
	}

	[Theory]
	[InlineData(-1)]
	public async Task SolvePart1(int expected)
	{
		var lines = base.GetInputAsync();
		var map = await ParseInputAsync(lines).ToDictionaryAsync(kvp => kvp.Key, kvp => kvp.Value);
		var actual = FindShortestRoute(map);
		Assert.Equal(expected, actual.Count - 1);
	}

	private static IEnumerable<KeyValuePair<Point, Level>> ParseInput(IEnumerable<string> lines)
	{
		var y = 0;
		foreach (var line in lines)
		{
			for (var x = 0; x < line.Length; x++)
			{
				var point = new Point(x: x, y: y);
				var @char = line[x];
				var level = Enum.Parse<Level>(@char.ToString());
				yield return new(point, level);
			}
			y++;
		}
	}

	private static async IAsyncEnumerable<KeyValuePair<Point, Level>> ParseInputAsync(IAsyncEnumerable<string> lines)
	{
		var y = 0;
		await foreach (var line in lines)
		{
			for (var x = 0; x < line.Length; x++)
			{
				var point = new Point(x: x, y: y);
				var @char = line[x];
				var level = Enum.Parse<Level>(@char.ToString());
				yield return new(point, level);
			}
			y++;
		}
	}

	private static ICollection<Point> FindShortestRoute(IDictionary<Point, Level> map)
	{

		var start = map.Single(kvp => kvp.Value == Level.S).Key;
		var end = map.Single(kvp => kvp.Value == Level.E).Key;

		var routes = new ConcurrentQueue<ICollection<Point>>();
		routes.Enqueue(new List<Point> { start, });

		while (routes.All(r => !r.Contains(end)))
		{
			var route = routes.TryDequeue(out var r)
				? r
				: throw new Exception();

			var curr = route.Last();

			// build neighbor points
			var points = new Point[4]
			{
				new Point(x: curr.X, y: curr.Y + 1),
				new Point(x: curr.X - 1, y: curr.Y),
				new Point(x: curr.X + 1, y: curr.Y),
				new Point(x: curr.X, y: curr.Y - 1),
			};

			foreach (var point in points)
			{
				var ok = map.TryGetValue(point, out var level);
				if (!ok) { continue; }
				if (Math.Abs(level - map[curr]) > 1) { continue; }
				if (route.Contains(point)) { continue; }

				var @new = route.Append(point).ToArray();
				routes.Enqueue(@new);
			}
		}

		return routes.Single(r => r.Contains(end));
	}

	public enum Level : sbyte
	{
		S = 0,
		a = 1,
		b = 2,
		c = 3,
		d = 4,
		e = 5,
		f = 6,
		g = 7,
		h = 8,
		i = 9,
		j = 10,
		k = 11,
		l = 12,
		m = 13,
		n = 14,
		o = 15,
		p = 16,
		q = 17,
		r = 18,
		s = 19,
		t = 20,
		u = 21,
		v = 22,
		w = 23,
		x = 24,
		y = 25,
		z = 26,
		E = 27,
	}

	[Theory]
	[InlineData('a', Level.a)]
	public void EnumParseTests(char c, Level expected)
	{
		var actual = Enum.Parse<Level>(c.ToString());
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(Level.a, Level.b, 1)]
	public void EnumCompareTests(Level left, Level right, int expected)
	{
		var actual = Math.Abs(left - right);
		Assert.Equal(expected, actual);
	}
}
