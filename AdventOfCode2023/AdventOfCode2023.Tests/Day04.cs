using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Tests;

public partial class Day04
{
	[Theory]
	[InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53")]
	[InlineData("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19")]
	[InlineData("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1")]
	[InlineData("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83")]
	[InlineData("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36")]
	[InlineData("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11")]
	public void ParseInputTests(string input)
	{
		var (_, winningNumbers, myNumbers) = Card.Parse(input, null);

		Assert.NotEmpty(winningNumbers);
		Assert.DoesNotContain(default, winningNumbers);
		Assert.NotEmpty(myNumbers);
		Assert.DoesNotContain(default, myNumbers);
	}

	[Theory]
	[InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", new byte[4] { 17, 48, 83, 86, })]
	[InlineData("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19", new byte[2] { 32, 61, })]
	[InlineData("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1", new byte[2] { 1, 21, })]
	[InlineData("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83", new byte[1] { 84, })]
	[InlineData("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36", new byte[0] { })]
	[InlineData("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", new byte[0] { })]
	public void OverlapTests(string input, byte[] expected)
	{
		var actual = Card.Parse(input, null).GetOverlap().ToArray();
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(13,
		"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
		"Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
		"Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
		"Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
		"Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
		"Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11")]
	public void SolveExample1(int expected, params string[] inputs)
	{
		var query = from card in inputs.Select(s => Card.Parse(s, null))
					let points = card.GetPoints()
					select points;
		var actual = query.Sum();
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(23_673)]
	public async Task SolvePart1(int expected)
	{
		var actual = 0;
		var path = Path.Combine(".", "Data", "day04.txt");
		var lines = File.ReadLinesAsync(path);
		await foreach (var line in lines)
		{
			var card = Card.Parse(line, null);
			var points = card.GetPoints();
			actual += points;
		}
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(30,
		"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
		"Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
		"Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
		"Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
		"Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
		"Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11")]
	public void SolveExample2Tests(int expected, params string[] inputs)
	{
		var actual = SolveExample2(inputs).Sum();
		Assert.Equal(expected, actual);
	}

	private static ICollection<int> SolveExample2(ICollection<string> inputs)
	{
		var dictionary = Enumerable.Range(start: 1, count: inputs.Count)
			.ToDictionary(i => i, _ => 1);

		foreach (var input in inputs)
		{
			var card = Card.Parse(input, null);
			var count = card.GetOverlap().Count();
			foreach (byte i in Enumerable.Range(start: card.Index + 1, count: count))
			{
				dictionary[i] += dictionary[card.Index];
			}
		}

		return dictionary.Values;
	}

	[Theory, InlineData(12_263_631)]
	public async Task SolvePart2(int expected)
	{
		var path = Path.Combine(".", "Data", "day04.txt");
		var inputs = await File.ReadLinesAsync(path).ToArrayAsync();
		var actual = SolveExample2(inputs).Sum();
		Assert.Equal(expected, actual);
	}

	private readonly partial record struct Card(byte Index, IReadOnlyCollection<byte> WinningNumbers, IReadOnlyCollection<byte> MyNumbers) : IParsable<Card>
	{
		public IEnumerable<byte> GetOverlap()
		{
			return from left in WinningNumbers
				   join right in MyNumbers on left equals right
				   orderby left
				   select left;
		}

		public int GetPoints() => (int)(Math.Pow(2, GetOverlap().Count()) / 2d);

		public static Card Parse(string s, IFormatProvider? provider)
		{
			var match = InputRegex().Match(s);
			var index = byte.Parse(match.Groups[1].Value);
			var winningNumbers = getvalues<byte>(match.Groups[2].Value);
			var myNumbers = getvalues<byte>(match.Groups[3].Value);

			return new(index, winningNumbers, myNumbers);

			static IReadOnlyCollection<T> getvalues<T>(string s, IFormatProvider? provider = null) where T : IParsable<T>
			{
				return s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(s => T.Parse(s, provider))
					.ToArray();
			}
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Card result)
		{
			throw new NotImplementedException();
		}

		[GeneratedRegex(@"^Card\s+(\d+): ([\d\s]+) \| ([\d\s]+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.NonBacktracking, matchTimeoutMilliseconds: 100)]
		private static partial Regex InputRegex();
	}
}
