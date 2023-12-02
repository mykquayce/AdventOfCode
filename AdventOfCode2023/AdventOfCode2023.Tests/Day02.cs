using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Tests;

public partial class Day02
{
	[Theory]
	[InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 4, 2, 6)]
	[InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 1, 3, 4)]
	[InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 20, 13, 6)]
	[InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 14, 3, 15)]
	[InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 6, 3, 2)]
	public void ParseInput(string input, int expectedMaxRed, int expectedMaxGreen, int expectedMaxBlue)
	{
		var game = Game.Parse(input, null);
		Assert.Equal(expectedMaxRed, game.MaxRed);
		Assert.Equal(expectedMaxGreen, game.MaxGreen);
		Assert.Equal(expectedMaxBlue, game.MaxBlue);
	}

	[Theory]
	[InlineData("3 blue, 4 red", 4, 0, 3)]
	[InlineData("1 red, 2 green, 6 blue", 1, 2, 6)]
	[InlineData("2 green", 0, 2, 0)]
	public void ParseReveal(string input, int expectedRed, int expectedGreen, int expectedBlue)
	{
		var reveal = Game.Reveal.Parse(input, null);

		Assert.Equal(expectedBlue, reveal.Blue);
		Assert.Equal(expectedGreen, reveal.Green);
		Assert.Equal(expectedRed, reveal.Red);
	}

	[Theory, InlineData(12, 13, 14, 8)]
	public void Example1(int red, int green, int blue, int expected)
	{
		var inputs = new string[5]
		{
			"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
			"Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
			"Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
			"Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
			"Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
		};

		var games = inputs.Select(s => Game.Parse(s, null));
		var actual = SolvePart1(red, green, blue, games);

		Assert.Equal(expected, actual);
	}

	private static int SolvePart1(int red, int green, int blue, IEnumerable<Game> games)
	{
		var total = 0;
		foreach (var game in games)
		{
			if (game.MaxRed <= red && game.MaxGreen <= green && game.MaxBlue <= blue)
			{
				total += game.Id;
			}
		}
		return total;
	}

	[Theory, InlineData(2_006)]
	public async Task Part1(int expected)
	{
		var actual = 0;
		string? line;
		using var reader = new StreamReader(path: Path.Combine(".", "Data", "day02.txt"));
		while ((line = await reader.ReadLineAsync()) is not null)
		{
			var game = Game.Parse(line, null);
			if (game.MaxRed <= 12 && game.MaxGreen <= 13 && game.MaxBlue <= 14)
			{
				actual += game.Id;
			}
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 48)]
	[InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 12)]
	[InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 1_560)]
	[InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 630)]
	[InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 36)]
	public void Example2_1(string input, int expected)
	{
		var game = Game.Parse(input, null);
		var actual = game.MaxRed * game.MaxGreen * game.MaxBlue;
		Assert.Equal(expected, actual);
	}

	[Theory, InlineData(2_286)]
	public void Example2_21(int expected)
	{
		var inputs = new string[5]
		{
			"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
			"Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
			"Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
			"Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
			"Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
		};

		var powers = from s in inputs
					 let g = Game.Parse(s, null)
					 let p = g.MaxRed * g.MaxGreen * g.MaxBlue
					 select p;

		Assert.Equal(expected, powers.Sum());
	}

	[Theory, InlineData(84_911)]
	public async Task Part2(int expected)
	{
		var actual = 0;
		string? line;
		using var reader = new StreamReader(path: Path.Combine(".", "Data", "day02.txt"));
		while ((line = await reader.ReadLineAsync()) is not null)
		{
			var game = Game.Parse(line, null);
			actual += game.MaxRed * game.MaxGreen * game.MaxBlue;
		}
		Assert.Equal(expected, actual);
	}

	private readonly partial record struct Game(int Id, Game.Reveal[] Reveals)
		: IParsable<Game>
	{
		public int MaxRed { get; } = Reveals.Max(x => x.Red);
		public int MaxGreen { get; } = Reveals.Max(x => x.Green);
		public int MaxBlue { get; } = Reveals.Max(x => x.Blue);

		public static Game Parse(string s, IFormatProvider? provider)
		{
			var match = GameRegex().Match(s);

			var id = int.Parse(match.Groups[1].Value);
			var reveals = match.Groups[2].Value
				.Split(';')
				.Select(s => s.Trim())
				.Select(s => Reveal.Parse(s, null))
				.ToArray();

			return new(id, reveals);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Game result)
		{
			throw new NotImplementedException();
		}

		public readonly partial record struct Reveal(int Red, int Green, int Blue)
			: IParsable<Reveal>
		{
			public static Reveal Parse(string s, IFormatProvider? provider)
			{
				int red = 0, green = 0, blue = 0;

				foreach (Match match in RevealRegex().Matches(s))
				{
					var count = int.Parse(match.Groups[1].Value);
					var color = match.Groups[2].Value;
					switch (color)
					{
						case "red":
							red = count;
							break;
						case "green":
							green = count;
							break;
						case "blue":
							blue = count;
							break;
						default:
							throw new Exception();
					}
				}

				return new(red, green, blue);
			}

			public static bool TryParse(string? s, IFormatProvider? provider, out Reveal result)
			{
				throw new NotImplementedException();
			}
		}

		[GeneratedRegex(@"^Game (\d+): (.+)$")]
		private static partial Regex GameRegex();

		[GeneratedRegex(@"(\d+) (red|green|blue)")]
		private static partial Regex RevealRegex();
	}
}
