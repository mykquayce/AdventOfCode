using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode2022.Tests;

public class Day09 : Base
{
	[Theory]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { 0, 1, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { 0, -1, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { 1, 0, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { 1, 1, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { 1, -1, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { -1, 0, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { -1, 1, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { -1, -1, }, new int[2] { 0, 0, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 0, 1, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 0, -1, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 1, 0, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 1, 1, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 1, -1, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { -1, 0, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { -1, 1, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { -1, -1, }, true)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 0, 2, }, false)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 0, -2, }, false)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 1, 2, }, false)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { 1, -2, }, false)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { -1, 2, }, false)]
	[InlineData(new int[2] { 0, 0, }, new int[2] { -1, -2, }, false)]
	public void TouchingTests(int[] leftInts, int[] rightInts, bool expected)
	{
		var left = new Point(x: leftInts[0], y: leftInts[1]);
		var right = new Point(x: rightInts[0], y: rightInts[1]);
		var actual = left.Touching(right);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("R", new int[2] { 1, 0, }, new int[2] { 0, 0, })]
	[InlineData("RR", new int[2] { 2, 0, }, new int[2] { 1, 0, })]
	[InlineData("RRR", new int[2] { 3, 0, }, new int[2] { 2, 0, })]
	[InlineData("RRRR", new int[2] { 4, 0, }, new int[2] { 3, 0, })]
	[InlineData("RRRRU", new int[2] { 4, -1, }, new int[2] { 3, 0, })]
	[InlineData("RRRRUU", new int[2] { 4, -2, }, new int[2] { 4, -1, })]
	[InlineData("RRRRUUU", new int[2] { 4, -3, }, new int[2] { 4, -2, })]
	[InlineData("RRRRUUUU", new int[2] { 4, -4, }, new int[2] { 4, -3, })]
	[InlineData("RRRRUUUUL", new int[2] { 3, -4, }, new int[2] { 4, -3, })]
	[InlineData("RRRRUUUULL", new int[2] { 2, -4, }, new int[2] { 3, -4, })]
	[InlineData("RRRRUUUULLL", new int[2] { 1, -4, }, new int[2] { 2, -4, })]
	[InlineData("RRRRUUUULLLD", new int[2] { 1, -3, }, new int[2] { 2, -4, })]
	[InlineData("RRRRUUUULLLDR", new int[2] { 2, -3, }, new int[2] { 2, -4, })]
	[InlineData("RRRRUUUULLLDRR", new int[2] { 3, -3, }, new int[2] { 2, -4, })]
	[InlineData("RRRRUUUULLLDRRR", new int[2] { 4, -3, }, new int[2] { 3, -3, })]
	[InlineData("RRRRUUUULLLDRRRR", new int[2] { 5, -3, }, new int[2] { 4, -3, })]
	[InlineData("RRRRUUUULLLDRRRRD", new int[2] { 5, -2, }, new int[2] { 4, -3, })]
	[InlineData("RRRRUUUULLLDRRRRDL", new int[2] { 4, -2, }, new int[2] { 4, -3, })]
	[InlineData("RRRRUUUULLLDRRRRDLL", new int[2] { 3, -2, }, new int[2] { 4, -3, })]
	[InlineData("RRRRUUUULLLDRRRRDLLL", new int[2] { 2, -2, }, new int[2] { 3, -2, })]
	[InlineData("RRRRUUUULLLDRRRRDLLLL", new int[2] { 1, -2, }, new int[2] { 2, -2, })]
	[InlineData("RRRRUUUULLLDRRRRDLLLLL", new int[2] { 0, -2, }, new int[2] { 1, -2, })]
	[InlineData("RRRRUUUULLLDRRRRDLLLLLR", new int[2] { 1, -2, }, new int[2] { 1, -2, })]
	[InlineData("RRRRUUUULLLDRRRRDLLLLLRR", new int[2] { 2, -2, }, new int[2] { 1, -2, })]
	public void MoveTests1(string move, int[] expectedHeadInts, int[] expectedTailInts)
	{
		var rope = new Rope();
		foreach (var @char in move)
		{
			Action method = @char switch
			{
				'D' => rope.MoveHeadDown,
				'L' => rope.MoveHeadLeft,
				'R' => rope.MoveHeadRight,
				'U' => rope.MoveHeadUp,
				_ => throw new Exception(),
			};
			method.Invoke();
		}
		Assert.Equal(rope.Head, new Point(x: expectedHeadInts[0], y: expectedHeadInts[1]));
		Assert.Equal(rope.Tail, new Point(x: expectedTailInts[0], y: expectedTailInts[1]));
	}

