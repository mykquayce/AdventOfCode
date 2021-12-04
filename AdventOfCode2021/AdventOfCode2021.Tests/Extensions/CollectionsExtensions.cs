using AdventOfCode2021.Tests.Models;
using System.Collections.Concurrent;

namespace System.Collections.Generic;

public static class CollectionsExtensions
{
	private static ChangeTypes Compare(IComparable left, object? right)
		=> left.CompareTo(right) switch
		{
			< 0 => ChangeTypes.Increased,
			0 => ChangeTypes.NoChange,
			> 0 => ChangeTypes.Decreased,
		};

	public static IEnumerable<ChangeTypes> Increasings<T>(this IEnumerable<T> items)
		where T : IComparable
	{
		var pairs = items.ToWindows(size: 2);

		foreach (var (first, second) in pairs)
		{
			yield return Compare(first!, second);
		}
	}

	public async static IAsyncEnumerable<ChangeTypes> IncreasingsAsync<T>(this IAsyncEnumerable<T> items, CancellationToken? cancellationToken = default)
		where T : IComparable
	{
		var pairs = items.ToWindowsAsync(size: 2, cancellationToken);

		await foreach (var (first, second) in pairs)
		{
			if (cancellationToken?.IsCancellationRequested == true) throw new OperationCanceledException(cancellationToken!.Value);
			yield return Compare(first!, second);
		}
	}

	public static IEnumerable<IReadOnlyCollection<T>> ToWindows<T>(this IEnumerable<T> items, int size = 3)
	{
		var queue = new ConcurrentQueue<T>();

		foreach (var item in items)
		{
			queue.Enqueue(item);
			var count = queue.Count;

			if (count == size)
			{
				yield return queue.ToArray();
				queue.TryDequeue(out var _);
			}
		}
	}

	public async static IAsyncEnumerable<IReadOnlyCollection<T>> ToWindowsAsync<T>(
		this IAsyncEnumerable<T> items,
		int size = 3,
		CancellationToken? cancellationToken = default)
	{
		var queue = new ConcurrentQueue<T>();

		await foreach (var item in items)
		{
			if (cancellationToken?.IsCancellationRequested == true) throw new OperationCanceledException(cancellationToken!.Value);
			queue.Enqueue(item);
			var count = queue.Count;

			if (count == size)
			{
				yield return queue.ToArray();
				queue.TryDequeue(out var _);
			}
		}
	}

	public static void Deconstruct<T>(this IEnumerable<T> items, out T? first, out T? second)
	{
		using var enumerator = items.GetEnumerator();
		first = enumerator.MoveNext() ? enumerator.Current : default;
		second = enumerator.MoveNext() ? enumerator.Current : default;
	}

	public static T[,] To2dArray<T>(this IEnumerable<IEnumerable<T>> itemses)
	{
		var outer = new List<List<T>>();

		foreach (var items in itemses)
		{
			var inner = new List<T>();

			foreach (var item in items)
			{
				inner.Add(item);
			}

			outer.Add(inner);
		}

		var length = outer.Count;
		var width = outer[0].Count;

		var result = new T[length, width];

		for (var y = 0; y < length; y++)
		{
			for (var x = 0; x < width; x++)
			{
				result[y, x] = outer[y][x];
			}
		}

		return result;
	}

	public static bool[,] To2dBoolArray<T>(this T[,] arrays, T truthValue)
		where T : IEqualityOperators<T, T>
	{
		var length = arrays.GetLength(dimension: 0);
		var width = arrays.GetLength(dimension: 1);
		var result = new bool[length, width];

		for (var y = 0; y < length; y++)
		{
			for (var x = 0; x < width; x++)
			{
				result[y, x] = arrays[y, x] == truthValue;
			}
		}

		return result;
	}

	public static T[,] Rotate<T>(this T[,] array)
	{
		var length = array.GetLength(dimension: 0);
		var width = array.GetLength(dimension: 1);
		var result = new T[width, length];

		for (var y = 0; y < length; y++)
		{
			for (var x = 0; x < width; x++)
			{
				result[x, y] = array[y, x];
			}
		}

		return result;
	}

	public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
	{
		var width = array.GetLength(dimension: 1);
		for (var column = 0; column < width; column++)
		{
			yield return array[row, column];
		}
	}

	public static int ToDecimal(this IEnumerable<bool> bools)
	{
		var chars = bools.Select(b => b ? '1' : '0').ToArray();
		var s = new string(chars);
		return Convert.ToInt32(s, fromBase: 2);
	}

	public static int ToDecimal(this IEnumerable<char> chars)
	{
		var s = new string(chars.ToArray());
		return Convert.ToInt32(s, fromBase: 2);
	}
}
