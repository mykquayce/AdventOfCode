using System.Collections;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day03
{
	[Fact]
	public void Rotate2dArray()
	{
		// Arrange
		var input = new int[3, 2] { { 1, 2, }, { 3, 4, }, { 5, 6, }, };

		// Assert
		Assert.Equal(1, input[0, 0]);
		Assert.Equal(2, input[0, 1]);
		Assert.Equal(3, input[1, 0]);
		Assert.Equal(4, input[1, 1]);
		Assert.Equal(5, input[2, 0]);
		Assert.Equal(6, input[2, 1]);

		// Act
		var actual = input.Rotate();

		// Assert
		Assert.Equal(2, actual.GetLength(dimension: 0));
		Assert.Equal(3, actual.GetLength(dimension: 1));
		Assert.Equal(1, actual[0, 0]);
		Assert.Equal(3, actual[0, 1]);
		Assert.Equal(5, actual[0, 2]);
		Assert.Equal(2, actual[1, 0]);
		Assert.Equal(4, actual[1, 1]);
		Assert.Equal(6, actual[1, 2]);
	}

	[Fact]
	public void ArrayOfArrays_To2dArray()
	{
		var input = new[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010", };

		var _2d = input.To2dArray();

		var actual = _2d.To2dBoolArray(truthValue: '1');

		Assert.Equal(12, actual.GetLength(dimension: 0));
		Assert.Equal(5, actual.GetLength(dimension: 1));
	}

	[Theory]
	[InlineData(198, "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010")]
	public void Test1(int expected, params string[] strings)
	{
		bool[,] bools = strings.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

		var rows = bools.GetLength(dimension: 0);
		bool[] gamma = new bool[rows], epsilon = new bool[rows];

		for (var x = 0; x < rows; x++)
		{
			var row = bools.GetRow(row: x).ToList();
			var falses = row.Count(b => !b);
			var trues = row.Count(b => b);

			// gamma is the most common value in each row
			gamma[x] = trues > falses;
			epsilon[x] = !gamma[x];
		}

		var actual = gamma.ToDecimal() * epsilon.ToDecimal();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(22, true, false, true, true, false)]
	public void ToDecimalTests(int expected, params bool[] bools)
	{
		var actual = bools.ToDecimal();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day03.txt", 4_138_664)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var strings = await fileName.ReadLinesAsync().ToListAsync();
		bool[,] bools = strings.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

		var rows = bools.GetLength(dimension: 0);
		bool[] gamma = new bool[rows], epsilon = new bool[rows];

		for (var x = 0; x < rows; x++)
		{
			var row = bools.GetRow(row: x).ToList();
			var falses = row.Count(b => !b);
			var trues = row.Count(b => b);

			// gamma is the most common value in each row
			gamma[x] = trues > falses;
			epsilon[x] = !gamma[x];
		}

		var actual = gamma.ToDecimal() * epsilon.ToDecimal();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(
		0,
		new[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010", },
		new[] { "11110", "10110", "10111", "10101", "11100", "10000", "11001", })]
	[InlineData(
		1,
		new[] { "11110", "10110", "10111", "10101", "11100", "10000", "11001", },
		new[] { "10110", "10111", "10101", "10000", })]
	[InlineData(
		2,
		new[] { "10110", "10111", "10101", "10000", },
		new[] { "10110", "10111", "10101" })]
	[InlineData(
		3,
		new[] { "10110", "10111", "10101", },
		new[] { "10110", "10111", })]
	[InlineData(
		4,
		new[] { "10110", "10111", },
		new[] { "10111", })]
	public void OxygenGeneratorRatingTests(int column, string[] input, string[] expected)
	{
		var bools = input.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

		// To find oxygen generator rating, determine the most common value (0 or 1) in the current bit position
		var row = bools.GetRow(row: column).ToList();
		var falses = row.Count(b => !b);
		var trues = row.Count(b => b);
		// and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 1 in the position being considered.
		IList actual;
		if (trues >= falses)
		{
			actual = input.Where(i => i[column] == '1').ToList();
		}
		else
		{
			actual = input.Where(i => i[column] == '0').ToList();
		}

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(
		0,
		new[] { "00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010", },
		new[] { "00100", "01111", "00111", "00010", "01010", })]
	[InlineData(
		1,
		new[] { "00100", "01111", "00111", "00010", "01010", },
		new[] { "01111", "01010", })]
	[InlineData(
		2,
		new[] { "01111", "01010", },
		new[] { "01010", })]
	public void CO2ScrubberRatingTests(int column, string[] input, string[] expected)
	{
		var bools = input.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

		// To find oxygen generator rating, determine the most common value (0 or 1) in the current bit position
		var row = bools.GetRow(row: column).ToList();
		var falses = row.Count(b => !b);
		var trues = row.Count(b => b);
		// and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 1 in the position being considered.
		IList actual;
		if (falses > trues)
		{
			actual = input.Where(i => i[column] == '1').ToList();
		}
		else
		{
			actual = input.Where(i => i[column] == '0').ToList();
		}

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(
		"10111",
		"00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010")]
	public void GetOxygenGeneratorRatingTests(string expected, params string[] input)
	{
		var column = 0;

		while (input.Length > 1)
		{
			var bools = input.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

			// To find oxygen generator rating, determine the most common value (0 or 1) in the current bit position
			var row = bools.GetRow(row: column).ToList();
			var falses = row.Count(b => !b);
			var trues = row.Count(b => b);
			// and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 1 in the position being considered.
			if (trues >= falses)
			{
				input = input.Where(i => i[column] == '1').ToArray();
			}
			else
			{
				input = input.Where(i => i[column] == '0').ToArray();
			}

			column++;
		}

		Assert.Single(input);
		Assert.Equal(expected, input[0]);
	}

	[Theory]
	[InlineData(
		"01010",
		"00100", "11110", "10110", "10111", "10101", "01111", "00111", "11100", "10000", "11001", "00010", "01010")]
	public void GetCO2ScrubberRatingTests(string expected, params string[] input)
	{
		var column = 0;

		while (input.Length > 1)
		{
			var bools = input.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

			// To find CO2 scrubber rating, determine the least common value (0 or 1) in the current bit position
			var row = bools.GetRow(row: column).ToList();
			var falses = row.Count(b => !b);
			var trues = row.Count(b => b);
			// and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 0 in the position being considered.
			if (falses > trues)
			{
				input = input.Where(i => i[column] == '1').ToArray();
			}
			else
			{
				input = input.Where(i => i[column] == '0').ToArray();
			}

			column++;
		}

		Assert.Single(input);
		Assert.Equal(expected, input[0]);
	}

	[Theory]
	[InlineData("Day03.txt", 4_273_224)]
	public async Task SolvePart2(string fileName, int expected)
	{
		int oxygen, co2;

		// oxygen
		{
			var input = await fileName.ReadLinesAsync().ToArrayAsync();
			var column = 0;

			while (input.Length > 1)
			{
				var bools = input.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

				// To find oxygen generator rating, determine the most common value (0 or 1) in the current bit position
				var row = bools.GetRow(row: column).ToList();
				var falses = row.Count(b => !b);
				var trues = row.Count(b => b);
				// and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 1 in the position being considered.
				if (trues >= falses)
				{
					input = input.Where(i => i[column] == '1').ToArray();
				}
				else
				{
					input = input.Where(i => i[column] == '0').ToArray();
				}

				column++;
			}

			oxygen = input[0].ToDecimal();
		}

		// co2
		{
			var input = await fileName.ReadLinesAsync().ToArrayAsync();
			var column = 0;

			while (input.Length > 1)
			{
				var bools = input.To2dArray().Rotate().To2dBoolArray(truthValue: '1');

				// To find CO2 scrubber rating, determine the least common value (0 or 1) in the current bit position
				var row = bools.GetRow(row: column).ToList();
				var falses = row.Count(b => !b);
				var trues = row.Count(b => b);
				// and keep only numbers with that bit in that position. If 0 and 1 are equally common, keep values with a 0 in the position being considered.
				if (falses > trues)
				{
					input = input.Where(i => i[column] == '1').ToArray();
				}
				else
				{
					input = input.Where(i => i[column] == '0').ToArray();
				}

				column++;
			}

			co2 = input[0].ToDecimal();
		}

		var actual = oxygen * co2;
		Assert.Equal(expected, actual);
	}
}
