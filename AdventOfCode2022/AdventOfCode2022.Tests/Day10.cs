namespace AdventOfCode2022.Tests;

public class Day10 : Base
{
	[Theory]
	[InlineData(-1, 5, "noop", "addx 3", "addx -5")]
	public void SolveExample1(int expectedX, int expectedCycles, params string[] commands)
	{
		int x = 1, cycles = 0;

		foreach (var command in commands)
		{
			switch (command[..4])
			{
				case "addx":
					x += int.Parse(command[5..]);
					cycles += 2;
					break;
				case "noop":
					cycles++;
					break;
			}
		}

		Assert.Equal(expectedX, x);
		Assert.Equal(expectedCycles, cycles);
	}

	[Theory]
	[InlineData(new int[5] { 1, 1, 1, 4, 4, }, "noop", "addx 3", "addx -5")]
	public void SolveExample2(int[] expecteds, params string[] commands)
	{
		var actuals = RunProgram(commands);
		Assert.Equal(expecteds, actuals);
	}

	[Theory]
	[InlineData(19, 59, 99, 139, 179, 219)]
	public void IndexTests(params int[] expected)
	{
		var actual = Enumerable.Range(start: 0, count: 220).Where((_, i) => ((i - 19) % 40) == 0);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(
		new int[6] { 21, 19, 18, 21, 16, 18, },
		"addx 15", "addx -11", "addx 6", "addx -3", "addx 5", "addx -1", "addx -8", "addx 13", "addx 4",
		"noop", "addx -1", "addx 5", "addx -1", "addx 5", "addx -1", "addx 5", "addx -1", "addx 5",
		"addx -1", "addx -35", "addx 1", "addx 24", "addx -19", "addx 1", "addx 16", "addx -11", "noop",
		"noop", "addx 21", "addx -15", "noop", "noop", "addx -3", "addx 9", "addx 1", "addx -3", "addx 8",
		"addx 1", "addx 5", "noop", "noop", "noop", "noop", "noop", "addx -36", "noop", "addx 1",
		"addx 7", "noop", "noop", "noop", "addx 2", "addx 6", "noop", "noop", "noop", "noop", "noop",
		"addx 1", "noop", "noop", "addx 7", "addx 1", "noop", "addx -13", "addx 13", "addx 7", "noop",
		"addx 1", "addx -33", "noop", "noop", "noop", "addx 2", "noop", "noop", "noop", "addx 8", "noop",
		"addx -1", "addx 2", "addx 1", "noop", "addx 17", "addx -9", "addx 1", "addx 1", "addx -3",
		"addx 11", "noop", "noop", "addx 1", "noop", "addx 1", "noop", "noop", "addx -13", "addx -19",
		"addx 1", "addx 3", "addx 26", "addx -30", "addx 12", "addx -1", "addx 3", "addx 1", "noop",
		"noop", "noop", "addx -9", "addx 18", "addx 1", "addx 2", "noop", "noop", "addx 9", "noop", "noop",
		"noop", "addx -1", "addx 2", "addx -37", "addx 1", "addx 3", "noop", "addx 15", "addx -21",
		"addx 22", "addx -6", "addx 1", "noop", "addx 2", "addx 1", "noop", "addx -10", "noop", "noop",
		"addx 20", "addx 1", "addx 2", "addx 2", "addx -6", "addx -11", "noop", "noop", "noop")]
	public void SolveExample3(int[] expecteds, params string[] commands)
	{
		var values = RunProgram(commands).ToArray();
		var actuals = RunProgram(commands).Where((_, i) => ((i - 19) % 40) == 0);
		Assert.Equal(expecteds, actuals);
	}

	[Theory]
	[InlineData(
		new int[6] { 420, 1_140, 1_800, 2_940, 2_880, 3_960, },
		"addx 15", "addx -11", "addx 6", "addx -3", "addx 5", "addx -1", "addx -8", "addx 13", "addx 4",
		"noop", "addx -1", "addx 5", "addx -1", "addx 5", "addx -1", "addx 5", "addx -1", "addx 5",
		"addx -1", "addx -35", "addx 1", "addx 24", "addx -19", "addx 1", "addx 16", "addx -11", "noop",
		"noop", "addx 21", "addx -15", "noop", "noop", "addx -3", "addx 9", "addx 1", "addx -3", "addx 8",
		"addx 1", "addx 5", "noop", "noop", "noop", "noop", "noop", "addx -36", "noop", "addx 1",
		"addx 7", "noop", "noop", "noop", "addx 2", "addx 6", "noop", "noop", "noop", "noop", "noop",
		"addx 1", "noop", "noop", "addx 7", "addx 1", "noop", "addx -13", "addx 13", "addx 7", "noop",
		"addx 1", "addx -33", "noop", "noop", "noop", "addx 2", "noop", "noop", "noop", "addx 8", "noop",
		"addx -1", "addx 2", "addx 1", "noop", "addx 17", "addx -9", "addx 1", "addx 1", "addx -3",
		"addx 11", "noop", "noop", "addx 1", "noop", "addx 1", "noop", "noop", "addx -13", "addx -19",
		"addx 1", "addx 3", "addx 26", "addx -30", "addx 12", "addx -1", "addx 3", "addx 1", "noop",
		"noop", "noop", "addx -9", "addx 18", "addx 1", "addx 2", "noop", "noop", "addx 9", "noop", "noop",
		"noop", "addx -1", "addx 2", "addx -37", "addx 1", "addx 3", "noop", "addx 15", "addx -21",
		"addx 22", "addx -6", "addx 1", "noop", "addx 2", "addx 1", "noop", "addx -10", "noop", "noop",
		"addx 20", "addx 1", "addx 2", "addx 2", "addx -6", "addx -11", "noop", "noop", "noop")]
	public void SolveExample4(int[] expecteds, params string[] commands)
	{
		var values = RunProgram(commands);
		var actuals = from t in RunProgram(commands).Select((e, idx) => (e, idx))
					  where ((t.idx - 19) % 40) == 0
					  select t.e * (t.idx + 1);
		Assert.Equal(expecteds, actuals);
	}

