using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Tests;

public partial class Day05 : Base
{
	[Theory]
	[InlineData(@"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2", "CMZ")]
	public void ParseInput(string input, string expected)
	{
		var (startingStacksString, rearrangementProcedureString) = input.Split(Environment.NewLine + Environment.NewLine);

		var stacks = ParseStackInput(startingStacksString!);
		var steps = rearrangementProcedureString!
			.Split(Environment.NewLine)
			.Select(s => Step.Parse(s, default))
			.ToArray();

		foreach (var step in steps)
		{
			foreach (var _ in Enumerable.Range(start: 0, count: step.Count))
			{
				var crate = stacks[step.From - 1].Pop();
				stacks[step.To - 1].Push(crate);
			}
		}

		var actual = new string(stacks.Select(s => s.Peek()).ToArray());
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(@"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 ",
		new[] { 'N', 'Z', }, new[] { 'D', 'C', 'M', }, new[] { 'P', })]
	public void ParseStartingStacks(string input, params char[][] expecteds)
	{
		var stacks = ParseStackInput(input);

		foreach (var (expected, stack) in (expecteds, stacks))
		{
			Assert.Equal(expected, stack.ToArray());
		}
	}

	[Theory]
	[InlineData("move 1 from 2 to 1", 1, 2, 1)]
	[InlineData("move 3 from 1 to 3", 3, 1, 3)]
	[InlineData("move 2 from 2 to 1", 2, 2, 1)]
	[InlineData("move 1 from 1 to 2", 1, 1, 2)]
	public void ParseSteps(string input, int expectedCount, int expectedFrom, int expectedTo)
	{
		var step = Step.Parse(input, default);
		Assert.Equal(expectedCount, step.Count);
		Assert.Equal(expectedFrom, step.From);
		Assert.Equal(expectedTo, step.To);
	}

	[Theory]
	[InlineData("FWNSHLDNZ  ")]
	public async Task SolvePart1(string expected)
	{
		var input = string.Join(Environment.NewLine, await base.Input.ToArrayAsync());
		var (startingStacksString, rearrangementProcedureString) = input.Split(Environment.NewLine + Environment.NewLine);

		var stacks = ParseStackInput(startingStacksString!);
		var steps = rearrangementProcedureString!
			.Split(Environment.NewLine)
			.Select(s => Step.Parse(s, default))
			.ToArray();

		foreach (var step in steps)
		{
			foreach (var _ in Enumerable.Range(start: 0, count: step.Count))
			{
				var crate = stacks[step.From - 1].Pop();
				stacks[step.To - 1].Push(crate);
			}
		}

		var query = from s in stacks
					let c = s.TryPeek(out var result) ? result : ' '
					select c;

		var actual = new string(query.ToArray());
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("RNRGDNFQG")]
	public async Task SolvePart2(string expected)
	{
		var input = string.Join(Environment.NewLine, await base.Input.ToArrayAsync());
		var (startingStacksString, rearrangementProcedureString) = input.Split(Environment.NewLine + Environment.NewLine);

		var stacks = ParseStackInput(startingStacksString!);
		var steps = rearrangementProcedureString!
			.Split(Environment.NewLine)
			.Select(s => Step.Parse(s, default))
			.ToArray();

		foreach (var step in steps)
		{
			var temp = new Stack<char>();

			foreach (var _ in Enumerable.Range(start: 0, count: step.Count))
			{
				var crate = stacks[step.From - 1].Pop();
				temp.Push(crate);
			}

			foreach (var _ in Enumerable.Range(start: 0, count: step.Count))
			{
				var crate = temp.Pop();
				stacks[step.To - 1].Push(crate);
			}
		}

		var query = from s in stacks
					let c = s.TryPeek(out var result) ? result : ' '
					select c;

		var actual = new string(query.ToArray());
		Assert.Equal(expected, actual);
	}

	private static IEnumerable<KeyValuePair<int, T>> GetValuesAndIndices<T>(IEnumerable<T> values)
	{
		var index = 0;
		foreach (var value in values)
		{
			yield return new(index++, value);
		}
	}

	private static IReadOnlyList<Stack<char>> ParseStackInput(string input)
	{
		// skip last line and reverse
		var lines = input.Split(Environment.NewLine).Reverse().Skip(1).ToArray();
		// get the max width
		var stackCount = (lines.Max(s => s.Length) - 1) / 3;
		// build the stacks
		var stacks = Enumerable.Repeat(() => new Stack<char>(), count: stackCount).Select(f => f()).ToArray();
		// get all the characters and their indices
		foreach (var line in lines)
		{
			foreach (var (index, @char) in GetValuesAndIndices<char>(line))
			{
				if (!char.IsLetter(@char)) { continue; }

				var stackNo = (index - 1) / 4;
				stacks[stackNo].Push(@char);
			}
		}

		return stacks;
	}

	private readonly partial record struct Step(int Count, int From, int To)
		: IParsable<Step>
	{
		public static Step Parse(string s, IFormatProvider? provider)
		{
			var match = Regex().Match(s);
			var count = int.Parse(match.Groups[1].Value);
			var from = int.Parse(match.Groups[2].Value);
			var to = int.Parse(match.Groups[3].Value);

			return new(count, from, to);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Step result)
		{
			result = Parse(s!, provider);
			return true;
		}

		[GeneratedRegex(@"^move (\d+) from (\d+) to (\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
		private static partial Regex Regex();
	}
}
