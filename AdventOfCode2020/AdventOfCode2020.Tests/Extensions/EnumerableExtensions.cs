using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2020.Tests.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> PadEnd<T>(this IEnumerable<T> values, int totalWidth, T paddingValue)
		{
			var padding = Enumerable.Repeat(paddingValue, totalWidth);

			return values
				.Concat(padding)
				.Take(totalWidth);
		}

		public static IEnumerable<ICollection<T>> GetCombinations<T>(this IList<T> array, int dimensions = 2)
		{
			var count = (int)Math.Pow(array.Count, dimensions);

			for (var a = 0; a < count; a++)
			{
				var indices = a
					.ToModulos(radix: array.Count)
					.PadEnd(dimensions, paddingValue: 0)
					.Reverse()
					.ToList();

				var values = new T[dimensions];

				for (var b = 0; b < dimensions; b++)
				{
					var index = indices[b];
					values[b] = array[index];
				}

				yield return values;
			}
		}

		public static IEnumerable<ICollection<T>> GetUniqueCombinations<T>(this IList<T> array, int dimensions = 2)
		{
			var combinations = array.GetCombinations(dimensions);

			foreach (var combination in combinations)
			{
				if (combination.Distinct().Count() < combination.Count)
				{
					continue;
				}

				yield return combination;
			}
		}

		public static Size GetSize<T>(this T[,] array) => new(array.GetLength(0), array.GetLength(1));

		public static IEnumerable<T> Dedupe<T>(this IEnumerable<T> collection)
		{
			return from i in collection
				   group i by i into gg
				   select gg.Key;
		}

		public static IEnumerable<T> ToEnumerable<T>(this IDictionary<int, T> dictionary, T @default)
		{
			var max = dictionary.Keys.Max();

			for (var a = 0; a <= max; a++)
			{
				yield return dictionary.ContainsKey(a)
					? dictionary[a]
					: @default;
			}
		}

		public static IEnumerable<KeyValuePair<int, T>> ToKeyValuePairs<T>(this IEnumerable<T> collection) => collection.Select((item, index) => new KeyValuePair<int, T>(index, item));
		public static IDictionary<int, T> ToDictionary<T>(this IEnumerable<T> collection) => new Dictionary<int, T>(collection.ToKeyValuePairs());
		public static IEnumerable<TValue> GetValues<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> collection) => collection.Select(kvp => kvp.Value);
		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> collection) where TKey : notnull
			=> collection.ToDictionary(t => t.Item1, t => t.Item2);

		public static IEnumerable<IList<int>> GetIndexCombinations(this IList<int> dimensions)
		{
			for (var a = 0; a < dimensions.Product(); a++)
			{
				var list = new int[dimensions.Count];

				for (var b = 0; b < dimensions.Count; b++)
				{
					list[b] = (a / dimensions.Take(b).Product()) % dimensions[b];
				}

				yield return list;
			}
		}

		public static IEnumerable<IList<T>> GetArraysCombinations<T>(this IList<IList<T>> lists)
		{
			var length = lists.Count;
			var dimensions = lists.Select(s => s.Count).ToList();

			foreach (var indices in dimensions.GetIndexCombinations())
			{
				var combination = new T[length];

				for (var a = 0; a < length; a++)
				{
					var index = indices[a];
					combination[a] = lists[a][index];
				}

				yield return combination;
			}
		}
	}
}
