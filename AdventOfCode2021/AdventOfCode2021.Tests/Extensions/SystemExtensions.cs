using System.Globalization;

namespace System;

public static class SystemExtensions
{

	public static IReadOnlyList<IReadOnlyList<T>> SplitTwiceAndParse<T>(this string s, string outerSplitString, string innerSplitString, CultureInfo? provider = default)
		where T : IParseable<T>
	{
		return (from row in s.Split(outerSplitString, StringSplitOptions.RemoveEmptyEntries)
				let cells = row.Split(innerSplitString, StringSplitOptions.RemoveEmptyEntries)
				let inner = from cell in cells
							let number = T.Parse(cell, provider)
							select number
				select inner.ToList()
			   ).ToList();
	}

	public static T Triangular<T>(this T number)
		where T : INumber<T>
	{
		T sum = T.Zero;
		for (T a = T.Zero; a <= number; a++) sum += a;
		return sum;
	}
}
