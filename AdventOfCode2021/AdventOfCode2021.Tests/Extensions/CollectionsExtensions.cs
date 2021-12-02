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
}
