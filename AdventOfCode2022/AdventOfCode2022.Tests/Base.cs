using System.Runtime.CompilerServices;

namespace AdventOfCode2022.Tests;

public abstract class Base
{
	public IAsyncEnumerable<string> Input => GetInputAsync();

	public async IAsyncEnumerable<string> GetInputAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var paths = new string[3] { ".", "Data", this.GetType().Name + ".txt", };
		var path = Path.Combine(paths);
		using var reader = new StreamReader(path);
		string? line;
		while ((line = await reader.ReadLineAsync(cancellationToken)) is not null)
		{
			yield return line;
		}
	}

	public IAsyncEnumerable<T> GetInputAsync<T>(IFormatProvider? provider = null, CancellationToken cancellationToken = default)
		where T : IParsable<T>
	{
		T? result = default;
		var lines = GetInputAsync(cancellationToken);
		return from line in lines
			   let ok = T.TryParse(line, provider, out result)
			   select ok ? result : default;
	}
}
