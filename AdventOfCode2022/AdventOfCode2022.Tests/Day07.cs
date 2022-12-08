using System.Text.RegularExpressions;

namespace AdventOfCode2022.Tests;

public partial class Day07 : Base
{
	[GeneratedRegex(@"^(\d+) ([\w\.]+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
	private static partial Regex FileOutputRegex();

	[Theory]
	[InlineData(
		48_381_165L,
		"$ cd /", "$ ls", "dir a", "14848514 b.txt", "8504156 c.dat", "dir d", "$ cd a", "$ ls", "dir e", "29116 f", "2557 g", "62596 h.lst", "$ cd e", "$ ls", "584 i", "$ cd ..", "$ cd ..", "$ cd d", "$ ls", "4060174 j", "8033020 d.log", "5626152 d.ext", "7214296 k")]
	public void SolveExample1_Total(long expected, params string[] lines)
	{
		var root = Populate(lines);

		var actual = getSize(root);
		Assert.Equal(expected, actual);

		static long getSize(Directory dir)
		{
			var size = 0L;

			foreach (var file in dir.Files)
			{
				size += file.Size;
			}

			foreach (var sub in dir.SubDirectories)
			{
				size += getSize(sub);
			}

			return size;
		}
	}

	[Theory]
	[InlineData(
		95_437L,
		"$ cd /", "$ ls", "dir a", "14848514 b.txt", "8504156 c.dat", "dir d", "$ cd a", "$ ls", "dir e", "29116 f", "2557 g", "62596 h.lst", "$ cd e", "$ ls", "584 i", "$ cd ..", "$ cd ..", "$ cd d", "$ ls", "4060174 j", "8033020 d.log", "5626152 d.ext", "7214296 k")]
	public void SolveExample1_Max100000(long expected, params string[] lines)
	{
		var sizes = new List<long>();
		var root = Populate(lines);

		getSize(root);
		var actual = sizes.Sum();
		Assert.Equal(expected, actual);

		long getSize(Directory dir)
		{
			var size = 0L;

			foreach (var file in dir.Files)
			{
				size += file.Size;
			}

			foreach (var sub in dir.SubDirectories)
			{
				size += getSize(sub);
			}

			if (size <= 100_000) { sizes.Add(size); }

			return size;
		}
	}

	[Theory]
	[InlineData(1_792_222)]
	public async Task SolvePart1(long expected)
	{
		var lines = await base.GetInputAsync().ToListAsync();
		var sizes = new List<long>();
		var root = Populate(lines);

		getSize(root);
		var actual = sizes.Sum();
		Assert.Equal(expected, actual);

		long getSize(Directory dir)
		{
			var size = 0L;

			foreach (var file in dir.Files)
			{
				size += file.Size;
			}

			foreach (var sub in dir.SubDirectories)
			{
				size += getSize(sub);
			}

			if (size <= 100_000) { sizes.Add(size); }

			return size;
		}
	}

	[Theory]
	[InlineData(
		24_933_642L,
		48_381_165L,
		21_618_835L,
		8_381_165L,
		new long[4] { 584, 94_853, 24_933_642, 48_381_165, },
		"$ cd /", "$ ls", "dir a", "14848514 b.txt", "8504156 c.dat", "dir d", "$ cd a", "$ ls", "dir e", "29116 f", "2557 g", "62596 h.lst", "$ cd e", "$ ls", "584 i", "$ cd ..", "$ cd ..", "$ cd d", "$ ls", "4060174 j", "8033020 d.log", "5626152 d.ext", "7214296 k")]
	public void SolveExample2(long expected, long expectedUsed, long expectedUnused, long expectedNeeded, long[] expectedSizes, params string[] lines)
	{
		var sizes = new List<long>();
		var root = Populate(lines);

		var used = getSize(root);
		Assert.Equal(expectedUsed, used);
		Assert.Equal(expectedSizes, sizes);
		var unused = 70_000_000 - used;
		Assert.Equal(expectedUnused, unused);
		var needed = 30_000_000 - unused;
		Assert.Equal(expectedNeeded, needed);
		var actual = sizes.Where(i => i > needed).Order().First();
		Assert.Equal(expected, actual);

		long getSize(Directory dir)
		{
			var size = 0L;

			foreach (var file in dir.Files)
			{
				size += file.Size;
			}

			foreach (var sub in dir.SubDirectories)
			{
				size += getSize(sub);
			}

			sizes.Add(size);
			return size;
		}
	}

	[Theory]
	[InlineData(41_035_571L)]
	public async Task SolvePart2_Total(long expected)
	{
		var lines = await base.GetInputAsync().ToListAsync();
		var root = Populate(lines);

		var actual = getSize(root);
		Assert.Equal(expected, actual);

		static long getSize(Directory dir)
		{
			var size = 0L;

			foreach (var file in dir.Files)
			{
				size += file.Size;
			}

			foreach (var sub in dir.SubDirectories)
			{
				size += getSize(sub);
			}

			return size;
		}
	}

	[Theory]
	[InlineData(1_112_963L)]
	public async Task SolvePart2(long expected)
	{
		var lines = await base.GetInputAsync().ToListAsync();
		var sizes = new List<long>();
		var root = Populate(lines);

		var used = getSize(root);
		var unused = 70_000_000 - used;
		var needed = 30_000_000 - unused;
		var actual = sizes.Where(i => i > needed).Order().First();
		Assert.Equal(expected, actual);

		long getSize(Directory dir)
		{
			var size = 0L;

			foreach (var file in dir.Files)
			{
				size += file.Size;
			}

			foreach (var sub in dir.SubDirectories)
			{
				size += getSize(sub);
			}

			sizes.Add(size);
			return size;
		}
	}

	private static Directory Populate(IEnumerable<string> lines)
	{
		Directory root, current;
		root = current = new Directory(name: "/", parent: null);

		foreach (var line in lines)
		{
			if (line == "$ cd /") { continue; }
			if (line == "$ ls") { continue; }

			if (line.StartsWith("dir "))
			{
				var name = line[4..];
				var parent = current;
				var subDirectory = new Directory(name, parent);
				current!.SubDirectories.Add(subDirectory);
				continue;
			}

			var match = FileOutputRegex().Match(line);

			if (match.Success)
			{
				var name = match.Groups[2].Value;
				var size = long.Parse(match.Groups[1].Value);
				var file = new File(name, size);
				current!.Files.Add(file);
				continue;
			}

			if (line == "$ cd ..")
			{
				current = current!.Parent!;
				continue;
			}

			if (line.StartsWith("$ cd "))
			{
				var name = line[5..];
				var subDir = current!.SubDirectories.Single(d => d.Name == name);
				current = subDir;
				continue;
			}

			throw new NotImplementedException();
		}

		return root;
	}

	private class Directory
	{
		public Directory(string name, Directory? parent)
		{
			Name = name;
			Parent = parent;
		}

		public string? Name { get; }
		public Directory? Parent { get; }
		public ICollection<File> Files { get; } = new List<File>();
		public ICollection<Directory> SubDirectories { get; } = new List<Directory>();
	}

	private readonly record struct File(string Name, long Size);
}
