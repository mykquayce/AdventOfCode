using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day03
	{
		private const string _exampleTrees = @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";

		[Theory]
		[InlineData(_exampleTrees, false, false, true, true, false, false, false, false, false, false, false)]
		public void ParseTress(string s, params bool[] expected)
		{
			var array = ParseTrees(s);

			var actual = Enumerable.Range(0, 11).Select(i => array[i, 0]).ToArray();

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"#..
..#", 3, 2)]
		public void ParseTreesTest(string trees, int width, int height)
		{
			var array = ParseTrees(trees);
			var size = array.GetSize();
			Assert.Equal(width, size.Width);
			Assert.Equal(height, size.Height);
		}

		private static bool[,] ParseTrees(string trees)
		{
			var lines = trees.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var size = new Size(lines[0].Length, lines.Length);

			var array = new bool[size.Width, size.Height];

			for (var row = 0; row < size.Height; row++)
			{
				for (var column = 0; column < size.Width; column++)
				{
					array[column, row] = lines[row][column] == '#';
				}
			}

			return array;
		}

		[Theory]
		[InlineData(_exampleTrees, 3, 1, 7)]
		public void GoTest(string treesString, int right, int down, int expected)
		{
			var trees = ParseTrees(treesString);
			var vector = new Size(width: right, height: down);

			var result = Go(trees, vector);
			var actual = result.Count(b => b);

			Assert.Equal(expected, actual);
		}

		private static IEnumerable<bool> Go(bool[,] trees, Size vector)
		{
			var point = new Point(x: 0, y: 0);
			var size = trees.GetSize();

			while (point.Y < size.Height)
			{
				yield return trees[point.X, point.Y];
				point += vector;
				point.X %= size.Width;
			}
		}

		[Theory]
		[InlineData("day03.txt", 3, 1, 200)]
		public async Task Part1(string filename, int right, int down, int expected)
		{
			var treesString = await filename.ReadAllTextAsync();
			var trees = ParseTrees(treesString);
			var vector = new Size(width: right, height: down);

			var results = Go(trees, vector);
			var actual = results.Count(b => b);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(_exampleTrees, 336, "1,1", "3,1", "5,1", "7,1", "1,2")]
		public void Part2_Example(string treesString, int expected, params string[] vectorsStrings)
		{
			var trees = ParseTrees(treesString);
			var vectors = StringsToVectors(vectorsStrings);

			var product = 1;

			foreach (var vector in vectors)
			{
				var results = Go(trees, vector);
				var count = results.Count(b => b);
				product *= count;
			}

			Assert.Equal(expected, product);
		}

		private static IEnumerable<Size> StringsToVectors(IEnumerable<string> strings)
		{
			return from s in strings
				   let ii = s.Split(',').Select(int.Parse).ToList()
				   let right = ii[0]
				   let down = ii[1]
				   let vector = new Size(width: right, height: down)
				   select vector;
		}

		[Theory]
		[InlineData("day03.txt", 3_737_923_200, "1,1", "3,1", "5,1", "7,1", "1,2")]
		public async Task Part2(string filename, long expected, params string[] vectorsStrings)
		{
			var treesString = await filename.ReadAllTextAsync();
			var trees = ParseTrees(treesString);
			var vectors = StringsToVectors(vectorsStrings);

			long product = 1;

			foreach (var vector in vectors)
			{
				var results = Go(trees, vector);
				var count = results.Count(b => b);
				product *= count;
			}

			Assert.Equal(expected, product);
		}
	}
}
