namespace AdventOfCode2023.Tests;

public class Day01
{
	[Theory]
	[InlineData("1abc2", 12)]
	[InlineData("pqr3stu8vwx", 38)]
	[InlineData("a1b2c3d4e5f", 15)]
	[InlineData("treb7uchet", 77)]
	public void Example1(string input, int expected)
	{
		var actual = GetCalibrationValue(input);
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(54_388)]
	public async Task SolvePart1(int expected)
	{
		var actual = 0;
		string? line;
		using var reader = new StreamReader(path: Path.Combine(".", "Data", "day01.txt"));
		while ((line = await reader.ReadLineAsync()) is not null)
		{
			var value = GetCalibrationValue(line);
			actual += value;
		}
		Assert.Equal(expected, actual);
	}

	private static int GetCalibrationValue(string input)
	{
		var first = input.First(char.IsDigit);
		var second = input.Last(char.IsDigit);
		return int.Parse(new string(new char[2] { first, second, }));
	}

	private static readonly IReadOnlyDictionary<char, string> _numbers = new Dictionary<char, string>
	{
		['1'] = "one",
		['2'] = "two",
		['3'] = "three",
		['4'] = "four",
		['5'] = "five",
		['6'] = "six",
		['7'] = "seven",
		['8'] = "eight",
		['9'] = "nine",
	};

	[Theory]
	[InlineData("two1nine", 29)]
	[InlineData("eightwothree", 83)]
	[InlineData("abcone2threexyz", 13)]
	[InlineData("xtwone3four", 24)]
	[InlineData("4nineeightseven2", 42)]
	[InlineData("zoneight234", 14)]
	[InlineData("7pqrstsixteen", 76)]
	public void Example2(string input, int expected)
	{
		var actual = GetCalibrationValue2(input);
		Assert.Equal(expected, actual);
	}

	private static int GetCalibrationValue2(string input)
	{
		char left = FindDigit(Enumerable.Range(0, input.Length).Select(i => input[i..]));
		char right = FindDigit(Enumerable.Range(0, input.Length).Reverse().Select(i => input[i..]));
		return int.Parse(new string(new char[2] { left, right, }));
	}

	private static char FindDigit(IEnumerable<string> inputs)
	{
		foreach (var input in inputs)
		{
			var digit = FindDigit(input);

			if (digit != default)
			{
				return digit;
			}
		}

		throw new Exception();
	}

	private static char FindDigit(string input)
	{
		if (char.IsDigit(input[0]))
		{
			return input[0];
		}

		foreach (var (c, s) in _numbers)
		{
			if (input.StartsWith(s, StringComparison.OrdinalIgnoreCase))
			{
				return c;
			}
		}

		return default;
	}

	[Theory, InlineData(53_515)]
	public async Task SolvePart2(int expected)
	{
		var actual = 0;
		string? line;
		using var reader = new StreamReader(path: Path.Combine(".", "Data", "day01.txt"));
		while ((line = await reader.ReadLineAsync()) is not null)
		{
			var value = GetCalibrationValue2(line);
			actual += value;
		}
		Assert.Equal(expected, actual);
	}
}
