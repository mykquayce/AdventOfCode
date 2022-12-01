namespace AdventOfCode2022.Tests;

public class Day01
{
	[Theory]
	[InlineData(24_000, "1000", "2000", "3000", "", "4000", "", "5000", "6000", "", "7000", "8000", "9000", "", "10000", "")]
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
	[InlineData(69_310, ".", "Data", "day01.txt")]
	public async Task SolvePart1(int expected, params string[] paths)
	{
		var most = 0;
		{
			var path = Path.Combine(paths);
			using var reader = new StreamReader(path);
			var current = 0;
			string? line;
			while ((line = await reader.ReadLineAsync()) is not null)
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
	[InlineData(45_000, "1000", "2000", "3000", "", "4000", "", "5000", "6000", "", "7000", "8000", "9000", "", "10000", "")]
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
	[InlineData(206_104, ".", "Data", "day01.txt")]
	public async Task SolvePart2(int expected, params string[] paths)
	{
		ICollection<int> totals = new List<int>();
		{
			var path = Path.Combine(paths);
			using var reader = new StreamReader(path);
			var current = 0;
			string? line;
			while ((line = await reader.ReadLineAsync()) is not null)
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
