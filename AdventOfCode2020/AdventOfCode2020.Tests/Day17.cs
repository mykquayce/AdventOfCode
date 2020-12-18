using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day17
	{
		[Theory]
		[InlineData(@".#.
..#
###
", false, true, false, false, false, true, true, true, true)]
		public void LoadInputTests(string s, params bool[] expected)
		{
			var dimension = Dimension.Parse(s);

			Assert.Equal(expected[0], dimension[x: 0, y: 0, z: 0]);
			Assert.Equal(expected[1], dimension[x: 1, y: 0, z: 0]);
			Assert.Equal(expected[2], dimension[x: 2, y: 0, z: 0]);
			Assert.Equal(expected[3], dimension[x: 0, y: 1, z: 0]);
			Assert.Equal(expected[4], dimension[x: 1, y: 1, z: 0]);
			Assert.Equal(expected[5], dimension[x: 2, y: 1, z: 0]);
			Assert.Equal(expected[6], dimension[x: 0, y: 2, z: 0]);
			Assert.Equal(expected[7], dimension[x: 1, y: 2, z: 0]);
			Assert.Equal(expected[8], dimension[x: 2, y: 2, z: 0]);
		}

		[Theory]
		[InlineData(@".#.
..#
###", 1, 11)]
		[InlineData(@".#.
..#
###", 2, 21)]
		[InlineData(@".#.
..#
###", 3, 38)]
		[InlineData(@".#.
..#
###", 6, 112)]
		public void DoCycleTest(string s, int cycles, int expected)
		{
			var dimension = Dimension.Parse(s);

			while (--cycles >= 0)
			{
				dimension = dimension.DoCycle();
			}

			Assert.Equal(expected, dimension.Count);
		}

		[Theory]
		[InlineData("day17.txt", 6, 207)]
		public async Task Part1(string filename, int cycles, int expected)
		{
			var s = await filename.ReadAllTextAsync();
			var dimension = Dimension.Parse(s);

			while (--cycles >= 0)
			{
				dimension = dimension.DoCycle();
			}

			Assert.Equal(expected, dimension.Count);
		}


		[Theory]
		[InlineData(@".#.
..#
###", 1, 29)]
		public void DoHyperCubeCycleTest(string s, int cycles, int expected)
		{
			var dimension = HyperDimension.Parse(s);

			while (--cycles >= 0)
			{
				dimension = dimension.DoCycle();
			}

			Assert.Equal(expected, dimension.Count);
		}
	}

	public record Cube(int X, int Y, int Z)
	{
		//public bool IsNear(int x, int y, int z) => X >= x - 1 && x + 1 >= X && Y >= y - 1 && y + 1 >= Y && Z >= z - 1 && z + 1 >= Z;
		public bool IsNear(int x, int y, int z) => X >= x - 1 && X <= x + 1 && Y >= y - 1 && Y <= y + 1 && Z >= z - 1 && Z <= z + 1;
	}

	public record HyperCube(IList<int> Coords)
	{
		public HyperCube(params int[] coords)
			: this(coords.ToList())
		{ }

		public bool IsNear(IList<int> coords)
		{
			for (var a = 0; a < coords.Count; a++)
			{
				int min = coords[a] - 1, max = coords[a] + 1;
				if (min <= Coords[a] && max >= Coords[a])
				{
					return true;
				}
			}

			return false;
		}
	}

	public class Dimension : ConcurrentBag<Cube>
	{
		public Dimension(IEnumerable<Cube> cubes)
			: base(cubes)
		{ }

		public bool this[int x, int y, int z] => this.Contains(new Cube(x, y, z));

		public IEnumerable<Cube> GetNearby(int x, int y, int z)
		{
			foreach (var cube in this)
			{
				if (cube.X == x && cube.Y == y && cube.Z == z) continue;

				if (cube.IsNear(x, y, z)) yield return cube;
			}
		}

		public IEnumerable<Cube> GetCorners()
		{
			int minX = 0, minY = 0, minZ = 0, maxX = 0, maxY = 0, maxZ = 0;

			foreach (var (x, y, z) in this)
			{
				if (x < minX) minX = x;
				if (y < minY) minY = y;
				if (z < minZ) minZ = z;
				if (x > maxX) maxX = x;
				if (y > maxY) maxY = y;
				if (z > maxZ) maxZ = z;
			}

			yield return new Cube(minX - 1, minY - 1, minZ - 1);
			yield return new Cube(minX - 1, minY - 1, maxZ + 1);
			yield return new Cube(minX - 1, maxY + 1, minZ - 1);
			yield return new Cube(minX - 1, maxY + 1, maxZ + 1);
			yield return new Cube(maxX + 1, minY - 1, minZ - 1);
			yield return new Cube(maxX + 1, minY - 1, maxZ + 1);
			yield return new Cube(maxX + 1, maxY + 1, minZ - 1);
			yield return new Cube(maxX + 1, maxY + 1, maxZ + 1);
		}

		public Dimension DoCycle()
		{
			var corners = GetCorners().ToList();
			int minX = 0, minY = 0, minZ = 0, maxX = 0, maxY = 0, maxZ = 0;

			foreach (var (x, y, z) in corners)
			{
				if (x < minX) minX = x;
				if (y < minY) minY = y;
				if (z < minZ) minZ = z;
				if (x > maxX) maxX = x;
				if (y > maxY) maxY = y;
				if (z > maxZ) maxZ = z;
			}

			var @new = new List<Cube>();

			for (var x = minX; x <= maxX; x++)
			{
				for (var y = minY; y <= maxY; y++)
				{
					for (var z = minZ; z <= maxZ; z++)
					{
						var active = this[x, y, z];
						var count = GetNearby(x, y, z).Take(4).Count();

						if (active)
						{
							if (count == 2 || count == 3)
							{
								@new.Add(new Cube(x, y, z));
							}
						}
						else
						{
							if (count == 3)
							{
								@new.Add(new Cube(x, y, z));
							}
						}
					}
				}
			}

			return new Dimension(@new);
		}

		public static Dimension Parse(string s)
		{
			var lines = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var rows = lines.Length;
			var cols = lines[0].Length;

			var cubes = new List<Cube>();

			for (var y = 0; y < rows; y++)
			{
				var line = lines[y];

				for (var x = 0; x < cols; x++)
				{
					var active = line[x] == '#';
					if (!active) continue;
					var cube = new Cube(x, y, 0);
					cubes.Add(cube);
				}
			}

			return new Dimension(cubes);
		}
	}

	public class HyperDimension : ConcurrentBag<HyperCube>
	{
		public HyperDimension(IEnumerable<HyperCube> cubes)
			: base(cubes)
		{ }

		public bool this[IList<int> coords] => this.Contains(new HyperCube(coords));

		public IEnumerable<HyperCube> GetCorners()
		{
			var count = this.First().Coords.Count;

			var mins = Enumerable.Repeat(0, count).ToList();
			var maxs = Enumerable.Repeat(0, count).ToList();

			foreach (var cube in this)
			{
				for (var a = 0; a < cube.Coords.Count; a++)
				{
					if (cube.Coords[a] < mins[a]) mins[a] = cube.Coords[a];
					if (cube.Coords[a] > maxs[a]) maxs[a] = cube.Coords[a];
				}
			}

			for (var a = 0; a < mins.Count; a++) { mins[a]--; maxs[a]++; }

			foreach (var coords in mins.GetCombinations(maxs))
			{
				yield return new HyperCube(coords);
			}
		}

		public HyperDimension DoCycle()
		{
			var count = this.First().Coords.Count;
			var mins = Enumerable.Repeat(0, count).ToList();
			var maxs = Enumerable.Repeat(0, count).ToList();

			// get the corners
			var corners = GetCorners().ToList();

			for (var a = 0; a < count; a++)
			{
				var values = corners.Select(c => c.Coords[a]).ToList();
				mins[a] = values.Min();
				maxs[a] = values.Max();
			}

			// get every cube in the space
			for (var a = 0; a < count; a++)
			{
				var min = mins[a];
				var max = maxs[a];

				for (var b = min; b <= max; b++)
				{

				}
			}

			throw new NotImplementedException();
		}

		public static HyperDimension Parse(string s)
		{
			var lines = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var rows = lines.Length;
			var cols = lines[0].Length;

			var cubes = new List<HyperCube>();

			for (var y = 0; y < rows; y++)
			{
				var line = lines[y];

				for (var x = 0; x < cols; x++)
				{
					var active = line[x] == '#';
					if (!active) continue;
					var cube = new HyperCube(x, y, 0, 0);
					cubes.Add(cube);
				}
			}

			return new HyperDimension(cubes);
		}

	}
}
