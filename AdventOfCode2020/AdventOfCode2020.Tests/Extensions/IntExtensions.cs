using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Tests.Extensions
{
	public static class IntExtensions
	{
		public static int Product(this IEnumerable<int> ints)
		{
			const int seed = 1;
			static int accumulator(int product, int next) => product * next;

			return ints.Aggregate(seed, accumulator);
		}

		public static IEnumerable<int> ToModulos(this int value, int radix = 10)
		{
			do
			{
				yield return value % radix;
				value /= radix;
			}
			while (value > 0);
		}
	}
}
