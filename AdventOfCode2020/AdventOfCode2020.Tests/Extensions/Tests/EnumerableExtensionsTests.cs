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
	}
}
