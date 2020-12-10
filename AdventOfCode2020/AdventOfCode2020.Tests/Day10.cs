using AdventOfCode2020.Tests.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day10
	{
		[Theory]
		[InlineData(
			new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4, },
			new[] { 0, 7, 0, 5, })]
		[InlineData(
			new[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3, },
			new[] { 0, 22, 0, 10, })]
		public void Example1(IEnumerable<int> input, IEnumerable<int> expected)
		{
			var joltages = MakeChain(input).ToList();

			var gaps = GetGaps(joltages).ToEnumerable(@default: 0);

			Assert.Equal(expected, gaps);
		}

		private static IEnumerable<int> MakeChain(IEnumerable<int> input)
		{
			var outlet = 0;
			var device = input.Max() + 3;

			return input.Append(outlet).Append(device);
		}

		public static IDictionary<int, int> GetGaps(IEnumerable<int> items)
		{
			var gaps = new Dictionary<int, int>();

			foreach (var curr in items)
			{
				var next = (from i in items
							where i > curr
							orderby i
							select i
							)
							.FirstOrDefault();

				if (next == default) break;

				var gap = next - curr;

				if (!gaps.TryAdd(gap, 1))
				{
					gaps[gap]++;
				}
			}

			return gaps;
		}

		[Theory]
		[InlineData("day10.txt", 2_516)]
		public async Task Part1(string filename, int expected)
		{
			var input = await filename.ReadLinesAsync(int.Parse).ToListAsync();
			var joltages = MakeChain(input).ToList();
			var gaps = GetGaps(joltages);
			var actual = gaps[1] * gaps[3];
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(
			new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4, },
			new[] { 0, 1, 4, 5, 6, 7, 10, 11, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 5, 6, 7, 10, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 5, 7, 10, 11, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 5, 7, 10, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 6, 7, 10, 11, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 6, 7, 10, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 7, 10, 11, 12, 15, 16, 19, 22, },
			new[] { 0, 1, 4, 7, 10, 12, 15, 16, 19, 22, }
			)]
		public void Example2(IEnumerable<int> input, params int[][] expected)
		{
			var joltages = MakeChain(input).ToList();
			var arrangements = GetPossibleArrangements(joltages);

			Assert.Equal(expected.Length, arrangements.Count);

			for (var a = 0; a < expected.Length; a++)
			{
				var found = false;

				foreach (var arrangement in arrangements)
				{
					try
					{
						Assert.Equal(expected[a], arrangement);
						found = true;
					}
					catch { }
				}

				Assert.True(found);
			}
		}

		[Theory]
		[InlineData(
			new[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3, },
			19_208)]
		public void Example3(int[] input, int expected)
		{
			var joltages = MakeChain(input).ToList();
			var actual = GetPossibleArrangements(joltages).Count;
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(
			new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4, },
			new[] { 0, 1, },
			new[] { 4, 5, 6, 7, },
			new[] { 10, 11, 12, },
			new[] { 15, 16, },
			new[] { 19, },
			new[] { 22, })]
		public void SplitContiguousTest(int[] input, params int[][] expected)
		{
			var values = MakeChain(input).OrderBy(i => i).ToList();

			var actual = SplitContiguous(values).ToList();

			Assert.Equal(expected.Length, actual.Count);
			Assert.Equal(expected, actual);
		}

		private static IEnumerable<IList<int>> SplitContiguous(IEnumerable<int> values)
		{
			var collection = new List<int>();

			foreach (var value in values)
			{
				if (collection.Any()
					&& value > collection.Max() + 1)
				{
					yield return collection;
					collection = new List<int>();
				}

				collection.Add(value);
			}

			yield return collection;
		}

		[Theory]
		[InlineData(
			new[] { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4, }, 8)]
		[InlineData(
			new[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3, },
			19_208)]
		public void Example3_Attempt2(int[] input, int expected)
		{
			var count = GetPossibilitiesCount(input);
			Assert.Equal(expected, count);
		}

		private static int GetPossibilitiesCount(IEnumerable<int> joltages)
		{
			var count = 1;
			f(0);
			return count;

			void f(int curr)
			{
				var idx = 0;
				var possibilities = GetPossibilities(joltages, curr).ToList();

				foreach (var next in possibilities)
				{
					if (idx > 0) count++;
					idx++;
					f(next);
				}
			}
		}

		private static IList<ICollection<int>> GetPossibleArrangements(IEnumerable<int> joltages)
		{
			var max = joltages.Max();

			var arrangements = new List<ICollection<int>>
			{
				new List<int>{0, },
			};

			while (true)
			{
				var arrangement = arrangements.FirstOrDefault(c => c.Max() < max);

				if (arrangement is null)
				{
					return arrangements;
				}

				var copy = new int[arrangement.Count];
				arrangement.CopyTo(copy, arrayIndex: 0);

				var curr = arrangement.Last();
				var possibilities = GetPossibilities(joltages, curr).ToList();

				for (var a = 0; a < possibilities.Count; a++)
				{
					var next = possibilities[a];
					if (a == 0) arrangement.Add(next);
					else arrangements.Add(copy.Append(next).ToList());
				}
			}
		}

		private static IEnumerable<int> GetPossibilities(
			IEnumerable<int> joltages,
			int curr)
		{
			return from j in joltages
				   where j > curr
				   where j <= curr + 3
				   orderby j
				   select j;
		}

		[Theory]
		[InlineData("day10.txt", 296_196_766_695_424)]
		public async Task Part2(string filename, int expected)
		{
			var input = await filename.ReadLinesAsync(int.Parse).ToListAsync();
			var joltages = MakeChain(input).OrderBy(i => i).ToList();
			var count = Solve(joltages);
			Assert.Equal(expected, count);
		}


		[Theory]
		[InlineData(
			new[] { 1, 4, 5, 6, 7, 10, 11, 12, 15, 16, 19, },
			8)]
		[InlineData(
			new[] { 28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38, 39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3, },
			19_208)]
		public void Example4(int[] input, long expected)
		{
			var adapters = MakeChain(input).OrderBy(i => i).ToList();
			var actual = Solve(adapters);
			Assert.Equal(expected, actual);
		}

		private static long Solve(IList<int> items)
		{
			var count = items.Count;
			var possibilities = new long[count];
			possibilities[0] = 1;

			for (var a = 1; a < count; a++)
			{
				var left = items[a];

				for (var b = a - 1; b >= 0; b--)
				{
					var right = items[b];

					if (left - right <= 3)
					{
						possibilities[a] += possibilities[b];
					}
					else break;
				}
			}

			return possibilities.Last();
		}
	}
}
