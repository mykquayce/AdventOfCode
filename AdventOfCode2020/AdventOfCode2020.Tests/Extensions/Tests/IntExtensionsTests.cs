using System;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Tests.Extensions.Tests
{
	public class IntExtensionsTests
	{
		[Theory]
		[InlineData(1, 1)]
		[InlineData(1, 1, 1)]
		[InlineData(1, 1, 1, 1)]
		[InlineData(2, 1, 2)]
		[InlineData(4, 2, 2)]
		[InlineData(9, 3, 3)]
		[InlineData(27, 3, 3, 3)]
		public void Product(int expected, params int[] before)
		{
			var actual = before.Product();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(0, 2, 0)]
		[InlineData(1, 2, 1)]
		[InlineData(2, 2, 1, 0)]
		[InlineData(3, 2, 1, 1)]
		[InlineData(4, 2, 1, 0, 0)]
		[InlineData(0, 3, 0)]
		[InlineData(1, 3, 1)]
		[InlineData(2, 3, 2)]
		[InlineData(3, 3, 1, 0)]
		[InlineData(16, 16, 1, 0)]
		public void ToModulosTest(int value, int @base, params int[] expected)
		{
			var actual = value.ToModulos(@base).Reverse().ToList();
			Assert.Equal(expected, actual);
		}
	}
}
