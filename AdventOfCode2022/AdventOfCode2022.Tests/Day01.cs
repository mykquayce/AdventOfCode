namespace AdventOfCode2022.Tests;

public class Day01 : Base
{
	[Theory]
	[InlineData(
		24_000,
		"1000", "2000", "3000", "", "4000", "", "5000", "6000", "", "7000", "8000", "9000", "", "10000", "")]
	public void SolveExample1(int expected, params string[] input)
	{
		int most = 0, current = 0;

		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				if (current > most)
				{
					most = current;
				}

				current = 0;
			}
			else
			{
				current += int.Parse(line);
			}
		}

		Assert.Equal(expected, most);
	}

	[Theory]
	[InlineData(69_310)]
	public async Task SolvePart1(int expected)
	{
		var most = 0;
		{
			var current = 0;
			await foreach (var line in Input)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					if (current > most)
					{
						most = current;
					}

					current = 0;
				}
				else
				{
					current += int.Parse(line);
				}
			}
		}

		Assert.Equal(expected, most);
	}

	[Theory]
	[InlineData(
		45_000,
		"1000", "2000", "3000", "", "4000", "", "5000", "6000", "", "7000", "8000", "9000", "", "10000", "")]
	public void SolveExample2(int expected, params string[] input)
	{
		ICollection<int> totals = new List<int>();

		var current = 0;

		foreach (var line in input)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				totals.Add(current);

				if (totals.Count > 3)
				{
					totals = totals.OrderDescending().Take(3).ToList();
				}

				current = 0;
			}
			else
			{
				current += int.Parse(line);
			}
		}

		Assert.Equal(expected, totals.Sum());
	}

	[Theory]
	[InlineData(206_104)]
	public async Task SolvePart2(int expected)
	{
		ICollection<int> totals = new List<int>();
		{
			var current = 0;
			await foreach (var line in Input)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					totals.Add(current);

					if (totals.Count > 3)
					{
						totals = totals.OrderDescending().Take(3).ToList();
					}

					current = 0;
				}
				else
				{
					current += int.Parse(line);
				}
			}
		}

		Assert.Equal(expected, totals.Sum());
	}
}
