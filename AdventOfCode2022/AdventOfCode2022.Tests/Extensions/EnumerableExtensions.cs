namespace System.Collections.Generic;

public static class EnumerableExtensions
{
	public static IEnumerable<T> Intersections<T>(this IEnumerable<IEnumerable<T>> collections)
	{
		var seed = collections.First();
		static IEnumerable<T> accumulator(IEnumerable<T> left, IEnumerable<T> right) => left.Intersect(right);

		return collections.Skip(1).Aggregate(seed, accumulator);
	}
}
