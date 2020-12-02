using AdventOfCode2020.Tests.Extensions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day02
	{
		[Theory]
		[InlineData("1-3 a: abcde", true)]
		[InlineData("1-3 b: cdefg", false)]
		[InlineData("2-9 c: ccccccccc", true)]
		public void Part1_Example(string s, bool expected)
		{
			var policy = Policy.Parse(s);

			Assert.Equal(expected, policy.IsValid_Old);
		}

		[Theory]
		[InlineData("day02.txt", 469)]
		public async Task Part1(string filename, int expected)
		{
			var count = 0;
			var lines = filename.ReadLinesAsync();

			await foreach (var line in lines)
			{
				var policy = Policy.Parse(line);

				if (policy.IsValid_Old) count++;
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("1-3 a: abcde", true)]
		[InlineData("1-3 b: cdefg", false)]
		[InlineData("2-9 c: ccccccccc", false)]
		public void Part2_Example(string s, bool expected)
		{
			var policy = Policy.Parse(s);

			var actual = policy.IsValid;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day02.txt", 267)]
		public async Task Part2(string filename, int expected)
		{
			var count = 0;
			var lines = filename.ReadLinesAsync();

			await foreach (var line in lines)
			{
				var policy = Policy.Parse(line);

				if (policy.IsValid) count++;
			}

			Assert.Equal(expected, count);
		}

		private record Policy(int Min, int Max, char Char, string Password)
		{
			public bool IsValid_Old
			{
				get
				{
					var count = Password.Count(c => c == Char);
					return count >= Min && count <= Max;
				}
			}

			public bool IsValid => Password[Min - 1] == Char ^ Password[Max - 1] == Char;

			public static Policy Parse(string s)
			{
				var regex = new Regex(@"^(\d+)-(\d+) (\w): (\w+)$");

				var match = regex.Match(s);
				var min = int.Parse(match.Groups[1].Value);
				var max = int.Parse(match.Groups[2].Value);
				var @char = match.Groups[3].Value[0];
				var password = match.Groups[4].Value;

				return new Policy(min, max, @char, password);
			}
		}
	}
}
