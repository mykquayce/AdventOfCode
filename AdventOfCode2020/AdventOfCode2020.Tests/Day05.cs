using AdventOfCode2020.Tests.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day05
	{
		[Theory]
		[InlineData("FBFBBFF", 44)]
		[InlineData("BFFFBBF", 70)]
		[InlineData("FFFBBBF", 14)]
		[InlineData("BBFFBBF", 102)]
		public void GetRowTests(string input, int expected)
		{
			var actual = GetRow(input);

			Assert.Equal(expected, actual);
		}

		private static int GetRow(string input)
		{
			float min = 0, max = 127;

			foreach (var c in input)
			{
				(min, max) = Partition(min, max, c == 'F');
			}

			return (int)max;
		}

		[Theory]
		[InlineData("RLR", 5)]
		[InlineData("RRR", 7)]
		[InlineData("RLL", 4)]
		public void GetColumnTests(string input, int expected)
		{
			var actual = GetColumn(input);

			Assert.Equal(expected, actual);
		}

		private static int GetColumn(string input)
		{
			float min = 0, max = 7;

			foreach (var c in input)
			{
				(min, max) = Partition(min, max, c == 'L');
			}

			return (int)max;
		}

		private static (float, float) Partition(float min, float max, bool lower)
		{
			var mid = min + ((max - min) / 2f);

			if (lower)
			{
				return (min, mid);
			}

			return (mid, max);
		}

		[Theory]
		[InlineData("BFFFBBFRRR", 567)]
		[InlineData("FFFBBBFRRR", 119)]
		[InlineData("BBFFBBFRLL", 820)]
		public void ParseSeatTests(string input, int expected)
		{
			var actual = GetSeat(input);

			Assert.Equal(expected, actual);
		}

		private static int GetSeat(string input)
		{
			var row = GetRow(input[..7]);
			var column = GetColumn(input[7..]);

			return (row * 8) + column;
		}

		[Theory]
		[InlineData("day05.txt", 871)]
		public async Task Part1(string filename, int expected)
		{
			var max = int.MinValue;

			var indices = filename.ReadLinesAsync(GetSeat);

			await foreach (var index in indices)
			{
				if (index > max) max = index;
			}

			Assert.Equal(expected, max);
		}

		[Theory]
		[InlineData("day05.txt", 640)]
		public async Task Part2(string filename, int expected)
		{
			var max = (127 * 8) + 8;
			var seats = new bool[max];

			var indices = filename.ReadLinesAsync(GetSeat);

			await foreach (var index in indices)
			{
				seats[index] = true;
			}

			var seat = -1;

			for (var a = 1; a < max - 1; a++)
			{
				if (seats[a] == false
					&& seats[a - 1] == true
					&& seats[a + 1] == true)
				{
					seat = a;
					break;
				}
			}

			Assert.Equal(expected, seat);
		}
	}
}
