using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022.Tests;

public class Day04 : Base
{
	[Theory]
	[InlineData("2-4,6-8", new int[] { 2, 3, 4, }, new int[] { 6, 7, 8, })]
	[InlineData("2-3,4-5", new int[] { 2, 3, }, new int[] { 4, 5, })]
	[InlineData("5-7,7-9", new int[] { 5, 6, 7, }, new int[] { 7, 8, 9, })]
	[InlineData("2-8,3-7", new int[] { 2, 3, 4, 5, 6, 7, 8, }, new int[] { 3, 4, 5, 6, 7, })]
	[InlineData("6-6,4-6", new int[] { 6, }, new int[] { 4, 5, 6, })]
	[InlineData("2-6,4-8", new int[] { 2, 3, 4, 5, 6, }, new int[] { 4, 5, 6, 7, 8, })]
	public void ParseInput(string input, params int[][] expecteds)
	{
		var group = Group.Parse(input, default);

		foreach (var (expected, elf) in (expecteds, group.Elves))
		{
			Assert.Equal(expected, elf.Ids);
		}
	}

	[Theory]
	[InlineData("2-4,6-8", false)]
	[InlineData("2-3,4-5", false)]
	[InlineData("5-7,7-9", false)]
	[InlineData("2-8,3-7", true)]
	[InlineData("6-6,4-6", true)]
	[InlineData("2-6,4-8", false)]
	public void FullyContains(string input, bool expected)
	{
		var group = Group.Parse(input, default);
		var idss = @group.Elves.Select(e => e.Ids);
		var intersection = idss.Intersections().ToArray();
		var actual = idss.Any(ids => intersection.SequenceEqual(ids));
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(2, "2-4,6-8", "2-3,4-5", "5-7,7-9", "2-8,3-7", "6-6,4-6", "2-6,4-8")]
	public void CountFullyContains(int expected, params string[] inputs)
	{
		var groups = inputs.Select(s => Group.Parse(s, default));

		var query = from g in groups
					let idss = from e in g.Elves
							   select e.Ids
					let intersection = idss.Intersections().ToArray()
					where idss.Any(ids => intersection.SequenceEqual(ids))
					select 1;

		var actual = query.Count();

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(448)]
	public async Task SolvePart1(int expected)
	{
		var groups = base.GetInputAsync<Group>();

		var query = from g in groups
					let idss = from e in g.Elves
							   select e.Ids
					let intersection = idss.Intersections().ToArray()
					where idss.Any(ids => intersection.SequenceEqual(ids))
					select 1;

		var actual = await query.CountAsync();

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("2-4,6-8", false)]
	[InlineData("2-3,4-5", false)]
	[InlineData("5-7,7-9", true)]
	[InlineData("2-8,3-7", true)]
	[InlineData("6-6,4-6", true)]
	[InlineData("2-6,4-8", true)]
	public void Overlap(string input, bool expected)
	{
		var group = Group.Parse(input, default);
		var ids = from g in @group.Elves
				  select g.Ids;
		var actual = ids.Intersections().Any();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(4, "2-4,6-8", "2-3,4-5", "5-7,7-9", "2-8,3-7", "6-6,4-6", "2-6,4-8")]
	public void CountOverlaps(int expected, params string[] inputs)
	{
		var group = inputs.Select(s => Group.Parse(s, default));

		var query = from g in @group
					let ids = from e in g.Elves
							  select e.Ids
					let overlaps = ids.Intersections().Any()
					where overlaps
					select 1;

		var actual = query.Count();

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(794)]
	public async Task SolvePart2(int expected)
	{
		var group = base.GetInputAsync<Group>();

		var query = from g in @group
					let ids = from e in g.Elves
							  select e.Ids
					let overlaps = ids.Intersections().Any()
					where overlaps
					select 1;

		var actual = await query.CountAsync();

		Assert.Equal(expected, actual);
	}

	private readonly record struct Group(IReadOnlyCollection<Elf> Elves)
		: IParsable<Group>
	{
		public static Group Parse(string s, IFormatProvider? provider)
		{
			var elves = s.Split(',', count: 2).Select(s => Elf.Parse(s, provider)).ToArray();
			return new(elves);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Group result)
		{
			result = Group.Parse(s!, provider);
			return true;
		}
	}

	private readonly record struct Elf(IReadOnlyCollection<int> Ids)
		: IParsable<Elf>
	{
		public static Elf Parse(string s, IFormatProvider? provider)
		{
			var (min, max) = s.Split('-', count: 2).Select(int.Parse);
			var ids = Enumerable.Range(min, (max - min) + 1).ToArray();
			return new(ids);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Elf result)
		{
			result = Parse(s!, default);
			return true;
		}
	}
}
