using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day15
	{
		[Theory]
		[InlineData(new[] { 0, 3, 6, }, 4, 0)]
		[InlineData(new[] { 0, 3, 6, }, 5, 3)]
		[InlineData(new[] { 0, 3, 6, }, 6, 3)]
		[InlineData(new[] { 0, 3, 6, }, 7, 1)]
		[InlineData(new[] { 0, 3, 6, }, 8, 0)]
		[InlineData(new[] { 0, 3, 6, }, 9, 4)]
		[InlineData(new[] { 0, 3, 6, }, 10, 0)]
		[InlineData(new[] { 0, 3, 6, }, 2_020, 436)]
		[InlineData(new[] { 1, 3, 2, }, 2_020, 1)]
		[InlineData(new[] { 2, 1, 3, }, 2_020, 10)]
		[InlineData(new[] { 1, 2, 3, }, 2_020, 27)]
		[InlineData(new[] { 2, 3, 1, }, 2_020, 78)]
		[InlineData(new[] { 3, 2, 1, }, 2_020, 438)]
		[InlineData(new[] { 3, 1, 2, }, 2_020, 1_836)]
		[InlineData(new[] { 1, 0, 16, 5, 17, 4, }, 2_020, 1_294)]
		[InlineData(new[] { 0, 3, 6, }, 30_000_000, 175_594)]
		[InlineData(new[] { 1, 3, 2, }, 30_000_000, 2_578)]
		[InlineData(new[] { 2, 1, 3, }, 30_000_000, 3_544_142)]
		[InlineData(new[] { 1, 2, 3, }, 30_000_000, 261_214)]
		[InlineData(new[] { 2, 3, 1, }, 30_000_000, 6_895_259)]
		[InlineData(new[] { 3, 2, 1, }, 30_000_000, 18)]
		[InlineData(new[] { 3, 1, 2, }, 30_000_000, 362)]
		[InlineData(new[] { 1, 0, 16, 5, 17, 4, }, 30_000_000, 573_522)]
		public void Example1(IList<int> startingNumbers, int turns, int expected)
		{
			var dictionary = new Dictionary<int, Indices>();

			for (var a = 0; a < startingNumbers.Count; a++)
			{
				var key = startingNumbers[a];
				var indices = new Indices { Current = a + 1, };
				dictionary.Add(key, indices);
			}

			var last = startingNumbers.Last();

			for (var a = startingNumbers.Count; a < turns; a++)
			{
				var indices = dictionary[last];
				var index = indices.Current + 1;

				var value = indices.Previous.HasValue
					? indices.Current - indices.Previous.Value
					: 0;

				if (dictionary.ContainsKey(value))
				{
					var o = dictionary[value];
					o.Previous = o.Current;
					o.Current = index;
					dictionary[value] = o;
				}
				else
				{
					dictionary.Add(value, new Indices { Current = index, });
				}

				last = value;
			}

			var actual = (from kvp in dictionary
						  where kvp.Value.Current == turns
						  select kvp.Key
						 ).Single();

			Assert.Equal(expected, actual);
		}
	}

	public struct Indices
	{
		public int Current { get; set; }
		public int? Previous { get; set; }
	}
}
