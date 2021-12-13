using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day13
{
	[Theory]
	[InlineData(@"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0", @"...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........")]
	public void ParseInput(string input, string expected)
	{
		var o = PaperObject.Parse(input, default);
		var actual = o.ToString();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0", 7, @"#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###
...........
...........")]
	public void FoldTests(string input, int row, string expected)
	{
		var before = PaperObject.Parse(input, default);
		var folded = before.FoldVertically(row);
		var actual = folded.ToString();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5", 17)]
	public void ParseInput2(string input, int expected)
	{
		var (pointsCsv, foldsCsv) = input.Split(Environment.NewLine + Environment.NewLine);

		var paper = PaperObject.Parse(pointsCsv!, default);

		var folds = from line in foldsCsv!.Split(Environment.NewLine)
					select FoldObject.Parse(line, default);

		var fold = folds.First();
		var foldedPaper = paper.Fold(fold);
		var actual = foldedPaper.Count;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day13.txt", 708)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var input = await fileName.ReadFileAsync();
		var (pointsCsv, foldsCsv) = input.Split(Environment.NewLine + Environment.NewLine);

		var paper = PaperObject.Parse(pointsCsv!, default);

		var folds = from line in foldsCsv!.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
					select FoldObject.Parse(line, default);

		var fold = folds.First();
		var foldedPaper = paper.Fold(fold);
		var actual = foldedPaper.Count;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day13.txt", 40, 6)]
	public async Task SolvePart2(string fileName, int expectedWidth, int expectedHeight)
	{
		var input = await fileName.ReadFileAsync();
		var (pointsCsv, foldsCsv) = input.Split(Environment.NewLine + Environment.NewLine);

		var paper = PaperObject.Parse(pointsCsv!, default);

		var folds = from line in foldsCsv!.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
					select FoldObject.Parse(line, default);

		foreach (var fold in folds)
		{
			paper = paper.Fold(fold);
		}

		Assert.Equal(expectedWidth, paper.Width);
		Assert.Equal(expectedHeight, paper.Height);
	}
}

public record PaperObject(IReadOnlyCollection<Point> Dots, int Width, int Height)
	: IParseable<PaperObject>, IReadOnlyCollection<Point>
{
	public PaperObject Fold(FoldObject fold)
	{
		return fold.Axis switch
		{
			FoldObject.Axes.Vertical => FoldVertically(fold.Position),
			FoldObject.Axes.Horizontal => FoldHorizontally(fold.Position),
			_ => throw new NotImplementedException(),
		};
	}

	public PaperObject FoldVertically(int row)
	{
		var top = Dots.Where(p => p.Y < row).ToList();
		var bottom = Dots.Where(p => p.Y > row).ToList();
		// flip bottom
		var height = (int)(Height / 2d);
		for (var a = 0; a < bottom.Count; a++)
		{
			var x = bottom[a].X;
			var y = row - (bottom[a].Y - row);
			bottom[a] = new Point(x, y);
		}

		var merged = top.Union(bottom).Distinct().ToList();
		return new(merged, Width, height);
	}

	public PaperObject FoldHorizontally(int column)
	{
		var left = Dots.Where(p => p.X < column).ToList();
		var right = Dots.Where(p => p.X > column).ToList();
		// flip bottom
		var width = (int)(Width / 2d);
		for (var a = 0; a < right.Count; a++)
		{
			var x = column - (right[a].X - column);
			var y = right[a].Y;
			right[a] = new Point(x, y);
		}

		var merged = left.Union(right).Distinct().ToList();
		return new(merged, width, Height);
	}

	#region ireadonlycollection implementation
	public int Count => Dots.Count;
	public IEnumerator<Point> GetEnumerator() => Dots.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	#endregion ireadonlycollection implementation

	#region iparseable implementation
	public static PaperObject Parse(string s, IFormatProvider? provider)
		=> TryParse(s, provider, out var result) ? result : throw new Exception();

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out PaperObject result)
	{
		var dots = (from line in s!.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
					let numbers = (from value in line.Split(',')
								   select int.Parse(value)
								  ).ToList()
					select new Point(x: numbers[0], y: numbers[1])
				   ).ToList();

		var width = dots.Max(point => point.X) + 1;
		var height = dots.Max(point => point.Y) + 1;
		result = new(dots, width, height);
		return true;
	}
	#endregion iparseable implementation

	public override string ToString()
	{
		var lines = (from y in Enumerable.Range(0, Height)
					 let line = (from x in Enumerable.Range(0, Width)
								 select '.'
								).ToArray()
					 select line
					).ToArray();

		foreach (var (x, y) in Dots) lines[y][x] = '#';

		var height = lines.GetLength(dimension: 0);

		return string.Join(Environment.NewLine, from y in Enumerable.Range(0, height)
												select new string(lines[y]));
	}
}

public record FoldObject(FoldObject.Axes Axis, int Position)
	: IParseable<FoldObject>
{
	private readonly static Regex _regex = new(@"^fold along (x|y)=(\d+)$");

	public static FoldObject Parse(string s, IFormatProvider? provider)
		=> TryParse(s, provider, out var result) ? result : throw new Exception();

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out FoldObject result)
	{
		var position = 0;
		var match = _regex.Match(s!);
		var ok = Enum.TryParse<Axes>(match.Groups[1].Value, ignoreCase: true, out var axis)
			&& int.TryParse(match.Groups[2].Value, System.Globalization.NumberStyles.None, provider, out position);

		result = new(axis, position);
		return ok;
	}

	[SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "<Pending>")]
	[Flags]
	public enum Axes : byte
	{
		None = 0,
		Horizontal = 1,
		X = 1,
		Vertical = 2,
		Y = 2,
	}
}
