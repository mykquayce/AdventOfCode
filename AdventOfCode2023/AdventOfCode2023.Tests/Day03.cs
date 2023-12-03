using System.Drawing;

namespace AdventOfCode2023.Tests;

public class Day03
{
	[Theory]
	[InlineData("467..114..",
		"...*......",
		"..35..633.",
		"......#...",
		"617*......",
		".....+.58.",
		"..592.....",
		"......755.",
		"...$.*....",
		".664.598..")]
	public void ParseInput(params string[] input)
	{
		var partNumbers = GetNumbers(input)
			.ToDictionary();

		Assert.NotEmpty(partNumbers);
		Assert.All(partNumbers.Keys, i => Assert.NotEqual(default, i));
		Assert.All(partNumbers.Values, Assert.NotEmpty);

		var symbols = GetSymbols(input).ToArray();

		Assert.NotEmpty(symbols);
	}

	[Theory]
	[InlineData('*', true)]
	[InlineData('#', true)]
	[InlineData('+', true)]
	[InlineData('$', true)]
	[InlineData('.', false)]
	[InlineData('0', false)]
	public void IsSymbolTests(char c, bool expected) => Assert.Equal(expected, IsSymbol(c));

	[Theory]
	[InlineData("467..114..",
		"...*......",
		"..35..633.",
		"......#...",
		"617*......",
		".....+.58.",
		"..592.....",
		"......755.",
		"...$.*....",
		".664.598..")]
	public void FindNeighbors(params string[] input)
	{
		var partNumbers = GetPartNumbers(input).ToArray();

		Assert.NotEmpty(partNumbers);
		Assert.DoesNotContain(default, partNumbers);
		Assert.Equal(partNumbers.Length, partNumbers.Distinct().Count());
	}

	[Theory]
	[InlineData(4_361,
		"467..114..",
		"...*......",
		"..35..633.",
		"......#...",
		"617*......",
		".....+.58.",
		"..592.....",
		"......755.",
		"...$.*....",
		".664.598..")]
	public void SolveExmaple1(int expected, params string[] input)
	{
		var actual = GetPartNumbers(input).Sum();
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(553_079)]
	public void SolvePart1(int expected)
	{
		var input = File.ReadLines(path: Path.Combine(".", "Data", "day03.txt")).ToArray();
		var actual = GetPartNumbers(input).Sum();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(467_835, "467..114..",
		"...*......",
		"..35..633.",
		"......#...",
		"617*......",
		".....+.58.",
		"..592.....",
		"......755.",
		"...$.*....",
		".664.598..")]
	public void FindGearsTests(int expected, params string[] input)
	{
		var gears = FindGears(input).ToArray();
		Assert.NotEmpty(gears);
		Assert.DoesNotContain(default, gears);

		var actual = gears.Sum(t => t.Item1 * t.Item2);
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(84_363_105)]
	public void SolvePart2(int expected)
	{
		var actual = 0;

		var input = File.ReadLines(path: Path.Combine(".", "Data", "day03.txt")).ToArray();
		foreach (var (left, right) in FindGears(input))
		{
			var rato = left * right;
			actual += rato;
		}

		Assert.Equal(expected, actual);
	}

	private static bool AreNeighbors(Point first, Point second)
		=> Math.Abs(first.X - second.X) <= 1 && Math.Abs(first.Y - second.Y) <= 1;

	private static IEnumerable<KeyValuePair<int, IReadOnlyCollection<Point>>> GetNumbers(IReadOnlyList<string> lines)
	{
		for (var y = 0; y < lines.Count; y++)
		{
			for (var x = 0; x < lines[y].Length; x++)
			{
				var digits = lines[y][x..].TakeWhile(char.IsDigit).ToArray();

				if (digits.Length <= 0) { continue; }

				var coords = Enumerable.Range(start: x, count: digits.Length)
					.Select(i => new Point(x: i, y: y))
					.ToArray();

				yield return new(int.Parse(new string(digits)), coords);

				x += digits.Length;
			}
		}
	}

	private static IEnumerable<KeyValuePair<char, Point>> GetSymbols(IReadOnlyList<string> lines)
	{
		for (var y = 0; y < lines.Count; y++)
		{
			for (var x = 0; x < lines[y].Length; x++)
			{
				var c = lines[y][x];
				if (IsSymbol(c))
				{
					var point = new Point(x: x, y: y);
					yield return new(c, point);
				}
			}
		}
	}

	private static bool IsSymbol(char c) => c != '.' && (char.IsPunctuation(c) || char.IsSymbol(c));

	private static IEnumerable<int> GetPartNumbers(IReadOnlyList<string> lines)
	{
		var numbers = GetNumbers(lines).ToArray();
		var symbols = GetSymbols(lines).ToArray();
		return GetPartNumbers(symbols, numbers);
	}

	private static IEnumerable<int> GetPartNumbers(
		IEnumerable<KeyValuePair<char, Point>> symbols,
		ICollection<KeyValuePair<int, IReadOnlyCollection<Point>>> numbers)
	{
		foreach (var (_, symbolPoint) in symbols)
		{
			foreach (var i in GetPartNumbers(symbolPoint, numbers))
			{
				yield return i;
			}
		}
	}

	private static IEnumerable<int> GetPartNumbers(
		Point symbol,
		ICollection<KeyValuePair<int, IReadOnlyCollection<Point>>> numbers)
	{
		foreach (var (i, points) in numbers)
		{
			foreach (var point in points)
			{
				if (AreNeighbors(symbol, point))
				{
					yield return i;
					break;
				}
			}
		}
	}

	private static IEnumerable<ValueTuple<int, int>> FindGears(IReadOnlyList<string> lines)
	{
		var symbols = GetSymbols(lines).Where(kvp => kvp.Key == '*').Select(kvp => kvp.Value);
		var numbers = GetNumbers(lines).ToArray();

		foreach (var symbol in symbols)
		{
			var neighbors = GetPartNumbers(symbol, numbers).ToArray();

			if (neighbors.Length == 2)
			{
				yield return ValueTuple.Create(neighbors[0], neighbors[1]);
			}
		}
	}
}
