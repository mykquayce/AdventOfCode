using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day22
	{
		[Theory]
		[InlineData(@"Player 1:
9
2
6
3
1", 9, 2, 6, 3, 1)]
		[InlineData(@"Player 2:
5
8
4
7
10", 5, 8, 4, 7, 10)]
		public void PlayerParseTests(string input, params int[] expected)
		{
			var player = Player.Parse(input);

			Assert.NotEmpty(player.Cards);

			for (var a = 0; a < expected.Length; a++)
			{
				Assert.True(player.Cards.TryDequeue(out var actual));
				Assert.Equal(expected[a], actual);
			}
		}

		[Theory]
		[InlineData(@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10", 306)]
		public void Example1(string input, int expected)
		{
			var game = Game.Parse(input);
			var score = game.PlayGame();
			Assert.Equal(expected, score);
		}

		[Theory]
		[InlineData("day22.txt", 31_754)]
		public async Task Part2(string filename, int expected)
		{
			var input = await filename.ReadAllTextAsync();
			var game = Game.Parse(input);
			var score = game.PlayGame();
			Assert.Equal(expected, score);
		}
	}

	public record Game(IReadOnlyList<Player> Players)
	{
		public int PlayGame()
		{
			while (Players.All(p => p.Cards.Any()))
			{
				var round = new List<byte>();

				foreach (var player in Players)
				{
					var card = player.Cards.TryDequeue(out var b) ? b : throw new Exception();
					round.Add(card);
				}

				var winner = round.IndexOf(round.Max());

				foreach (var card in round.OrderByDescending(b => b))
				{
					Players[winner].Cards.Enqueue(card);
				}
			}

			return Players.Single(p => p.Cards.Any()).Score;
		}

		public static Game Parse(string s)
		{
			var players = s.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(Player.Parse)
				.ToList();

			return new Game(players);
		}
	}

	public record Player(ConcurrentQueue<byte> Cards)
	{
		public int Score
		{
			get
			{
				var multiplier = Cards.Count;
				var seed = 0;
				int accumulator(int sum, byte next) => sum + (multiplier-- * next);
				return Cards.Aggregate(seed, accumulator);
			}
		}

		public static Player Parse(string s)
		{
			var lines = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var numbers = lines[1..].Select(byte.Parse);
			var cards = new ConcurrentQueue<byte>(numbers);
			return new Player(cards);
		}
	}
}
