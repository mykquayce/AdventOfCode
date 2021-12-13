using Xunit;

namespace AdventOfCode2021.Tests;

public class Day12
{
	[Theory]
	[InlineData(@"start-A
start-b
A-c
A-b
b-d
A-end
b-end", 10)]
	[InlineData(@"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc", 19)]
	[InlineData(@"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW", 226)]
	public void Test1(string input, int expected)
	{
		var dictionary = ParseInput(input);
		var actual = GetPaths(dictionary).Count();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day12.txt", 4_413)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var input = await fileName.ReadFileAsync();
		var dictionary = ParseInput(input);
		var actual = GetPaths(dictionary).Count();
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new[] { "start", "b", "d", "b", "A", "c", "A", "end", }, "b")]
	public void Test3(string[] caves, string expected)
	{
		var path = new CaveSystemPath(caves);
		var actual = (from p in path
					  where p != "start"
					  where p != "end"
					  where char.IsLower(p, index: 0)
					  group p by p into gg
					  where gg.Skip(1).Any()
					  select gg.Key
					).Single();
		Assert.Equal(expected, actual);
	}

	private static IReadOnlyDictionary<string, ICollection<string>> ParseInput(string input)
	{
		var dictionary = new Dictionary<string, ICollection<string>>();

		foreach (var path in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
		{
			var (left, right) = path.Split('-', StringSplitOptions.RemoveEmptyEntries);
			if (dictionary.ContainsKey(left!))
			{
				dictionary[left!].Add(right!);
			}
			else
			{
				dictionary.Add(left!, new List<string> { right!, });
			}
			if (dictionary.ContainsKey(right!))
			{
				dictionary[right!].Add(left!);
			}
			else
			{
				dictionary.Add(right!, new List<string> { left!, });
			}
		}

		return dictionary;
	}

	private static IEnumerable<ICollection<string>> GetPaths(IReadOnlyDictionary<string, ICollection<string>> graph)
	{
		IList<CaveSystemPath> paths = new List<CaveSystemPath> { new("start"), };

		while (paths.Any(p => p.Last() != "end" && p.Last() != "blocked"))
		{
			var count = paths.Count;
			for (var a = 0; a < count; a++)
			{
				var from = paths[a].Last();
				if (from == "end" || from == "blocked") continue;
				var tos = graph[from];
				foreach (var to in tos.Skip(1))
				{
					if (char.IsLower(to, index: 0)
						&& paths[a].Contains(to))
					{
						paths.Add(new(paths[a].Append("blocked")));
					}
					else
					{
						paths.Add(new(paths[a].Append(to)));
					}
				}
				if (char.IsLower(tos.First(), index: 0)
					&& paths[a].Contains(tos.First()))
				{
					paths[a].Add("blocked");
				}
				else
				{
					paths[a].Add(tos.First());
				}
			}
		}

		return paths.Where(p => p.Last() != "blocked");
	}
}

public class CaveSystemPath : ICollection<string>
{
	private readonly ICollection<string> _path;

	public CaveSystemPath(string first) => _path = new List<string> { first, };
	public CaveSystemPath(IEnumerable<string> path) => _path = path.ToList();

	public int SmallCaveVisitCount => (from p in _path
									   where p != "start"
									   where p != "end"
									   where char.IsLower(p, index: 0)
									   group p by p into gg
									   where gg.Skip(1).Any()
									   select 1).Count();

	#region icollection implementation
	public int Count => _path.Count;
	public bool IsReadOnly => _path.IsReadOnly;
	public void Add(string item) => _path.Add(item);
	public void Clear() => _path.Clear();
	public bool Contains(string item) => _path.Contains(item);
	public void CopyTo(string[] array, int arrayIndex) => _path.CopyTo(array, arrayIndex);
	public IEnumerator<string> GetEnumerator() => _path.GetEnumerator();
	public bool Remove(string item) => _path.Remove(item);
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
	#endregion icollection implementation
}
