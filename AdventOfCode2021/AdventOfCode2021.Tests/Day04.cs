using System.Drawing;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day04
{
	[Theory]
	[InlineData(@"22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19",
		@"3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6",
		@"14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7",
		new[] { 7, 4, 9, 5, 11, 17, 23, 2, 0, 14, 21, 24, 10, 16, 13, 6, 15, 25, 12, 22, 18, 20, 8, 19, 3, 26, 1, },
		4_512)]
	public void Test1(string input1, string input2, string input3, IReadOnlyCollection<int> numbers, int expected)
	{
		var actual = 0;
		var boards = from input in new[] { input1, input2, input3, }
					 let arrayOfArrays = input.SplitTwiceAndParse<int>(outerSplitString: Environment.NewLine, innerSplitString: " ")
					 let board = arrayOfArrays.Reverse2dArrayToDictionary()
					 select board;

		var pointses = new List<List<Point>>()
		{
			new List<Point>(),
			new List<Point>(),
			new List<Point>(),
		};

		// get the coords of 7
		foreach (var number in numbers)
		{
			foreach (var (board, points) in (boards, pointses))
			{
				var ok = board.TryGetValue(number, out var point);

				if (!ok) continue;

				points.Add(point);

				if (points.Count < 5) continue;

				if (points.GroupBy(p => p.Y).Any(g => g.Count() == 5)
					|| points.GroupBy(p => p.X).Any(g => g.Count() == 5))
				{
					// winner
					var sum = board
						.Where(kvp => !points.Contains(kvp.Value))
						.Select(kvp => kvp.Key)
						.Sum();

					actual = sum * number;
					goto End;
				}
			}
		}

	End:
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day04.txt", 34_506)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var actual = 0;
		var contents = await fileName.ReadFileAsync();
		var numbers = contents
			.Split(Environment.NewLine)
			.First()
			.Split(',', StringSplitOptions.RemoveEmptyEntries)
			.Select(int.Parse)
			.ToList();

		var boards = string.Join(Environment.NewLine, contents.Split(Environment.NewLine).Skip(1))
			.Split(Environment.NewLine + Environment.NewLine)
			.Select(input => input.SplitTwiceAndParse<int>(outerSplitString: Environment.NewLine, innerSplitString: " "))
			.Select(arrayOfArrays => arrayOfArrays.Reverse2dArrayToDictionary())
			.ToList();


		var pointses = Enumerable.Range(0, boards.Count)
			.Select(_ => new List<Point>())
			.ToList();

		// get the coords of 7
		foreach (var number in numbers)
		{
			foreach (var (board, points) in (boards, pointses))
			{
				var ok = board.TryGetValue(number, out var point);

				if (!ok) continue;

				points.Add(point);

				// Test the points
				if (points.Count < 5) continue;

				if (points.GroupBy(p => p.Y).Any(g => g.Count() == 5)
					|| points.GroupBy(p => p.X).Any(g => g.Count() == 5))
				{
					// winner
					var sum = board
						.Where(kvp => !points.Contains(kvp.Value))
						.Select(kvp => kvp.Key)
						.Sum();

					actual = sum * number;
					goto End;
				}
			}
		}

	End:
		Assert.Equal(expected, actual);
	}
}
