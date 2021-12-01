using System.Globalization;

namespace System.IO;

public static class IOExtensions
{
	public static async IAsyncEnumerable<T> ReadAndParseLinesAsync<T>(this string fileName)
		where T : IParseable<T>
	{
		var path = Path.Combine(".", "PuzzleInput", fileName);
		await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		using var reader = new StreamReader(stream);
		string? line = null;

		while ((line = await reader.ReadLineAsync()) is not null)
		{
			yield return T.Parse(line, CultureInfo.InvariantCulture);
		}
	}
}
