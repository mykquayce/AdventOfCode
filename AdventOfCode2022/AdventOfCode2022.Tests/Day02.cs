using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022.Tests;

public class Day02 : Base
{
	[Theory]
	[InlineData("A Y", 8)]
	[InlineData("B X", 1)]
	[InlineData("C Z", 6)]
	public void ParseInput(string input, int expected)
	{
		var game = Game.Parse(input, default);
		var actual = game.Score;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(15, "A Y", "B X", "C Z")]
	public void SolveExample1(int expected, params string[] inputs)
	{
		int actual = 0;
		foreach (var input in inputs)
		{
			var game = Game.Parse(input, default);
			actual += game.Score;
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(12_276)]
	public async Task SolvePart1(int expected)
	{
		int actual = 0;
		var games = base.GetInputAsync<Game>();
		await foreach (var game in games)
		{
			actual += game.Score;
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("A Y", 4)]
	[InlineData("B X", 1)]
	[InlineData("C Z", 7)]
	public void ParseRevisedInput(string input, int expected)
	{
		var game = RevisedGame.Parse(input, default);
		var actual = game.Score;
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(12, "A Y", "B X", "C Z")]
	public void SolveExample2(int expected, params string[] inputs)
	{
		int actual = 0;
		foreach (var input in inputs)
		{
			var game = RevisedGame.Parse(input, default);
			actual += game.Score;
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(9_975)]
	public async Task SolvePart2(int expected)
	{
		int actual = 0;
		var games = base.GetInputAsync<RevisedGame>();
		await foreach (var game in games)
		{
			actual += game.Score;
		}
		Assert.Equal(expected, actual);
	}

	private enum Move
	{
		Rock = 1,
		Paper = 2,
		Scissors = 3,
	}

	private enum Outcome
	{
		Lose = 1,
		Draw = 2,
		Win = 3,
	}

	private record struct Game(Move Opponent, Move Me)
		: IParsable<Game>
	{
		public int Score
		{
			get
			{
				return (Opponent, Me) switch
				{
					(Move.Rock, Move.Rock) => 1 + 3,
					(Move.Rock, Move.Paper) => 2 + 6,
					(Move.Rock, Move.Scissors) => 3 + 0,
					(Move.Paper, Move.Rock) => 1 + 0,
					(Move.Paper, Move.Paper) => 2 + 3,
					(Move.Paper, Move.Scissors) => 3 + 6,
					(Move.Scissors, Move.Rock) => 1 + 6,
					(Move.Scissors, Move.Paper) => 2 + 0,
					(Move.Scissors, Move.Scissors) => 3 + 3,
					_ => throw new Exception(),
				};
			}
		}

		public static Game Parse(string s, IFormatProvider? provider)
		{
			var opponent = s[0] switch
			{
				'A' => Move.Rock,
				'B' => Move.Paper,
				'C' => Move.Scissors,
				_ => throw new Exception(),
			};

			var me = s[2] switch
			{
				'X' => Move.Rock,
				'Y' => Move.Paper,
				'Z' => Move.Scissors,
				_ => throw new Exception(),
			};

			return new(opponent, me);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Game result)
		{
			result = Parse(s!, provider);
			return true;
		}
	}

	private record struct RevisedGame(Move Opponent, Outcome Outcome)
		: IParsable<RevisedGame>
	{
		public int Score
		{
			get
			{
				return (Opponent, Outcome) switch
				{
					(Move.Rock, Outcome.Lose) => 3 + 0,
					(Move.Rock, Outcome.Draw) => 1 + 3,
					(Move.Rock, Outcome.Win) => 2 + 6,
					(Move.Paper, Outcome.Lose) => 1 + 0,
					(Move.Paper, Outcome.Draw) => 2 + 3,
					(Move.Paper, Outcome.Win) => 3 + 6,
					(Move.Scissors, Outcome.Lose) => 2 + 0,
					(Move.Scissors, Outcome.Draw) => 3 + 3,
					(Move.Scissors, Outcome.Win) => 1 + 6,
					_ => throw new Exception(),
				};
			}
		}

		public static RevisedGame Parse(string s, IFormatProvider? provider)
		{
			var opponent = s[0] switch
			{
				'A' => Move.Rock,
				'B' => Move.Paper,
				'C' => Move.Scissors,
				_ => throw new Exception(),
			};

			var outcome = s[2] switch
			{
				'X' => Outcome.Lose,
				'Y' => Outcome.Draw,
				'Z' => Outcome.Win,
				_ => throw new Exception(),
			};

			return new(opponent, outcome);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RevisedGame result)
		{
			result = Parse(s!, provider);
			return true;
		}
	}
}
