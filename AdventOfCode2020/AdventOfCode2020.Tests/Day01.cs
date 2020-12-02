using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day01
	{
		[Theory]
		[InlineData(514_579, 1_721, 979, 366, 299, 675, 1_456)]
		public void Part1_Example(int expected, params int[] entries)
		{
			var actual = ProductOfTwoValuesWhichSumTo2020(entries);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day01.txt", 889_779)]
		public async Task Part1(string filename, int expected)
		{
			var contents = await filename.ReadLinesAsync().ToListAsync();
			var entries = contents.Select(int.Parse).ToArray();
			var actual = ProductOfTwoValuesWhichSumTo2020(entries);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(241_861_950, 1_721, 979, 366, 299, 675, 1_456)]
		public void Part2_Example(int expected, params int[] entries)
		{
			var actual = ProductOfThreeValuesWhichSumTo2020(entries);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day01.txt", 76_110_336)]
		public async Task Part2(string filename, int expected)
		{
			var entries = await filename.ReadLinesAsync(int.Parse).ToListAsync();
			var actual = ProductOfThreeValuesWhichSumTo2020(entries);
			Assert.Equal(expected, actual);
		}

		private static int ProductOfTwoValuesWhichSumTo2020(IList<int> values)
		{
			var combinations = values.GetUniqueCombinations(2);

			return GetProductOfCominationsTotallingValue(2020, combinations);
		}

		private static int ProductOfThreeValuesWhichSumTo2020(IList<int> values)
		{
			var combinations = values.GetUniqueCombinations(dimensions: 3);

			return GetProductOfCominationsTotallingValue(2020, combinations);
		}

		private static int GetProductOfCominationsTotallingValue(int total, IEnumerable<IEnumerable<int>> combinations)
		{
			foreach (var combination in combinations)
			{
				if (combination.Sum() == total)
				{
					return combination.Product();
				}
			}

			throw new ArgumentException("no combination sum to " + total);
		}
	}
}