	[Theory]
	[InlineData(14_540)]
	public async Task SolvePart1(int expected)
	{
		var commands = base.GetInputAsync();
		var values = RunProgramAsync(commands);
		var actuals = from t in values.Select((e, idx) => (e, idx))
					  where ((t.idx - 19) % 40) == 0
					  select t.e * (t.idx + 1);
		var actual = await actuals.SumAsync();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....",
		"addx 15", "addx -11", "addx 6", "addx -3", "addx 5", "addx -1", "addx -8", "addx 13", "addx 4",
		"noop", "addx -1", "addx 5", "addx -1", "addx 5", "addx -1", "addx 5", "addx -1", "addx 5",
		"addx -1", "addx -35", "addx 1", "addx 24", "addx -19", "addx 1", "addx 16", "addx -11", "noop",
		"noop", "addx 21", "addx -15", "noop", "noop", "addx -3", "addx 9", "addx 1", "addx -3", "addx 8",
		"addx 1", "addx 5", "noop", "noop", "noop", "noop", "noop", "addx -36", "noop", "addx 1",
		"addx 7", "noop", "noop", "noop", "addx 2", "addx 6", "noop", "noop", "noop", "noop", "noop",
		"addx 1", "noop", "noop", "addx 7", "addx 1", "noop", "addx -13", "addx 13", "addx 7", "noop",
		"addx 1", "addx -33", "noop", "noop", "noop", "addx 2", "noop", "noop", "noop", "addx 8", "noop",
		"addx -1", "addx 2", "addx 1", "noop", "addx 17", "addx -9", "addx 1", "addx 1", "addx -3",
		"addx 11", "noop", "noop", "addx 1", "noop", "addx 1", "noop", "noop", "addx -13", "addx -19",
		"addx 1", "addx 3", "addx 26", "addx -30", "addx 12", "addx -1", "addx 3", "addx 1", "noop",
		"noop", "noop", "addx -9", "addx 18", "addx 1", "addx 2", "noop", "noop", "addx 9", "noop", "noop",
		"noop", "addx -1", "addx 2", "addx -37", "addx 1", "addx 3", "noop", "addx 15", "addx -21",
		"addx 22", "addx -6", "addx 1", "noop", "addx 2", "addx 1", "noop", "addx -10", "noop", "noop",
		"addx 20", "addx 1", "addx 2", "addx 2", "addx -6", "addx -11", "noop", "noop", "noop")]
	public void SolveExample5(string expected, params string[] commands)
	{
		var output = new bool[240];
		{
			var cycle = 0;
			var values = RunProgram(commands);
			foreach (var value in values)
			{
				output[cycle] = Math.Abs(value - (cycle % 40)) <= 1;
				cycle++;
			}
		}

		var actual = Render(output);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"####.#..#.####.####.####.#..#..##..####.
#....#..#....#.#.......#.#..#.#..#....#.
###..####...#..###....#..####.#......#..
#....#..#..#...#.....#...#..#.#.....#...
#....#..#.#....#....#....#..#.#..#.#....
####.#..#.####.#....####.#..#..##..####.")]
	public async Task SolvePart2(string expected)
	{
		var output = new bool[240];
		{
			var cycle = 0;
			var commands = base.GetInputAsync();
			var values = RunProgramAsync(commands);
			await foreach (var value in values)
			{
				output[cycle] = Math.Abs(value - (cycle % 40)) <= 1;
				cycle++;
			}
		}

		var actual = Render(output);
		Assert.Equal(expected, actual);
	}

	private static string Render(IEnumerable<bool> output)
	{
		var lines = from bools in output.Chunk(size: 40)
					let cc = (from b in bools
							  let c = b ? '#' : '.'
							  select c
							).ToArray()
					select new string(cc);

		return string.Join(Environment.NewLine, lines);
	}

	private static IEnumerable<int> RunProgram(IEnumerable<string> commands)
	{
		int x = 1;
		foreach (var command in commands)
		{
			switch (command[..4])
			{
				case "addx":
					yield return x;
					yield return x;
					x += int.Parse(command[5..]);
					break;
				case "noop":
					yield return x;
					break;
			}
		}
	}

	private static async IAsyncEnumerable<int> RunProgramAsync(IAsyncEnumerable<string> commands)
	{
		int x = 1;
		await foreach (var command in commands)
		{
			switch (command[..4])
			{
				case "addx":
					yield return x;
					yield return x;
					x += int.Parse(command[5..]);
					break;
				case "noop":
					yield return x;
					break;
			}
		}
	}
}
