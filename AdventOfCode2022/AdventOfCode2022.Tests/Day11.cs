using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022.Tests;

public class Day11 : Base
{
	private const string _sampleInput = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

	[Theory]
	[InlineData(
		"Monkey 0:",
		"  Starting items: 79, 98",
		"  Operation: new = old * 19",
		"  Test: divisible by 23",
		"    If true: throw to monkey 2",
		"    If false: throw to monkey 3")]
	public void ParseInput1(params string[] lines)
	{
		var text = string.Join(Environment.NewLine, lines);
		var monkey = Monkey.Parse(text, default);
		Assert.NotEqual(default, monkey);
	}

	[Theory]
	[InlineData(_sampleInput)]
	public void ParseInput2(string text)
	{
		var monkeys = Monkeys.Parse(text, default);
		Assert.NotEmpty(monkeys);
		Assert.All(monkeys.Keys, idx => Assert.InRange(idx, 0, 3));
		Assert.DoesNotContain(null, monkeys.Values);
	}

	[Theory]
	[InlineData(_sampleInput)]
	public void PlayTurns(string text)
	{
		var monkeys = Monkeys.Parse(text, default);
		Assert.Equal(new[] { 79, 98, }, monkeys[0].Items);
		Assert.Equal(new[] { 74, }, monkeys[3].Items);
		monkeys.PlayTurn(monkeyIndex: 0);
		Assert.Equal(new[] { 98, }, monkeys[0].Items);
		Assert.Equal(new[] { 74, 500, }, monkeys[3].Items);
		monkeys.PlayTurn(monkeyIndex: 0);
		Assert.Empty(monkeys[0].Items);
		Assert.Equal(new[] { 74, 500, 620, }, monkeys[3].Items);
		monkeys.PlayTurn(monkeyIndex: 1);
		Assert.Equal(new[] { 20, }, monkeys[0].Items);
		Assert.Equal(new[] { 65, 75, 74, }, monkeys[1].Items);
		monkeys.PlayTurn(monkeyIndex: 1);
		Assert.Equal(new[] { 20, 23, }, monkeys[0].Items);
		Assert.Equal(new[] { 75, 74, }, monkeys[1].Items);
		monkeys.PlayTurn(monkeyIndex: 1);
		Assert.Equal(new[] { 20, 23, 27, }, monkeys[0].Items);
		Assert.Equal(new[] { 74, }, monkeys[1].Items);
		monkeys.PlayTurn(monkeyIndex: 1);
		Assert.Equal(new[] { 20, 23, 27, 26, }, monkeys[0].Items);
		Assert.Empty(monkeys[1].Items);
	}

