using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day11
	{
		private const string _exampleLayout = @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";

		[Theory]
		[InlineData(@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL")]
		public void AsynmmetricTests(string s)
		{
			var layout = Layout.Parse(s);
			layout.Go();
		}

		[Theory]
		[InlineData(_exampleLayout)]
		public void ParseInputTests(string s)
		{
			var layout = Layout.Parse(s);

			Assert.Equal(Layout.EmptyChair, layout[0, 0]);
			Assert.Equal(Layout.Floor, layout[0, 1]);
			Assert.Equal(Layout.EmptyChair, layout[0, 2]);

			Assert.Equal(s, layout.ToString());
		}

		[Theory]
		[InlineData(_exampleLayout, 0, 0, '.', 'L', 'L')]
		[InlineData(_exampleLayout, 0, 1, 'L', 'L', 'L', 'L', 'L')]
		[InlineData(_exampleLayout, 1, 1, 'L', '.', 'L', 'L', 'L', 'L', '.', 'L')]
		[InlineData(_exampleLayout, 0, 9, 'L', 'L', 'L')]
		public void GetAdjacentTest(string s, int row, int col, params char[] expected)
		{
			var layout = Layout.Parse(s);
			var adjacent = layout.GetAdjacent(row, col).ToArray();
			Assert.Equal(expected, adjacent);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##")]
		public void Round1Test(string s, string expected)
		{
			var layout = Layout.Parse(s);

			layout = layout.Go();

			var actual = layout.ToString();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##")]
		public void Round2Test(string s, string expected)
		{
			var layout = Layout.Parse(s);

			layout = layout.Go().Go();

			var actual = layout.ToString();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##")]
		public void Round3Test(string s, string expected)
		{
			var layout = Layout.Parse(s);

			layout = layout.Go().Go().Go();

			var actual = layout.ToString();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##")]
		public void Round4Test(string s, string expected)
		{
			var layout = Layout.Parse(s);

			layout = layout.Go().Go().Go().Go();

			var actual = layout.ToString();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##")]
		public void Round5Test(string s, string expected)
		{
			var layout = Layout.Parse(s);

			layout = layout.Go().Go().Go().Go().Go();

			var actual = layout.ToString();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, 37)]
		public void GetStabilizedTest(string s, int expected)
		{
			var layout = Layout.Parse(s);
			var stabilized = GetStabilized(layout);
			Assert.Equal(expected, stabilized.OccupiedCount);
		}

		private static Layout GetStabilized(Layout layout)
		{
			Layout next;

			while (true)
			{
				next = layout.Go();
				if (next.ToString() == layout.ToString()) return layout;
				layout = next;
			}
		}

		private static Layout GetRevizedStabilized(Layout layout)
		{
			Layout next;

			while (true)
			{
				next = layout.Go2();
				if (next.ToString() == layout.ToString()) return layout;
				layout = next;
			}
		}

		[Theory]
		[InlineData("day11.txt", 2_359)]
		public async Task Part1(string filename, int expected)
		{
			var s = await filename.ReadAllTextAsync();
			var layout = Layout.Parse(s);
			var stabilized = GetStabilized(layout);
			Assert.Equal(expected, stabilized.OccupiedCount);
		}

		[Theory]
		[InlineData(@".......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....",
			4, 3,
			Layout.OccupiedChair, Layout.OccupiedChair, Layout.OccupiedChair, Layout.OccupiedChair, Layout.OccupiedChair, Layout.OccupiedChair, Layout.OccupiedChair, Layout.OccupiedChair)]
		[InlineData(@".............
.L.L.#.#.#.#.
.............",
			1, 1,
			'\0', '\0', Layout.EmptyChair, '\0', '\0', '\0', '\0', '\0')]
		[InlineData(@".##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.",
			3, 3,
			'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0')]
		[InlineData(@"#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#",
			2, 7,
			'\0', Layout.EmptyChair, '\0', Layout.EmptyChair, Layout.EmptyChair, Layout.EmptyChair, Layout.EmptyChair, Layout.EmptyChair)]
		public void GetVisibleTests(string s, int row, int col, params char[] expected)
		{
			var layout = Layout.Parse(s);
			var visible = layout.GetVisible(row, col).ToList();

			Assert.Equal(expected[0], visible[0]);
			Assert.Equal(expected[1], visible[1]);
			Assert.Equal(expected[2], visible[2]);
			Assert.Equal(expected[3], visible[3]);
			Assert.Equal(expected[4], visible[4]);
			Assert.Equal(expected[5], visible[5]);
			Assert.Equal(expected[6], visible[6]);
			Assert.Equal(expected[7], visible[7]);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##")]
		public void Part2_Round1(string s, string expected)
		{
			var layout = Layout.Parse(s);
			var outcome = layout.Go2();
			var actual = outcome.ToString();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#")]
		public void Part2_Round2(string s, string expected)
		{
			var layout = Layout.Parse(s);
			var outcome = layout.Go2().Go2();
			var actual = outcome.ToString();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.L#.##.L#
#L#####.LL
L.#.#..#..
##L#.##.##
#.##.#L.##
#.#####.#L
..#.#.....
LLL####LL#
#.L#####.L
#.L####.L#")]
		public void Part2_Round3(string s, string expected)
		{
			var layout = Layout.Parse(s);
			var outcome = layout.Go2().Go2().Go2();
			var actual = outcome.ToString();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##LL.LL.L#
L.LL.LL.L#
#.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLL#.L
#.L#LL#.L#")]
		public void Part2_Round4(string s, string expected)
		{
			var layout = Layout.Parse(s);
			var outcome = layout.Go2().Go2().Go2().Go2();
			var actual = outcome.ToString();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.#L.L#
#.L####.LL
..#.#.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#")]
		public void Part2_Round5(string s, string expected)
		{
			var layout = Layout.Parse(s);
			var outcome = layout.Go2().Go2().Go2().Go2().Go2();
			var actual = outcome.ToString();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, @"#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.LL.L#
#.LLLL#.LL
..#.L.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#")]
		public void Part2_Round6(string s, string expected)
		{
			var layout = Layout.Parse(s);
			var outcome = layout.Go2().Go2().Go2().Go2().Go2().Go2();
			var actual = outcome.ToString();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleLayout, 26)]
		public void Part2_GetStabilizedTest(string s, int expected)
		{
			var layout = Layout.Parse(s);
			var stabilized = GetRevizedStabilized(layout);
			var occupied = stabilized.OccupiedCount;
			Assert.Equal(expected, occupied);
		}

		[Theory]
		[InlineData("day11.txt", 2_131)]
		public async Task Part2(string filename, int expected)
		{
			var s = await filename.ReadAllTextAsync();
			var layout = Layout.Parse(s);
			var stabilized = GetRevizedStabilized(layout);
			var occupied = stabilized.OccupiedCount;
			Assert.Equal(expected, occupied);
		}
	}

	public record Layout(string[] Rows)
	{
		public const char Floor = '.';
		public const char EmptyChair = 'L';
		public const char OccupiedChair = '#';

		public char this[int row, int col]
		{
			get => Rows[row][col];
			set
			{
				var cc = Rows[row].ToCharArray();
				cc[col] = value;
				Rows[row] = new string(cc, startIndex: 0, length: ColCount);
			}
		}
		public int RowCount => Rows.Length;
		public int ColCount => Rows[0].Length;
		public int OccupiedCount => Rows.Sum(s => s.Count(c => c == OccupiedChair));
		public IEnumerable<char> GetRow(int row) => Enumerable.Range(0, ColCount).Select(col => this[row, col]);

		private IDictionary<Point, char>? _seats;
		public IDictionary<Point, char> Seats => _seats ??= new Dictionary<Point, char>(GetSeats());

		public IEnumerable<KeyValuePair<Point, char>> GetSeats()
		{
			for (var row = 0; row < RowCount; row++)
			{
				for (var col = 0; col < ColCount; col++)
				{
					var value = this[row, col];
					if (value != Floor)
					{
						var key = new Point(col, row);
						yield return new KeyValuePair<Point, char>(key, value);
					}
				}
			}
		}

		public Layout Go()
		{
			var clone = Parse(ToString());

			for (var col = 0; col < ColCount; col++)
			{
				for (var row = 0; row < RowCount; row++)
				{
					var seat = this[row, col];

					if (seat == Floor) continue;

					var adjacents = GetAdjacent(row, col).ToList();

					if (seat == EmptyChair
						&& adjacents.All(a => a != OccupiedChair))
					{
						clone.Occupy(row, col);
					}
					else if (seat == OccupiedChair
						&& adjacents.Count(a => a == OccupiedChair) >= 4)
					{
						clone.Vacate(row, col);
					}
				}
			}

			return clone;
		}

		public Layout Go2()
		{
			var clone = Parse(ToString());

			for (var col = 0; col < ColCount; col++)
			{
				for (var row = 0; row < RowCount; row++)
				{
					var seat = this[row, col];

					if (seat == Floor) continue;

					var visible = GetVisible(row, col).ToList();

					if (seat == EmptyChair
						&& visible.All(a => a != OccupiedChair))
					{
						clone.Occupy(row, col);
					}
					else if (seat == OccupiedChair
						&& visible.Count(a => a == OccupiedChair) >= 5)
					{
						clone.Vacate(row, col);
					}
				}
			}

			return clone;
		}

		public void Occupy(int row, int col) => this[row, col] = '#';
		public void Vacate(int row, int col) => this[row, col] = 'L';

		public IEnumerable<char> GetAdjacent(int row, int col)
		{
			var xx = Enumerable.Range(col - 1, 3);
			var yy = Enumerable.Range(row - 1, 3);

			foreach (var y in yy)
			{
				if (y < 0 || y >= RowCount) continue;

				foreach (var x in xx)
				{
					if (x < 0 || x >= ColCount) continue;
					if (x == col && y == row) continue;
					yield return this[y, x];
				}
			}
		}

		public IEnumerable<char> GetSeatsUp(int row, int col) => Seats.Where(kvp => kvp.Key.X == col && kvp.Key.Y < row).OrderByDescending(kvp => kvp.Key.Y).GetValues();
		public IEnumerable<char> GetSeatsRigntAndUp(int row, int col) => Seats.Where(kvp => kvp.Key.X > col && kvp.Key.Y < row && Math.Abs(kvp.Key.X - col) == Math.Abs(kvp.Key.Y - row)).OrderBy(kvp => kvp.Key.X).GetValues();
		public IEnumerable<char> GetSeatsRight(int row, int col) => Seats.Where(kvp => kvp.Key.X > col && kvp.Key.Y == row).GetValues();
		public IEnumerable<char> GetSeatsRightAndDown(int row, int col) => Seats.Where(kvp => kvp.Key.X > col && kvp.Key.Y > row && Math.Abs(kvp.Key.X - col) == Math.Abs(kvp.Key.Y - row)).OrderBy(kvp => kvp.Key.X).GetValues();
		public IEnumerable<char> GetSeatsDown(int row, int col) => Seats.Where(kvp => kvp.Key.X == col && kvp.Key.Y > row).GetValues();
		public IEnumerable<char> GetSeatsDownAndLeft(int row, int col) => Seats.Where(kvp => kvp.Key.X < col && kvp.Key.Y > row && Math.Abs(kvp.Key.X - col) == Math.Abs(kvp.Key.Y - row)).OrderBy(kvp => kvp.Key.Y).GetValues();
		public IEnumerable<char> GetSeatsLeft(int row, int col) => Seats.Where(kvp => kvp.Key.X < col && kvp.Key.Y == row).OrderByDescending(kvp => kvp.Key.X).GetValues();
		public IEnumerable<char> GetSeatsLeftAndUp(int row, int col) => Seats.Where(kvp => kvp.Key.X < col && kvp.Key.Y < row && Math.Abs(kvp.Key.X - col) == Math.Abs(kvp.Key.Y - row)).OrderByDescending(kvp => kvp.Key.Y).GetValues();

		public IEnumerable<char> GetVisible(int row, int col)
		{
			yield return GetSeatsUp(row, col).FirstOrDefault();
			yield return GetSeatsRigntAndUp(row, col).FirstOrDefault();
			yield return GetSeatsRight(row, col).FirstOrDefault();
			yield return GetSeatsRightAndDown(row, col).FirstOrDefault();
			yield return GetSeatsDown(row, col).FirstOrDefault();
			yield return GetSeatsDownAndLeft(row, col).FirstOrDefault();
			yield return GetSeatsLeft(row, col).FirstOrDefault();
			yield return GetSeatsLeftAndUp(row, col).FirstOrDefault();
		}

		public static Layout Parse(string s)
		{
			var rows = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			return new Layout(rows);
		}

		public override string ToString() => string.Join(Environment.NewLine, Rows);
	}
}