	[Theory]
	[InlineData("RRRRUUUULLLDRRRRDLLLLLRR", 13)]
	public void CountTests(string move, int expected)
	{
		var rope = new Rope();
		foreach (var @char in move)
		{
			Action method = @char switch
			{
				'D' => rope.MoveHeadDown,
				'L' => rope.MoveHeadLeft,
				'R' => rope.MoveHeadRight,
				'U' => rope.MoveHeadUp,
				_ => throw new Exception(),
			};
			method.Invoke();
		}
		var actual = rope.TailPositions.Distinct().Count();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("RRRRUUUULLLDRRRRDLLLLLRR", "R 4", "U 4", "L 3", "D 1", "R 4", "D 1", "L 5", "R 2")]
	public void FormatInput(string expected, params string[] moves)
	{
		var actual = new string(moves.SelectMany(f).ToArray());
		Assert.Equal(expected, actual);

		static IEnumerable<char> f(string input)
		{
			var move = input[0];
			var count = byte.Parse(input[2..]);
			while (count-- > 0)
			{
				yield return move;
			}
		}
	}

	[Theory]
	[InlineData(6_236)]
	public async Task SolvePart1(int expected)
	{
		var rope = new Rope();
		{
			var moves = base.GetInputAsync().SelectMany(f);

			await foreach (var move in moves)
			{
				Action method = move switch
				{
					'D' => rope.MoveHeadDown,
					'L' => rope.MoveHeadLeft,
					'R' => rope.MoveHeadRight,
					'U' => rope.MoveHeadUp,
					_ => throw new Exception(),
				};
				method.Invoke();
			}
		}
		var actual = rope.TailPositions.Distinct().Count();
		Assert.Equal(expected, actual);

		async static IAsyncEnumerable<char> f(string input)
		{
			var move = input[0];
			var count = byte.Parse(input[2..]);
			while (count-- > 0)
			{
				yield return move;
			}
		}
	}

	private class Rope
	{
		public Rope()
		{
			TailPositions.Add(Tail);
		}

		public Point Head { get; private set; } = new Point(x: 0, y: 0);
		public Point Tail { get; private set; } = new Point(x: 0, y: 0);
		public ICollection<Point> TailPositions { get; } = new List<Point>();

		private void MoveHead(Size size)
		{
			Head += size;
			if (Head.Touching(Tail)) { return; }

			var x = (Head.X - Tail.X) switch
			{
				> 0 => 1,
				0 => 0,
				< 0 => -1,
			};

			var y = (Head.Y - Tail.Y) switch
			{
				> 0 => 1,
				0 => 0,
				< 0 => -1,
			};
			var move = new Size(width: x, height: y);
			Tail += move;
			TailPositions.Add(Tail);
		}

		public void MoveHeadDown() => MoveHead(new Size(width: 0, height: 1));
		public void MoveHeadLeft() => MoveHead(new Size(width: -1, height: 0));
		public void MoveHeadRight() => MoveHead(new Size(width: 1, height: 0));
		public void MoveHeadUp() => MoveHead(new Size(width: 0, height: -1));
	}