	[Theory]
	[InlineData(_sampleInput)]
	public void PlayRounds(string text)
	{
		// Arrange
		var monkeys = Monkeys.Parse(text, default);

		// Act : round 1
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 20, 23, 27, 26, }, monkeys[0].Items);
		Assert.Equal(new[] { 2_080, 25, 167, 207, 401, 1_046, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 2
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 695, 10, 71, 135, 350, }, monkeys[0].Items);
		Assert.Equal(new[] { 43, 49, 58, 55, 362, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 3
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 16, 18, 21, 20, 122, }, monkeys[0].Items);
		Assert.Equal(new[] { 1_468, 22, 150, 286, 739, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 4
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 491, 9, 52, 97, 248, 34, }, monkeys[0].Items);
		Assert.Equal(new[] { 39, 45, 43, 258, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 5
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 15, 17, 16, 88, 1_037, }, monkeys[0].Items);
		Assert.Equal(new[] { 20, 110, 205, 524, 72, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 6
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 8, 70, 176, 26, 34, }, monkeys[0].Items);
		Assert.Equal(new[] { 481, 32, 36, 186, 2_190, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 7
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 162, 12, 14, 64, 732, 17, }, monkeys[0].Items);
		Assert.Equal(new[] { 148, 372, 55, 72, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 8
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 51, 126, 20, 26, 136, }, monkeys[0].Items);
		Assert.Equal(new[] { 343, 26, 30, 1546, 36, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 9
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 116, 10, 12, 517, 14, }, monkeys[0].Items);
		Assert.Equal(new[] { 108, 267, 43, 55, 288, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 10
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 91, 16, 20, 98, }, monkeys[0].Items);
		Assert.Equal(new[] { 481, 245, 22, 26, 1092, 30, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 11-15
		monkeys.PlayRound();
		monkeys.PlayRound();
		monkeys.PlayRound();
		monkeys.PlayRound();
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 83, 44, 8, 184, 9, 20, 26, 102, }, monkeys[0].Items);
		Assert.Equal(new[] { 110, 36, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		// Act : round 16-20
		monkeys.PlayRound();
		monkeys.PlayRound();
		monkeys.PlayRound();
		monkeys.PlayRound();
		monkeys.PlayRound();

		// Assert
		Assert.Equal(new[] { 10, 12, 14, 26, 34, }, monkeys[0].Items);
		Assert.Equal(new[] { 245, 93, 53, 199, 115, }, monkeys[1].Items);
		Assert.Empty(monkeys[2].Items);
		Assert.Empty(monkeys[3].Items);

		Assert.Equal(101, monkeys[0].InspectionCount);
		Assert.Equal(95, monkeys[1].InspectionCount);
		Assert.Equal(7, monkeys[2].InspectionCount);
		Assert.Equal(105, monkeys[3].InspectionCount);

		var actual = monkeys.Values.Select(m => m.InspectionCount).OrderDescending().Take(2).Product();

		Assert.Equal(10_605, actual);
	}

	[Theory]
	[InlineData(_sampleInput, 20, 10_605)]
	public void MonkeyBusiness(string text, int rounds, int expected)
	{
		var monkeys = Monkeys.Parse(text, default);
		while (rounds-- > 0)
		{
			monkeys.PlayRound();
		}

		var actual = monkeys.Values.Select(m => m.InspectionCount).OrderDescending().Take(2).Product();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(20, 182_293)]
	public async Task SolvePart1(int rounds, int expected)
	{
		var text = string.Join(Environment.NewLine, await base.GetInputAsync().ToArrayAsync());
		var monkeys = Monkeys.Parse(text, default);
		while (rounds-- > 0)
		{
			monkeys.PlayRound();
		}

		var actual = monkeys.Values.Select(m => m.InspectionCount).OrderDescending().Take(2).Product();
		Assert.Equal(expected, actual);
	}

	private record class Monkeys(IDictionary<int, Monkey> Dictionary)
		: IParsable<Monkeys>, IReadOnlyDictionary<int, Monkey>
	{
		public void PlayRound()
		{
			foreach (var (idx, monkey) in this)
			{
				while (monkey.Items.Any())
				{
					PlayTurn(idx);
				}
			}
		}

		public void PlayTurn(int monkeyIndex)
		{
			var monkey = this[monkeyIndex];
			var worryLevel = monkey.Items.Dequeue();
			monkey.InspectionCount++;
			worryLevel = monkey.Operation(worryLevel);
			worryLevel /= 3;
			var destination = (worryLevel % monkey.DivisbleBy) == 0
				? monkey.TrueRoute
				: monkey.FalseRoute;
			this[destination].Items.Enqueue(worryLevel);
		}

		#region ireadonlydictionary implementation
		public Monkey this[int key] => Dictionary[key];
		public IEnumerable<int> Keys => Dictionary.Keys;
		public IEnumerable<Monkey> Values => Dictionary.Values;
		public int Count => Dictionary.Count;
		public bool ContainsKey(int key) => Dictionary.ContainsKey(key);
		public IEnumerator<KeyValuePair<int, Monkey>> GetEnumerator() => Dictionary.GetEnumerator();
		public bool TryGetValue(int key, [MaybeNullWhen(false)] out Monkey value) => Dictionary.TryGetValue(key, out value);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion ireadonlydictionary implementation

		#region iparseable implementation
		public static Monkeys Parse(string s, IFormatProvider? provider)
		{
			var dictionary = new Dictionary<int, Monkey>();

			foreach (var text in s.Split(Environment.NewLine + Environment.NewLine))
			{
				var monkey = Monkey.Parse(text, provider);
				dictionary.Add(monkey.Index, monkey);
			}

			return new(dictionary);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Monkeys result)
		{
			result = Parse(s!, provider);
			return true;
		}
		#endregion iparseable implementation
	}

	private record class Monkey(int Index, Queue<int> Items, Func<int, int> Operation, int DivisbleBy, int TrueRoute, int FalseRoute)
		: IParsable<Monkey>
	{
		public int InspectionCount { get; set; }

		public static Monkey Parse(string s, IFormatProvider? provider)
		{
			var lines = s.Split(Environment.NewLine);

			var index = int.Parse(lines[0][7..^1]);
			var startingItems = lines[1][18..].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse);
			var items = new Queue<int>(startingItems);
			Func<int, int> operation = lines[2][23] switch
			{
				'*' => (int old) => old * (int.TryParse(lines[2][25..], out var i) ? i : old),
				'+' => (int old) => old + (int.TryParse(lines[2][25..], out var i) ? i : old),
				_ => throw new Exception()
			}; ;
			var divisibleBy = int.Parse(lines[3][21..]);
			var trueRoute = int.Parse(lines[4][29..]);
			var falseRoute = int.Parse(lines[5][30..]);

			return new(index, items, operation, divisibleBy, trueRoute, falseRoute);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Monkey result)
		{
			result = Parse(s!, provider);
			return true;
		}
	}
}
