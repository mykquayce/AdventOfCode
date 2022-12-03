using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022.Tests;

public class Day03 : Base
{
	[Theory]
	[InlineData("vJrwpWtwJgWrhcsFMMfFFhFp", 'p', 16)]
	[InlineData("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", 'L', 38)]
	[InlineData("PmmdzqPrVvPwwTWBwg", 'P', 42)]
	[InlineData("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", 'v', 22)]
	[InlineData("ttgJtRGJQctTZtZT", 't', 20)]
	[InlineData("CrZsJsPPZsGzwwsLwLmpwMDw", 's', 19)]
	public void SolveExample1(string input, char expectedCommon, int expectedPriority)
	{
		var rucksack = Rucksack.Parse(input, default);
		Assert.Equal(expectedCommon, rucksack.Common);
		Assert.Equal(expectedPriority, rucksack.Priority);
	}

	[Theory]
	[InlineData(
		157,
		"vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg", "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
	public void SolveExample2(int expected, params string[] inputs)
	{
		var actual = inputs.Select(s => Rucksack.Parse(s, default)).Select(r => r.Priority).Sum();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(7_446)]
	public async Task SolvePart1(int expected)
	{
		var actual = await base.GetInputAsync<Rucksack>().Select(r => r.Priority).SumAsync();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData('r', "vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg")]
	[InlineData('Z', "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
	public void SolveExample3(char expected, params string[] inputs)
	{
		var rucksacks = inputs.Select(s => Rucksack.Parse(s, default)).ToArray();
		var group = new Group(rucksacks);
		Assert.Equal(expected, group.Badge);
	}

	[Theory]
	[InlineData(
		70,
		"vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg", "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw")]
	public void SolveExample4(int expected, params string[] inputs)
	{
		var priorities = from array in inputs.Chunk(size: 3)
						 let rucksacks = from s in array
										 select Rucksack.Parse(s, default)
						 let @group = new Group(rucksacks.ToArray())
						 select @group.Priority;

		Assert.Equal(expected, priorities.Sum());
	}

	[Theory]
	[InlineData(2_646)]
	public async Task SolvePart2(int expected)
	{
		var rucksacks = await base.GetInputAsync<Rucksack>().ToListAsync();

		var priorities = from array in rucksacks.Chunk(size: 3)
						 let @group = new Group(array)
						 select @group.Priority;

		Assert.Equal(expected, priorities.Sum());
	}

	private readonly record struct Rucksack(string Contents, char Common, int Priority)
		: IParsable<Rucksack>
	{
		private const string _priorities = "\0abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public static Rucksack Parse(string s, IFormatProvider? provider)
		{
			var split = s.Length / 2;
			var first = s[..split];
			var second = s[split..];
			var common = first.Intersect(second).Single();
			var priority = _priorities.IndexOf(common);

			return new(s, common, priority);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rucksack result)
		{
			result = Parse(s!, provider);
			return true;
		}
	}

	private readonly record struct Group(IReadOnlyCollection<Rucksack> Rucksacks)
	{
		private const string _priorities = "\0abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public char Badge => Rucksacks.Select(r => r.Contents).Intersections().Single();
		public int Priority => _priorities.IndexOf(Badge);
	}
}
