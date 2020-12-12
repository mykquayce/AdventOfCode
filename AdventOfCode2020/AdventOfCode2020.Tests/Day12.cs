using AdventOfCode2020.Tests.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day12
	{
		[Theory]
		[InlineData(new[] { "F10", "N3", "F7", "R90", "F11", }, 17, -8)]
		public void Example1(string[] inputs, int expectedEast, int expectedNorth)
		{
			var ship = Ship.Initialize();

			foreach (var input in inputs)
			{
				ship.Move(input);
			}

			Assert.Equal(expectedEast, ship.East);
			Assert.Equal(expectedNorth, ship.North);
		}

		[Theory]
		[InlineData("day12.txt", 962)]
		public async Task Part1(string filename, int expected)
		{
			var ship = Ship.Initialize();

			await foreach (var input in filename.ReadLinesAsync())
			{
				ship.Move(input);
			}

			Assert.Equal(expected, ship.ManhattanDistance);
		}

		[Theory]
		[InlineData("F10", 100, 10)]
		public void Example2(string input, int expectedEast, int expectedNorth)
		{
			var ship = RevisedShip.Initialize();

			ship.Move(input);

			Assert.Equal(expectedEast, ship.East);
			Assert.Equal(expectedNorth, ship.North);
		}

		[Theory]
		[InlineData(new[] { "F10", "N3", "F7", "R90", "F11", }, 214, -72)]
		public void Example3(string[] inputs, int expectedEast, int expectedNorth)
		{
			var ship = RevisedShip.Initialize();

			foreach (var input in inputs)
			{
				ship.Move(input);
			}

			Assert.Equal(expectedEast, ship.East);
			Assert.Equal(expectedNorth, ship.North);
		}

		[Theory]
		[InlineData("day12.txt", 56_135)]
		public async Task Part2(string filename, int expected)
		{
			var ship = RevisedShip.Initialize();

			await foreach (var input in filename.ReadLinesAsync())
			{
				ship.Move(input);
			}

			Assert.Equal(expected, ship.ManhattanDistance);
		}
	}

	public class RevisedShip : Ship
	{
		public int WayPointEast { get; set; }
		public int WayPointNorth { get; set; }

		public new void Move(string input)
		{
			var @char = input[0];
			var @int = int.Parse(input[1..]);

			switch (@char)
			{
				case 'F':
					while (@int-- > 0)
					{
						East += WayPointEast;
						North += WayPointNorth;
					}
					break;
				case 'N':
					WayPointNorth += @int;
					break;
				case 'E':
					WayPointEast += @int;
					break;
				case 'S':
					WayPointNorth -= @int;
					break;
				case 'W':
					WayPointEast -= @int;
					break;
				case 'L':
				case 'R':
					@int *= @char == 'L' ? -1 : 1;

					var theta = Math.Atan2(WayPointEast, WayPointNorth);
					var hypoteneuse = Math.Sqrt((WayPointEast * WayPointEast) + (WayPointNorth * WayPointNorth));
					var radians = (@int / 180d) * Math.PI;
					theta += radians;
					WayPointNorth = (int)Math.Round(Math.Cos(theta) * hypoteneuse);
					WayPointEast = (int)Math.Round(Math.Sin(theta) * hypoteneuse);
					break;
			}
		}

		public new static RevisedShip Initialize() => new() { East = 0, North = 0, Direction = 90, WayPointEast = 10, WayPointNorth = 1, };
	}

	public class Ship
	{
		public int East { get; set; }
		public int North { get; set; }
		public int Direction { get; set; }
		public int ManhattanDistance => Math.Abs(East) + Math.Abs(North);

		public void Move(string input)
		{
			var @char = input[0];
			var @int = int.Parse(input[1..]);

			switch (@char)
			{
				case 'F':
					(East, North) = Direction switch
					{
						0 => (East, North + @int),
						90 => (East + @int, North),
						180 => (East, North - @int),
						270 => (East - @int, North),
						_ => throw new ArgumentOutOfRangeException(nameof(input), input, $"unexpected {nameof(input)}: {input}"),
					};
					break;
				case 'N':
					North += @int;
					break;
				case 'S':
					North -= @int;
					break;
				case 'E':
					East += @int;
					break;
				case 'W':
					East -= @int;
					break;
				case 'R':
					Direction += @int;
					break;
				case 'L':
					Direction -= @int;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(input), input, $"unexpected {nameof(input)}: {input}");
			}

			Direction = (Direction + 360) % 360;
		}

		public static Ship Initialize() => new() { East = 0, North = 0, Direction = 90, };
	}
}
