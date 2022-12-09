using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2022.Tests;

public class Day08 : Base
{
	[Theory]
	[InlineData(
		new byte[5] { 3, 0, 3, 7, 3, },
		"30373", "25512", "65332", "33549", "35390")]
	public void ParseInputTests(byte[] expected, params string[] lines)
	{
		var trees = Trees.Parse(lines);

		var topRow = from kvp in trees.GetRow(0)
					 select kvp.Value;

		Assert.Equal(expected, topRow);
	}

	[Theory]
	[InlineData(1, 1,
		Visibilities.Left | Visibilities.Top,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(2, 1,
		Visibilities.Right | Visibilities.Top,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(3, 1,
		Visibilities.Invisible,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(1, 2,
		Visibilities.Right,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(2, 2,
		Visibilities.Invisible,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(3, 2,
		Visibilities.Right,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(1, 3,
		Visibilities.Invisible,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(2, 3,
		Visibilities.Bottom | Visibilities.Left,
		"30373", "25512", "65332", "33549", "35390")]
	[InlineData(3, 3,
		Visibilities.Invisible,
		"30373", "25512", "65332", "33549", "35390")]
	public void VisibilityTests(int treeX, int treeY, Visibilities expected, params string[] lines)
	{
		var trees = Trees.Parse(lines);
		var point = new Point(x: treeX, y: treeY);
		var actual = GetVisibility(trees, point);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(21, "30373", "25512", "65332", "33549", "35390")]
	public void CountVisible(int expected, params string[] lines)
	{
		var trees = Trees.Parse(lines);
		var query = from kvp in trees
					let visibility = GetVisibility(trees, kvp.Key)
					where visibility != Visibilities.Invisible
					select 1;
		Assert.Equal(expected, query.Count());
	}

	[Theory]
	[InlineData(1_538)]
	public async Task SolvePart1(int expected)
	{
		var lines = base.GetInputAsync();
		var trees = await Trees.ParseAsync(lines);
		var query = from kvp in trees
					let visibility = GetVisibility(trees, kvp.Key)
					where visibility != Visibilities.Invisible
					select 1;
		Assert.Equal(expected, query.Count());
	}

	[Theory]
	[InlineData(2, 1, 4, "30373", "25512", "65332", "33549", "35390")]
	[InlineData(2, 3, 8, "30373", "25512", "65332", "33549", "35390")]
	public void SceneicScoreTests(int treeX, int treeY, int expected, params string[] lines)
	{
		var trees = Trees.Parse(lines);
		var point = new Point(x: treeX, y: treeY);
		var tree = trees[point];
		var up = trees.GetAbove(point).TakeUpto(b => b >= tree).Count();
		var left = trees.GetLeft(point).TakeUpto(b => b >= tree).Count();
		var right = trees.GetRight(point).TakeUpto(b => b >= tree).Count();
		var bottom = trees.GetBelow(point).TakeUpto(b => b >= tree).Count();
		var actual = up * left * right * bottom;

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(8, "30373", "25512", "65332", "33549", "35390")]
	public void GetAllScenicScores(int expected, params string[] lines)
	{
		var scores = new List<int>();
		var trees = Trees.Parse(lines);
		foreach (var (point, tree) in trees)
		{
			var up = trees.GetAbove(point).TakeUpto(b => b >= tree).Count();
			var left = trees.GetLeft(point).TakeUpto(b => b >= tree).Count();
			var right = trees.GetRight(point).TakeUpto(b => b >= tree).Count();
			var bottom = trees.GetBelow(point).TakeUpto(b => b >= tree).Count();
			var score = up * left * right * bottom;
			scores.Add(score);
		}
		Assert.Equal(expected, scores.Max());
	}

	[Theory]
	[InlineData(4_961_25)]
	public async Task SolvePart2(int expected)
	{
		var actual = int.MinValue;
		var lines = base.GetInputAsync();
		var trees = await Trees.ParseAsync(lines);
		foreach (var (point, tree) in trees)
		{
			var up = trees.GetAbove(point).TakeUpto(b => b >= tree).Count();
			var left = trees.GetLeft(point).TakeUpto(b => b >= tree).Count();
			var right = trees.GetRight(point).TakeUpto(b => b >= tree).Count();
			var bottom = trees.GetBelow(point).TakeUpto(b => b >= tree).Count();
			var score = up * left * right * bottom;
			if (score > actual) actual = score;
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(5, new int[2] { 5, 2, }, new int[1] { 5, })]
	[InlineData(5, new int[2] { 1, 2, }, new int[2] { 1, 2, })]
	public void TakeDoWhile(int tree, int[] trees, int[] expected)
	{
		var actual = trees.TakeUpto(i => i >= tree).ToArray();
		Assert.Equal(expected, actual);
	}

	private static Visibilities GetVisibility(Trees trees, Point point)
	{
		var visibilities = Visibilities.Invisible;
		if (GetVisibilityBottom(trees, point)) visibilities |= Visibilities.Bottom;
		if (GetVisibilityLeft(trees, point)) visibilities |= Visibilities.Left;
		if (GetVisibilityRight(trees, point)) visibilities |= Visibilities.Right;
		if (GetVisibilityTop(trees, point)) visibilities |= Visibilities.Top;
		return visibilities;
	}

	private static bool GetVisibilityBottom(Trees trees, Point point)
	{
		var tree = trees[point];
		return trees.GetBelow(point).All(b => b < tree);
	}

	private static bool GetVisibilityLeft(Trees trees, Point point)
	{
		var tree = trees[point];
		return trees.GetLeft(point).All(b => b < tree);
	}

	private static bool GetVisibilityRight(Trees trees, Point point)
	{
		var tree = trees[point];
		return trees.GetRight(point).All(b => b < tree);
	}

	private static bool GetVisibilityTop(Trees trees, Point point)
	{
		var tree = trees[point];
		return trees.GetAbove(point).All(b => b < tree);
	}

	[Flags]
	public enum Visibilities : byte
	{
		Invisible = 0,
		Bottom = 1,
		Left = 2,
		Right = 4,
		Top = 8,
		Visible = 15,
	}

	private record Trees(IReadOnlyDictionary<Point, byte> Dictionary)
		: IReadOnlyDictionary<Point, byte>
	{
		private int? _height, _width;

		public int Height => _height ??= GetHeight();
		public int Width => _width ??= GetWidth();

		public int GetHeight() => Dictionary.Keys.Select(point => point.Y).Max();
		public int GetWidth() => Dictionary.Keys.Select(point => point.X).Max();

		public IEnumerable<KeyValuePair<Point, byte>> GetColumn(int column) => Dictionary.Where(kvp => kvp.Key.X == column);
		public IEnumerable<KeyValuePair<Point, byte>> GetRow(int row) => Dictionary.Where(kvp => kvp.Key.Y == row);

		public IEnumerable<byte> GetAbove(Point point)
		{
			return from kvp in GetColumn(point.X)
				   where kvp.Key.Y < point.Y
				   orderby kvp.Key.Y descending
				   select kvp.Value;
		}

		public IEnumerable<byte> GetBelow(Point point)
		{
			return from kvp in GetColumn(point.X)
				   where kvp.Key.Y > point.Y
				   orderby kvp.Key.Y
				   select kvp.Value;
		}

		public IEnumerable<byte> GetLeft(Point point)
		{
			return from kvp in GetRow(point.Y)
				   where kvp.Key.X < point.X
				   orderby kvp.Key.X descending
				   select kvp.Value;
		}

		public IEnumerable<byte> GetRight(Point point)
		{
			return from kvp in GetRow(point.Y)
				   where kvp.Key.X > point.X
				   orderby kvp.Key.X
				   select kvp.Value;
		}

		public static Trees Parse(IEnumerable<string> lines, IFormatProvider? provider = default)
		{
			var dictionary = new Dictionary<Point, byte>();
			var y = 0;
			foreach (var line in lines)
			{
				var x = 0;
				foreach (var @char in line)
				{
					var point = new Point(x: x, y: y);
					var value = byte.Parse(@char.ToString(), provider);
					dictionary.Add(point, value);
					x++;
				}
				y++;
			}
			return new(dictionary);
		}

		public static async Task<Trees> ParseAsync(IAsyncEnumerable<string> lines, IFormatProvider? provider = default)
		{
			var dictionary = new Dictionary<Point, byte>();
			var y = 0;
			await foreach (var line in lines)
			{
				var x = 0;
				foreach (var @char in line)
				{
					var point = new Point(x: x, y: y);
					var value = byte.Parse(@char.ToString(), provider);
					dictionary.Add(point, value);
					x++;
				}
				y++;
			}
			return new(dictionary);
		}

		#region ireadonlydictionary implementation
		public byte this[Point key] => Dictionary[key];
		public IEnumerable<Point> Keys => Dictionary.Keys;
		public IEnumerable<byte> Values => Dictionary.Values;
		public int Count => Dictionary.Count;
		public bool ContainsKey(Point key) => Dictionary.ContainsKey(key);
		public IEnumerator<KeyValuePair<Point, byte>> GetEnumerator() => Dictionary.GetEnumerator();
		public bool TryGetValue(Point key, [MaybeNullWhen(false)] out byte value) => Dictionary.TryGetValue(key, out value);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion ireadonlydictionary implementation
	}
}