	[Theory]
	[InlineData("", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("R", "1,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RR", "2,0", "1,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRR", "3,0", "2,0", "1,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRR", "4,0", "3,0", "2,0", "1,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRU", "4,-1", "3,0", "2,0", "1,0", "0,0", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUU", "4,-2", "4,-1", "3,-1", "2,-1", "1,-1", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUU", "4,-3", "4,-2", "3,-1", "2,-1", "1,-1", "0,0", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUU", "4,-4", "4,-3", "4,-2", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUUL", "3,-4", "4,-3", "4,-2", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULL", "2,-4", "3,-4", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLL", "1,-4", "2,-4", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLD", "1,-3", "2,-4", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDR", "2,-3", "2,-4", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRR", "3,-3", "2,-4", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRR", "4,-3", "3,-3", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRR", "5,-3", "4,-3", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRD", "5,-2", "4,-3", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDL", "4,-2", "4,-3", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDLL", "3,-2", "4,-3", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDLLL", "2,-2", "3,-2", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDLLLL", "1,-2", "2,-2", "3,-3", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDLLLLL", "0,-2", "1,-2", "2,-2", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDLLLLLR", "1,-2", "1,-2", "2,-2", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	[InlineData("RRRRUUUULLLDRRRRDLLLLLRR", "2,-2", "1,-2", "2,-2", "3,-2", "2,-2", "1,-1", "0,0", "0,0", "0,0", "0,0")]
	public void MoveTests2(string moves, params string[] expectedStrings)
	{
		var rope = new LooseRope();

		foreach (var move in moves)
		{
			Action method = move switch
			{
				'D' => rope.MoveHeadDown,
				'L' => rope.MoveHeadLeft,
				'R' => rope.MoveHeadRight,
				'U' => rope.MoveHeadUp,
				_ => throw new Exception(),
			};
			method.Invoke();
		}

		var expected = ParsePointStrings(expectedStrings);
		Assert.Equal(rope.Knots, expected);
	}

	[Theory]
	[InlineData(36, "R 5", "U 8", "L 8", "D 3", "R 17", "D 10", "L 25", "U 20")]
	public void SolveExample2(int expected, params string[] moveStrings)
	{
		var rope = new LooseRope();
		{
			var moves = moveStrings.Select(s => Move.Parse(s, default)).SelectMany(c => c);

			foreach (var move in moves)
			{
				Action method = move switch
				{
					'D' => rope.MoveHeadDown,
					'L' => rope.MoveHeadLeft,
					'R' => rope.MoveHeadRight,
					'U' => rope.MoveHeadUp,
					_ => throw new Exception(),
				};
				method.Invoke();
			}
		}
		var actual = rope.TailPositions.Distinct().Count();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(2_449)]
	public async Task SolvePart2(int expected)
	{
		var rope = new LooseRope();
		{
			var moves = base.GetInputAsync<Move>();
			await foreach (var move in moves)
			{
				foreach (var @char in move)
				{
					Action method = @char switch
					{
						'D' => rope.MoveHeadDown,
						'L' => rope.MoveHeadLeft,
						'R' => rope.MoveHeadRight,
						'U' => rope.MoveHeadUp,
						_ => throw new Exception(),
					};
					method.Invoke();
				}
			}
		}
		var actual = rope.TailPositions.Distinct().Count();
		Assert.Equal(expected, actual);
	}

	private record Move(char Direction, byte Count)
		: IParsable<Move>, IReadOnlyCollection<char>
	{
		int IReadOnlyCollection<char>.Count => Count;
		public IEnumerator<char> GetEnumerator() => Enumerable.Repeat(Direction, Count).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public static Move Parse(string s, IFormatProvider? provider)
		{
			var direction = s[0];
			var count = byte.Parse(s[2..]);
			return new(direction, count);
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Move result)
		{
			result = Parse(s!, provider);
			return true;
		}
	}

	private static IEnumerable<Point> ParsePointStrings(params string[] strings)
	{
		return from s in strings
			   let aa = s.Split(',')
			   let x = int.Parse(aa[0])
			   let y = int.Parse(aa[1])
			   select new Point(x: x, y: y);
	}

	private class LooseRope
	{
		public LooseRope()
		{
			Knots = Enumerable.Repeat(new Point(x: 0, y: 0), count: 10).ToList();
			TailPositions.Add(Tail);
		}

		public IList<Point> Knots { get; }
		public Point Head { get => Knots[0]; set => Knots[0] = value; }
		public Point Tail => Knots[9];
		public ICollection<Point> TailPositions { get; } = new List<Point>();

		private void MoveHead(Size size)
		{
			Head += size;
			for (var a = 0; a < Knots.Count - 1; a++)
			{
				var curr = Knots[a];
				var next = Knots[a + 1];
				if (curr.Touching(next)) { continue; }

				var x = (curr.X - next.X) switch
				{
					> 0 => 1,
					0 => 0,
					< 0 => -1,
				};

				var y = (curr.Y - next.Y) switch
				{
					> 0 => 1,
					0 => 0,
					< 0 => -1,
				};
				var move = new Size(width: x, height: y);
				Knots[a + 1] += move;
				if (a == 8)
				{
					TailPositions.Add(Tail);
				}
			}
		}

		public void MoveHeadDown() => MoveHead(new Size(width: 0, height: 1));
		public void MoveHeadLeft() => MoveHead(new Size(width: -1, height: 0));
		public void MoveHeadRight() => MoveHead(new Size(width: 1, height: 0));
		public void MoveHeadUp() => MoveHead(new Size(width: 0, height: -1));
	}
}
