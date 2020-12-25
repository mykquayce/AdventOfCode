using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day20
	{
		[Theory]
		[InlineData(@"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###", 2_311)]
		public void GetEdgesTests(string input, int expectedId)
		{
			var max = Math.Pow(2, 10);
			var tile = Tile.Parse(input);

			Assert.NotNull(tile);
			Assert.Equal(expectedId, tile.Id);
			Assert.NotNull(tile.Checksums);
			Assert.NotEmpty(tile.Checksums);
			Assert.Equal(8, tile.Checksums.Count);
			Assert.All(tile.Checksums, i => Assert.InRange(i, 1, max));
		}

		[Theory]
		[InlineData("day20_example1.txt", 1_171, 1_951, 2_971, 3_079)]
		public async Task Example1(string filename, params int[] expected)
		{
			var text = await filename.ReadAllTextAsync();
			var tiles = text
				.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(Tile.Parse)
				.ToList();

			var corners = GetCorners(tiles).ToList();

			Assert.Equal(4, corners.Count);
			Assert.Equal(expected, corners.Select(tile => tile.Id).OrderBy(i => i));
		}

		private static IEnumerable<Tile> GetCorners(IEnumerable<Tile> tiles)
		{
			var edgeChecksums = (from tile in tiles
								 from checksum in tile.Checksums
								 group checksum by checksum into gg
								 where gg.Count() == 2
								 select gg.Key
								).ToList();

			foreach (var tile in tiles)
			{
				var count = tile.Checksums.Intersect(edgeChecksums).Count();

				switch (count)
				{
					case 4:
						yield return tile;
						break;
					case 6:
						break;
					case 8:
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(count), count, $"Unexpected {nameof(count)} {count:D}");
				}
			}
		}

		[Theory]
		[InlineData("day20.txt", 17_712_468_069_479)]
		public async Task Part1(string filename, long expected)
		{
			var text = await filename.ReadAllTextAsync();
			var tiles = text
				.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(Tile.Parse)
				.ToList();

			var corners = GetCorners(tiles).ToList();

			Assert.Equal(4, corners.Count);
			var actual = corners.Select(tile => (long)tile.Id).Product();
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"#...##.#..
..#.#..#.#
.###....#.
###.##.##.
.###.#####
.##.#....#
#...######
.....#..##
#.####...#
#.##...##.", @".#.#..#.
###....#
##.##.##
###.####
##.#....
...#####
....#..#
.####...")]
		public void RemoveBordersTests(string input, string expected)
		{
			var actual = string.Join(Environment.NewLine,
				Regex.Matches(input, @"^.(.{8}).\r?$", RegexOptions.Multiline).Select(match => match.Groups[1].Value).ToArray()[1..^1]
				);

			Assert.Equal(expected, actual);
		}
	}

	public record Tile(int Id, IList<ushort> Checksums)
	{
		public static Tile Parse(string s)
		{
			var lines = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

			var match = Regex.Match(lines[0], @"^Tile (\d+):\r?$");
			var id = int.Parse(match.Groups[1].Value);

			var checksums = GetEdges(lines[1..]).SelectMany(GetBinaryValues).ToList();

			return new Tile(id, checksums);
		}

		private static IEnumerable<IEnumerable<char>> GetEdges(IList<string> lines)
		{
			yield return lines[0];
			yield return lines[^1];
			yield return Enumerable.Range(0, lines.Count).Select(y => lines[y][0]);
			yield return Enumerable.Range(0, lines.Count).Select(y => lines[y][^1]);
		}

		private static IEnumerable<ushort> GetBinaryValues(IEnumerable<char> chars)
		{
			var binaryString = new string(chars.Select(c => c == '#' ? '1' : '0').ToArray());
			yield return binaryString.ToBinary();
			yield return new string(binaryString.Reverse().ToArray()).ToBinary();
		}
	}
}
