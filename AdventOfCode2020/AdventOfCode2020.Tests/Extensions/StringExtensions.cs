using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2020.Tests.Extensions
{
	public static class StringExtensions
	{
		public static IAsyncEnumerable<string> ReadLinesAsync(this string filename) => filename.ReadLinesAsync(s => s);

		public async static IAsyncEnumerable<T> ReadLinesAsync<T>(this string filename, Func<string, T> parser)
		{
			var path = Path.Combine(".", "Data", filename);
			using var reader = File.OpenText(path);

			for (string? line = await reader.ReadLineAsync();
				line is not null;
				line = await reader.ReadLineAsync())
			{
				var value = parser(line);
				yield return value;
			}
		}

		public static Task<string> ReadAllTextAsync(this string filename)
		{
			var path = Path.Combine(".", "Data", filename);
			return File.ReadAllTextAsync(path);
		}
	}
}
