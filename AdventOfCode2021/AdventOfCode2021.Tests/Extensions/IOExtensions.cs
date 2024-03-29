using System.Globalization;

namespace System.IO;

public static class IOExtensions
{
	public static async IAsyncEnumerable<string> ReadLinesAsync(this string fileName)
	{
		var path = Path.Combine(".", "PuzzleInput", fileName);
		await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		using var reader = new StreamReader(stream);
		string? line = null;

		while ((line = await reader.ReadLineAsync()) is not null)
		{
			yield return line;
		}
	}

	public static async IAsyncEnumerable<T> ReadAndParseLinesAsync<T>(this string fileName)
		where T : IParseable<T>
	{
		await foreach (var line in ReadLinesAsync(fileName))
		{
			yield return T.Parse(line, CultureInfo.InvariantCulture);
		}
	}

	public static async Task<string> ReadFileAsync(this string fileName)
	{
		var path = Path.Combine(".", "PuzzleInput", fileName);
		await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		using var reader = new StreamReader(stream);
		return await reader.ReadToEndAsync();
	}

	public static async Task<T> ReadAndParseFileAsync<T>(this string fileName, IFormatProvider? provider = default)
		where T : IParseable<T>
	{
		var contents = await ReadFileAsync(fileName);
		return T.Parse(contents, provider);
	}
}
