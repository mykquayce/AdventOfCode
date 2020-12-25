using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Tests.Extensions.Tests
{
	public class EnumerableExtensionsTests
	{
		[Fact]
		public void GetCombinationsTest()
		{
			var dimensions = 2;
			var array = new[] { 1, 2, 3, 4, 5, 6, };

			var combinations = array.GetCombinations(dimensions).ToList();

			Assert.NotNull(combinations);
			Assert.NotEmpty(combinations);
			Assert.Equal(36, combinations.Count);
			Assert.Equal(new[] { 1, 1, }, combinations[0]);
			Assert.Equal(new[] { 1, 2, }, combinations[1]);
			Assert.Equal(new[] { 1, 3, }, combinations[2]);
			Assert.Equal(new[] { 1, 4, }, combinations[3]);
			Assert.Equal(new[] { 1, 5, }, combinations[4]);
			Assert.Equal(new[] { 1, 6, }, combinations[5]);
			Assert.Equal(new[] { 2, 1, }, combinations[6]);
			Assert.Equal(new[] { 2, 2, }, combinations[7]);
			Assert.Equal(new[] { 2, 3, }, combinations[8]);
			Assert.Equal(new[] { 2, 4, }, combinations[9]);
			Assert.Equal(new[] { 2, 5, }, combinations[10]);
			Assert.Equal(new[] { 2, 6, }, combinations[11]);
			Assert.Equal(new[] { 3, 1, }, combinations[12]);
			Assert.Equal(new[] { 3, 2, }, combinations[13]);
			Assert.Equal(new[] { 3, 3, }, combinations[14]);
			Assert.Equal(new[] { 3, 4, }, combinations[15]);
			Assert.Equal(new[] { 3, 5, }, combinations[16]);
			Assert.Equal(new[] { 3, 6, }, combinations[17]);
			Assert.Equal(new[] { 4, 1, }, combinations[18]);
			Assert.Equal(new[] { 4, 2, }, combinations[19]);
			Assert.Equal(new[] { 4, 3, }, combinations[20]);
			Assert.Equal(new[] { 4, 4, }, combinations[21]);
			Assert.Equal(new[] { 4, 5, }, combinations[22]);
			Assert.Equal(new[] { 4, 6, }, combinations[23]);
			Assert.Equal(new[] { 5, 1, }, combinations[24]);
			Assert.Equal(new[] { 5, 2, }, combinations[25]);
			Assert.Equal(new[] { 5, 3, }, combinations[26]);
			Assert.Equal(new[] { 5, 4, }, combinations[27]);
			Assert.Equal(new[] { 5, 5, }, combinations[28]);
			Assert.Equal(new[] { 5, 6, }, combinations[29]);
			Assert.Equal(new[] { 6, 1, }, combinations[30]);
			Assert.Equal(new[] { 6, 2, }, combinations[31]);
			Assert.Equal(new[] { 6, 3, }, combinations[32]);
			Assert.Equal(new[] { 6, 4, }, combinations[33]);
			Assert.Equal(new[] { 6, 5, }, combinations[34]);
			Assert.Equal(new[] { 6, 6, }, combinations[35]);
		}

		[Fact]
		public void PadEndTest()
		{
			var before = new[] { 1, 2, 3, };
			var actual = before.PadEnd(6, 0);
			Assert.Equal(new[] { 1, 2, 3, 0, 0, 0, }, actual);
		}

		[Theory]
		[InlineData(5, 4)]
		[InlineData(4, 5)]
		public void GetSize(int width, int height)
		{
			var array = new bool[width, height];
			var size = array.GetSize();
			Assert.Equal(width, size.Width);
			Assert.Equal(height, size.Height);
		}

		[Theory]
		[InlineData("abcxabcyabcz", "abcxyz")]
		public void Dedupe(string s, string expected)
		{
			var actual = new string(s.Dedupe().ToArray());
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(0, 0, 0, 0)]
		[InlineData(1, 1, 0, 0)]
		[InlineData(2, 0, 1, 0)]
		[InlineData(3, 1, 1, 0)]
		[InlineData(4, 0, 2, 0)]
		[InlineData(5, 1, 2, 0)]
		[InlineData(6, 0, 3, 0)]
		[InlineData(7, 1, 3, 0)]
		[InlineData(8, 0, 4, 0)]
		[InlineData(9, 1, 4, 0)]
		[InlineData(10, 0, 0, 1)]
		[InlineData(11, 1, 0, 1)]
		[InlineData(12, 0, 1, 1)]
		[InlineData(13, 1, 1, 1)]
		[InlineData(14, 0, 2, 1)]
		[InlineData(15, 1, 2, 1)]
		[InlineData(16, 0, 3, 1)]
		[InlineData(17, 1, 3, 1)]
		[InlineData(18, 0, 4, 1)]
		[InlineData(19, 1, 4, 1)]
		[InlineData(20, 0, 0, 2)]
		[InlineData(21, 1, 0, 2)]
		[InlineData(22, 0, 1, 2)]
		[InlineData(23, 1, 1, 2)]
		[InlineData(24, 0, 2, 2)]
		[InlineData(25, 1, 2, 2)]
		[InlineData(26, 0, 3, 2)]
		[InlineData(27, 1, 3, 2)]
		[InlineData(28, 0, 4, 2)]
		[InlineData(29, 1, 4, 2)]
		[InlineData(30, 0, 0, 3)]
		[InlineData(31, 1, 0, 3)]
		[InlineData(32, 0, 1, 3)]
		[InlineData(33, 1, 1, 3)]
		[InlineData(34, 0, 2, 3)]
		[InlineData(35, 1, 2, 3)]
		[InlineData(36, 0, 3, 3)]
		[InlineData(37, 1, 3, 3)]
		[InlineData(38, 0, 4, 3)]
		[InlineData(39, 1, 4, 3)]
		public void GetIndexCombinations(int index, params int[] expected)
		{
			var dimensions = new[] { 2, 5, 4, };
			var indices = dimensions.GetIndexCombinations().ToList();
			Assert.Equal(expected, indices[index]);
		}

		[Theory]
		[InlineData(new[] { "abcdefghi", "jklmno", "pqrstu", "vwxyz", }, 0, "ajpv")]
		[InlineData(new[] { "abcdefghi", "jklmno", "pqrstu", "vwxyz", }, 1, "bjpv")]
		[InlineData(new[] { "abcdefghi", "jklmno", "pqrstu", "vwxyz", }, 2, "cjpv")]
		[InlineData(new[] { "abcdefghi", "jklmno", "pqrstu", "vwxyz", }, 9, "akpv")]
		[InlineData(new[] { "ab", "cd", }, 0, "ac")]
		[InlineData(new[] { "ab", "cd", }, 1, "bc")]
		[InlineData(new[] { "ab", "cd", }, 2, "ad")]
		[InlineData(new[] { "ab", "cd", }, 3, "bd")]
		public void GetCombinationsTests(string[] strings, int index, string expected)
		{
			var lists = strings.Select(s => (IList<char>)s.ToCharArray()).ToList();
			var actual = lists.GetArraysCombinations().Skip(index).First();
			Assert.Equal(expected, new string((char[])actual));
		}

		[Fact]
		public void Day19Tests()
		{
			var arrays = new[]
			{
				new []{"a",},
				new[]{"ab", "ba", },
			};

			var combinations = arrays.GetArraysCombinations().ToList();

			Assert.Equal(2, combinations.Count);
			Assert.Equal(new[] { "a", "ab", }, combinations[0]);
			Assert.Equal(new[] { "a", "ba", }, combinations[1]);
		}
	}
}
