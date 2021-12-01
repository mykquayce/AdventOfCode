using AdventOfCode2021.Tests.Models;
using System.Collections.Concurrent;

namespace System.Collections.Generic;

public static class CollectionsExtensions
{
	private static ChangeTypes Compare(IComparable left, IComparable right)
		=> left.CompareTo(right) switch
		{
			< 0 => ChangeTypes.Increased,
			0 => ChangeTypes.NoChange,
			> 0 => ChangeTypes.Decreased,
		};

	public static IEnumerable<ChangeTypes> Increasings<T>(this IEnumerable<T> items)
		where T : IComparable
	{
		using var enumerator = items.GetEnumerator();

		enumerator.MoveNext();
		var prev = enumerator.Current;

		while (enumerator.MoveNext())
		{
			var curr = enumerator.Current;
			yield return Compare(prev, curr);
			prev = curr;
		}
	}

	public async static IAsyncEnumerable<ChangeTypes> IncreasingsAsync<T>(this IAsyncEnumerable<T> items, CancellationToken? cancellationToken = default)
		where T : IComparable
	{
		await using var enumerator = items.GetAsyncEnumerator(cancellationToken ?? CancellationToken.None);

		await enumerator.MoveNextAsync(cancellationToken ?? CancellationToken.None);
		var prev = enumerator.Current;

		while (await enumerator.MoveNextAsync(cancellationToken ?? CancellationToken.None))
		{
			var curr = enumerator.Current;
			yield return Compare(prev, curr);
			prev = curr;
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
				yield return queue.ToList();
				queue.TryDequeue(out var _);
			}
		}
	}

	public async static IAsyncEnumerable<IReadOnlyCollection<T>> ToWindowsAsync<T>(this IAsyncEnumerable<T> items, int size = 3)
	{
		var queue = new ConcurrentQueue<T>();

		await foreach (var item in items)
		{
			queue.Enqueue(item);
			var count = queue.Count;

			if (count == size)
			{
				yield return queue.ToList();
				queue.TryDequeue(out var _);
			}
		}
	}
}
