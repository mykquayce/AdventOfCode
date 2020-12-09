using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day09
	{
		[Theory]
		[InlineData(new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }, 26, true)]
		[InlineData(new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }, 49, true)]
		[InlineData(new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }, 100, false)]
		[InlineData(new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }, 50, false)]
		public void Example1(ICollection<long> preamble, long result, bool expected)
		{
			var actual = IsValid(preamble, result);

			Assert.Equal(expected, actual);
		}

		private static bool IsValid(IEnumerable<long> preamble, long answer)
		{
			return (from a in preamble
					from b in preamble
					where a != b
					where a + b == answer
					select 1
						).Any();
		}

		[Theory]
		[InlineData(new long[] { 35, 20, 15, 25, 47, 40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576 }, 127)]
		public void Example2(IEnumerable<long> numbers, short expected)
		{
			using var enumerator = numbers.GetEnumerator();
			var actual = FindWeakness(enumerator, 5);
			Assert.Equal(expected, actual);
		}

		private static long FindWeakness(IEnumerator<long> enumerator, byte preambleLength)
		{
			var preamble = new Queue<long>(capacity: preambleLength);

			for (var a = 0; a < preambleLength; a++)
			{
				enumerator.MoveNext();
				var value = enumerator.Current;
				preamble.Enqueue(value);
			}

			while (enumerator.MoveNext())
			{
				var value = enumerator.Current;

				if (!IsValid(preamble, value))
				{
					return value;
				}

				preamble.Enqueue(value);
				preamble.Dequeue();
			}

			throw new System.Exception("numbers have no weakness");
		}

		private async static Task<long> FindWeaknessAsync(IAsyncEnumerator<long> enumerator, byte preambleLength)
		{
			var preamble = new Queue<long>(capacity: preambleLength);

			for (var a = 0; a < preambleLength; a++)
			{
				await enumerator.MoveNextAsync();
				var value = enumerator.Current;
				preamble.Enqueue(value);
			}

			while (await enumerator.MoveNextAsync())
			{
				var value = enumerator.Current;

				if (!IsValid(preamble, value))
				{
					return value;
				}

				preamble.Enqueue(value);
				preamble.Dequeue();
			}

			throw new System.Exception("numbers have no weakness");
		}

		[Theory]
		[InlineData("day09.txt", 31_161_678)]
		public async Task Part1(string filename, long expected)
		{
			var numbers = filename.ReadLinesAsync(long.Parse);

			await using var asyncEnumerator = numbers.GetAsyncEnumerator();

			var actual = await FindWeaknessAsync(asyncEnumerator, 25);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(
			new short[] { 35, 20, 15, 25, 47, 40, 62, 55, 65, 95, 102, 117, 150, 182, 127, 219, 299, 277, 309, 576 },
			127,
			62)]
		public void Example3(IList<short> numbers, short answer, short expected)
		{
			var values = FindEncryptionWeakness(numbers, answer);
			var actual = values.Min() + values.Max();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day09.txt", 31_161_678, 5_453_868)]
		public async Task Part2(string filename, long answer, long expected)
		{
			var asyncEnumerable = filename.ReadLinesAsync(long.Parse);

			var values = await FindEncryptionWeaknessAsync(asyncEnumerable, answer).ToListAsync();

			var actual = values.Min() + values.Max();

			Assert.Equal(expected, actual);
		}

		private static IEnumerable<short> FindEncryptionWeakness(IEnumerable<short> enumerable, short answer)
		{
			var start = 0;

			while (true)
			{
				var values = new List<short>();

				using var enumerator = enumerable.GetEnumerator();

				for (var a = 0; a < start; a++) enumerator.MoveNext();

				while (enumerator.MoveNext())
				{
					var value = enumerator.Current;
					values.Add(value);
					var sum = values.Sum(l => l);

					if (sum == answer) return values;

					if (sum > answer) break;
				}

				start++;
			}

			throw new Exception();
		}

		private async static IAsyncEnumerable<long> FindEncryptionWeaknessAsync(IAsyncEnumerable<long> asyncEnumerable, long answer)
		{
			var start = 0;

			while (true)
			{
				var sum = 0L;
				var values = new List<long>();

				await using var enumerator = asyncEnumerable.GetAsyncEnumerator();

				for (var a = 0; a < start; a++) await enumerator.MoveNextAsync();

				while (await enumerator.MoveNextAsync())
				{
					var value = enumerator.Current;
					values.Add(value);
					sum = values.Sum(l => l);

					if (sum >= answer) break;
				}

				if (sum == answer)
				{
					foreach (var l in values) yield return l;
					break;
				}

				start++;
			}
		}
	}
}
