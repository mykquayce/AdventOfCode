using AdventOfCode2020.Tests.Extensions;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day18
	{
		[Theory]
		[InlineData("1 + 2", 3)]
		[InlineData("1 + 2 * 3", 9)]
		[InlineData("1 + 2 * 3 + 4", 13)]
		[InlineData("1 + 2 * 3 + 4 * 5", 65)]
		[InlineData("1 + 2 * 3 + 4 * 5 + 6", 71)]
		public void Example1(string s, int expected)
		{
			var result = DoExpression(s);

			Assert.Equal(expected, int.Parse(result));
		}

		[Theory]
		[InlineData("(2 * 3)", "6")]
		[InlineData("1 + (2 * 3)", "7")]
		[InlineData("(5 + 6)", "11")]
		[InlineData("4 * (5 + 6)", "44")]
		[InlineData("(4 * (5 + 6))", "44")]
		public void DoParenthesesTests(string s, string expected)
		{
			var actual = DoParentheses(s);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("1 + (2 * 3)", 7)]
		[InlineData("4 * (5 + 6)", 44)]
		[InlineData("1 + (2 * 3) + (4 * (5 + 6))", 51)]
		[InlineData("2 * 3 + (4 * 5)", 26)]
		[InlineData("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
		[InlineData("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12_240)]
		[InlineData("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13_632)]
		[InlineData("(7 * (3 * 8)) * 2 * ((2 * 8 * 4 * 2 * 5 + 2) * 6 * (6 * 9 + 4 + 9) * 8) * 2 * 2 + (5 + 7)", 2_774_919_180ul)]
		[InlineData("(4 * (2 * 3)) + 7 + (4 + 2 + 6 * 4 + 9) * 2 * 3", 528ul)]
		[InlineData("3 * (8 + 2 * 2 + 5) + ((2 + 6 + 5 * 7) * 6 + (7 * 7 + 3 + 8) * 5 * 2 * 4) + (8 + (5 + 4 + 5 + 5 * 5))", 24_418ul)]
		[InlineData("(9 + (3 * 9 * 4 * 9 * 3) + 8) * 2", 5_866ul)]
		[InlineData("(9 + 6 * 5) + 7 * 6 + (2 * 8 + 4 * 5) + (8 + 4 * 9 * 3 + 9) * 3", 2_775ul)]
		[InlineData("3 * (2 + (4 + 9 + 9) * 5 * 8 * 8 + (9 * 6 + 4 * 2 + 5 * 9)) + 6 + (6 + 7 * 9 + (2 * 7 * 3)) + 9", 26_481ul)]
		[InlineData("(7 * 5 + (7 + 7 + 8) * 3 * (7 * 5 + 4 * 7 + 5)) + 3 * 6 * 3 * 2", 1_711_476ul)]
		[InlineData("((4 + 6 + 3 * 8 * 7 + 8) + 3) * 5 * ((6 + 4) * 6 + 4)", 236_480)]
		[InlineData("6 * 8 * (2 * 9) + 2 * 8 * 4", 27_712)]
		[InlineData("2 + 3 * 4 * 2 + 3 * 7", 301)]
		[InlineData("3 + (6 * 9 * (4 * 4) * (7 + 2 + 9 * 3 + 5 + 8)) + (3 * (6 + 7 * 4 + 9 * 8) + 3 * 5 + 9 * (5 + 9)) ", 160_707)]
		[InlineData("2 * 2 * (2 * 2)", 16)]
		public void DoBothTests(string s, ulong expected)
		{
			var result = DoParentheses(s);
			Assert.Equal(expected, ulong.Parse(result));
		}

		[Theory]
		[InlineData("day18.txt")]
		public async Task CheckInput(string filename)
		{
			await foreach (var line in filename.ReadLinesAsync())
			{
				if (string.IsNullOrWhiteSpace(line)) continue;
				var result = DoParentheses(line);

				Assert.NotNull(result);
				Assert.NotEmpty(result);
				Assert.NotEqual("0", result);
			}
		}

		[Theory]
		[InlineData("day18.txt", 1_890_866_893_020ul)]
		public async Task Part1(string filename, ulong expected)
		{
			var sum = 0ul;

			await foreach (var line in filename.ReadLinesAsync())
			{
				if (string.IsNullOrWhiteSpace(line)) continue;
				var result = DoParentheses(line);
				var @int = ulong.Parse(result);
				sum += @int;
			}

			Assert.Equal(expected, sum);
		}

		[Theory]
		[InlineData(@"1 + 2 * 3 + 4 * 5 + 6", 231)]
		public void DoExpressionRevisedTest(string s, ulong expected)
		{
			var actual = ulong.Parse(DoExpressionRevised(s));
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"1 + (2 * 3) + (4 * (5 + 6))", 51)]
		[InlineData(@"2 * 3 + (4 * 5)", 46)]
		[InlineData(@"5 + (8 * 3 + 9 + 3 * 4 * 3)", 1_445)]
		[InlineData(@"5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669_060)]
		[InlineData(@"((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23_340)]
		public void DoParenthesesRevisedTests(string s, ulong expected)
		{
			var actual = ulong.Parse(DoParenthesesRevised(s));
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day18.txt", 34646237037193ul)]
		public async Task Part2(string filename, ulong expected)
		{
			var sum = 0ul;

			await foreach (var line in filename.ReadLinesAsync())
			{
				if (string.IsNullOrWhiteSpace(line)) continue;
				var result = DoParenthesesRevised(line);
				var @int = ulong.Parse(result);
				sum += @int;
			}

			Assert.Equal(expected, sum);
		}

		private static ulong DoSum(ulong left, char @operator, ulong right)
		{
			return @operator switch
			{
				'+' => left + right,
				'*' => left * right,
				_ => throw new ArgumentOutOfRangeException(nameof(@operator), @operator, $"Unknown {nameof(@operator)}: {@operator}")
				{
					Data =
					{
						[nameof(left)] = left,
						[nameof(@operator)] = @operator,
						[nameof(right)] = right,
					},
				},
			};
		}

		private static string DoExpression(string input)
		{
			var output = input;

			while (true)
			{
				var match = Regex.Match(output, @"(\d+) ([+*]) (\d+)");

				if (!match.Success) break;

				var left = ulong.Parse(match.Groups[1].Value);
				var @operator = match.Groups[2].Value[0];
				var right = ulong.Parse(match.Groups[3].Value);

				var result = DoSum(left, @operator, right);

				output = string.Concat(
					output[..match.Groups[0].Index],
					result,
					output[(match.Groups[0].Index + match.Groups[0].Length)..]);
			}

			return output;
		}

		private static string DoExpressionRevised(string input)
		{
			var output = input;

			while (true)
			{
				var match = Regex.Match(output, @"(\d+) \+ (\d+)");

				if (!match.Success) break;

				var left = ulong.Parse(match.Groups[1].Value);
				var right = ulong.Parse(match.Groups[2].Value);
				var result = DoSum(left, '+', right);

				output = string.Concat(
					output[..match.Groups[0].Index],
					result,
					output[(match.Groups[0].Index + match.Groups[0].Length)..]);
			}

			while (true)
			{
				var match = Regex.Match(output, @"(\d+) \* (\d+)");

				if (!match.Success) break;

				var left = ulong.Parse(match.Groups[1].Value);
				var right = ulong.Parse(match.Groups[2].Value);
				var result = DoSum(left, '*', right);

				output = string.Concat(
					output[..match.Groups[0].Index],
					result,
					output[(match.Groups[0].Index + match.Groups[0].Length)..]);
			}

			return output;
		}

		private static string DoParentheses(string input)
		{
			var output = input;

			while (true)
			{
				var match = Regex.Match(output, @"\((\d+(?: [+*] \d+)+)\)");

				if (!match.Success) break;

				var result = DoExpression(match.Groups[1].Value);

				output = string.Concat(
					output[..match.Groups[0].Index],
					result,
					output[(match.Groups[0].Index + match.Groups[0].Length)..]);
			}

			output = DoExpression(output);

			return output;
		}

		private static string DoParenthesesRevised(string input)
		{
			var output = input;

			while (true)
			{
				var match = Regex.Match(output, @"\((\d+(?: [+*] \d+)+)\)");

				if (!match.Success) break;

				var result = DoExpressionRevised(match.Groups[1].Value);

				output = string.Concat(
					output[..match.Groups[0].Index],
					result,
					output[(match.Groups[0].Index + match.Groups[0].Length)..]);
			}

			output = DoExpressionRevised(output);

			return output;
		}
	}
}
