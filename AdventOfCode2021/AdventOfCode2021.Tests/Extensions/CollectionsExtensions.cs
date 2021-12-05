using AdventOfCode2021.Tests.Models;
using System.Collections.Concurrent;
using System.Drawing;

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
		var outer = itemses
			.Select(row => row.ToList())
			.ToList();

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

	public static IEnumerable<T> GetColumn<T>(this T[,] array, int column)
	{
		var height = array.GetLength(dimension: 0);
		for (var row = 0; row < height; row++)
		{
			yield return array[row, column];
		}
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

	public static IEnumerable<KeyValuePair<T, int>> ReverseArray<T>(this IEnumerable<T> items)
	{
		var value = 0;
		using var enumerator = items.GetEnumerator();

		while (enumerator.MoveNext())
		{
			var key = enumerator.Current;
			yield return new(key, value++);
		}
	}

	public static IReadOnlyDictionary<T, Point> Reverse2dArrayToDictionary<T>(this IEnumerable<IEnumerable<T>> rows)
		where T : notnull
		=> new Dictionary<T, Point>(Reverse2dArray(rows));

	public static IEnumerable<KeyValuePair<T, Point>> Reverse2dArray<T>(this IEnumerable<IEnumerable<T>> rows)
	{
		var y = 0;
		using var outerEnumerator = rows.GetEnumerator();
		while (outerEnumerator.MoveNext())
		{
			var row = outerEnumerator.Current;

			foreach (var (key, x) in ReverseArray(row))
			{
				var value = new Point(x, y);
				yield return new(key, value);
			}
			y++;
		}
	}
	public static IEnumerator<(TFirst, TSecond)> GetEnumerator<TFirst, TSecond>(this (IEnumerable<TFirst>, IEnumerable<TSecond>) tuple)
	{
		var first = tuple.Item1.GetEnumerator();
		var second = tuple.Item2.GetEnumerator();

		return new DoubleEnumerator<TFirst, TSecond>(first, second);
	}
}

public sealed class DoubleEnumerator<TFirst, TSecond> : IEnumerator<(TFirst, TSecond)>
{
	private readonly IEnumerator<TFirst> _first;
	private readonly IEnumerator<TSecond> _second;

	public DoubleEnumerator(IEnumerator<TFirst> first, IEnumerator<TSecond> second)
	{
		_first = first;
		_second = second;
	}

	public (TFirst, TSecond) Current => (_first.Current, _second.Current);
	object IEnumerator.Current => Current;

	public void Dispose()
	{
		_first.Dispose();
		_second.Dispose();
	}

	public bool MoveNext() => _first.MoveNext() && _second.MoveNext();

	public void Reset()
	{
		_first.Reset();
		_second.Reset();
	}
}
