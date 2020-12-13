using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day13
	{
		[Theory]
		[InlineData(@"939
7,13,x,x,59,x,31,19", 59, 5)]
		public void Example1(string input, int expectedBusNo, int expectedWaitTime)
		{
			var (earliest, buses) = ParseInput(input);

			var (bus, wait) = GetEarliestBus(earliest, buses);

			Assert.Equal(expectedBusNo, bus);
			Assert.Equal(expectedWaitTime, wait);
		}

		private static (int, IEnumerable<int>) ParseInput(string input)
		{
			var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var earliest = int.Parse(lines[0]);
			var buses = ParseBuses(lines[1]);
			return (earliest, buses);
		}

		private static IEnumerable<int> ParseBuses(string line)
		{
			return from s in line.Split(',', StringSplitOptions.RemoveEmptyEntries)
				   let n = int.TryParse(s, out var i) ? i : -1
				   where n >= 0
				   select n;
		}

		private static (int bus, int wait) GetEarliestBus(int offset, IEnumerable<int> buses)
		{
			var lowestDiff = int.MaxValue;
			var earliestBus = -1;

			foreach (var bus in buses)
			{
				var modulo = offset % bus;
				var diff = bus - modulo;

				if (diff < lowestDiff)
				{
					lowestDiff = diff;
					earliestBus = bus;
				}

				if (diff == 0) break;
			}

			return (earliestBus, lowestDiff);
		}

		[Theory]
		[InlineData("day13.txt", 222)]
		public async Task Part1(string filename, int expected)
		{
			var input = await filename.ReadAllTextAsync();
			var (earliest, buses) = ParseInput(input);
			var (bus, wait) = GetEarliestBus(earliest, buses);
			Assert.Equal(expected, bus * wait);
		}

		[Theory]
		[InlineData("7,13,x,x,59,x,31,19", 1_068_781)]
		[InlineData("17,x,13,19", 3_417)]
		[InlineData("67,7,59,61", 754_018)]
		[InlineData("67,x,7,59,61", 779_210)]
		[InlineData("67,7,x,59,61", 1_261_476)]
		[InlineData("1789,37,47,1889", 1_202_161_486)]
		public void Example2(string input, int expected)
		{
			var buses = ParseBuses2(input).ToDictionary();
			var timestamp = SolvePart2(buses);
			Assert.Equal(expected, timestamp);
		}

		private static IEnumerable<(int, int)> ParseBuses2(string s)
		{
			var values = s.Split(',', StringSplitOptions.RemoveEmptyEntries);

			for (var index = 0; index < values.Length; index++)
			{
				if (int.TryParse(values[index], out var bus))
				{
					yield return (index, bus);
				}
			}
		}

		private static long SolvePart2(IDictionary<int, int> buses)
		{
			var found = 0;
			long timestamp, start = 0, product = 1;

			for (long a = 1; true; a++)
			{
				timestamp = start + (product * a);

				foreach (var (index, bus) in buses.Skip(found))
				{
					var divisible = ((timestamp + index) % bus) == 0;

					if (divisible)
					{
						found++;
						product = buses.Take(found).GetValues().Select(i => (long)i).Product();
						start = timestamp;
						a = 0;
					}
					else break;
				}

				if (found == buses.Count) return timestamp;
			}
		}

		[Theory]
		[InlineData("day13.txt", 408_270_049_879_073)]
		public async Task Part2(string filename, int expected)
		{
			var line = await filename.ReadLinesAsync().Skip(1).FirstAsync();
			var buses = ParseBuses2(line).ToDictionary();
			var timestamp = SolvePart2(buses);
			Assert.Equal(expected, timestamp);
		}
	}
}
