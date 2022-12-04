namespace System.Collections.Generic;

public static class EnumerableExtensions
{
	public static IEnumerable<T> Intersections<T>(this IEnumerable<IEnumerable<T>> collections)
	{
		var seed = collections.First();
		static IEnumerable<T> accumulator(IEnumerable<T> left, IEnumerable<T> right) => left.Intersect(right);

		return collections.Skip(1).Aggregate(seed, accumulator);
	}

	public static void Deconstruct<T>(this IEnumerable<T> collection, out T? first, out T? second)
		=> Deconstruct(collection, out first, out second, out _, out _, out _, out _);

	public static void Deconstruct<T>(this IEnumerable<T> collection, out T? first, out T? second, out T? third)
		=> Deconstruct(collection, out first, out second, out third, out _, out _, out _);

	public static void Deconstruct<T>(this IEnumerable<T> collection, out T? first, out T? second, out T? third, out T? fourth)
		=> Deconstruct(collection, out first, out second, out third, out fourth, out _, out _);

	public static void Deconstruct<T>(this IEnumerable<T> collection, out T? first, out T? second, out T? third, out T? fourth, out T? fifth)
		=> Deconstruct(collection, out first, out second, out third, out fourth, out fifth, out _);

	public static void Deconstruct<T>(this IEnumerable<T> collection, out T? first, out T? second, out T? third, out T? fourth, out T? fifth, out T? sixth)
	{
		using var enumerator = collection.GetEnumerator();
		first = f();
		second = f();
		third = f();
		fourth = f();
		fifth = f();
		sixth = f();

		T? f() => enumerator.MoveNext() ? enumerator.Current : default;
	}

	public static IEnumerator<(TFirst, TSecond)> GetEnumerator<TFirst, TSecond>(this (IEnumerable<TFirst>, IEnumerable<TSecond>) tuple)
	{
		var first = tuple.Item1.GetEnumerator();
		var second = tuple.Item2.GetEnumerator();

		return new DoubleEnumerator<TFirst, TSecond>(first, second);
	}
}
