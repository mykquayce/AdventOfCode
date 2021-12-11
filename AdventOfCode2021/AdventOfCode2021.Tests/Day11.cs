using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day11
{
	[Theory]
	[InlineData(@"11111
19991
19191
19991
11111", @"34543
40004
50005
40004
34543")]
	[InlineData(@"34543
40004
50005
40004
34543", @"45654
51115
61116
51115
45654")]
	[InlineData(@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526", @"6594254334
3856965822
6375667284
7252447257
7468496589
5278635756
3287952832
7993992245
5957959665
6394862637")]
	[InlineData(@"6594254334
3856965822
6375667284
7252447257
7468496589
5278635756
3287952832
7993992245
5957959665
6394862637", @"8807476555
5089087054
8597889608
8485769600
8700908800
6600088989
6800005943
0000007456
9000000876
8700006848")]
	[InlineData(@"8807476555
5089087054
8597889608
8485769600
8700908800
6600088989
6800005943
0000007456
9000000876
8700006848", @"0050900866
8500800575
9900000039
9700000041
9935080063
7712300000
7911250009
2211130000
0421125000
0021119000")]
	[InlineData(@"0050900866
8500800575
9900000039
9700000041
9935080063
7712300000
7911250009
2211130000
0421125000
0021119000", @"2263031977
0923031697
0032221150
0041111163
0076191174
0053411122
0042361120
5532241122
1532247211
1132230211")]
	[InlineData(@"2263031977
0923031697
0032221150
0041111163
0076191174
0053411122
0042361120
5532241122
1532247211
1132230211", @"4484144000
2044144000
2253333493
1152333274
1187303285
1164633233
1153472231
6643352233
2643358322
2243341322")]
	[InlineData(@"4484144000
2044144000
2253333493
1152333274
1187303285
1164633233
1153472231
6643352233
2643358322
2243341322", @"5595255111
3155255222
3364444605
2263444496
2298414396
2275744344
2264583342
7754463344
3754469433
3354452433")]
	[InlineData(@"5595255111
3155255222
3364444605
2263444496
2298414396
2275744344
2264583342
7754463344
3754469433
3354452433", @"6707366222
4377366333
4475555827
3496655709
3500625609
3509955566
3486694453
8865585555
4865580644
4465574644")]
	[InlineData(@"6707366222
4377366333
4475555827
3496655709
3500625609
3509955566
3486694453
8865585555
4865580644
4465574644", @"7818477333
5488477444
5697666949
4608766830
4734946730
4740097688
6900007564
0000009666
8000004755
6800007755")]
	[InlineData(@"7818477333
5488477444
5697666949
4608766830
4734946730
4740097688
6900007564
0000009666
8000004755
6800007755", @"9060000644
7800000976
6900000080
5840000082
5858000093
6962400000
8021250009
2221130009
9111128097
7911119976")]
	[InlineData(@"9060000644
7800000976
6900000080
5840000082
5858000093
6962400000
8021250009
2221130009
9111128097
7911119976", @"0481112976
0031112009
0041112504
0081111406
0099111306
0093511233
0442361130
5532252350
0532250600
0032240000")]
	public void Test1(string input, string expected)
	{
		var seafloor = Seafloor.Parse(input, default);
		seafloor.Increment();
		Assert.Equal(Seafloor.Parse(expected, default), seafloor);
	}

	[Theory]
	[InlineData(@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526", 10, 204)]
	[InlineData(@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526", 100, 1_656)]
	public void Test2(string input, int steps, int expectedFlashCount)
	{
		var seafloor = Seafloor.Parse(input, default);
		while (steps-- > 0) seafloor.Increment();
		Assert.Equal(expectedFlashCount, seafloor.FlashCount);
	}

	[Theory]
	[InlineData("Day11.txt", 100, 1_773)]
	public async Task SolvePart1(string fileName, int steps, int expectedFlashCount)
	{
		var seafloor = await fileName.ReadAndParseFileAsync<Seafloor>();
		while (steps-- > 0) seafloor.Increment();
		Assert.Equal(expectedFlashCount, seafloor.FlashCount);
	}

	[Theory]
	[InlineData(@"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526", 195)]
	public void Test3(string input, int expected)
	{
		var actual = 0;
		var seafloor = Seafloor.Parse(input, default);
		do
		{
			actual++;
			seafloor.Increment();
		}
		while (seafloor.Values.Any(i => i > 0));
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day11.txt", 494)]
	public async Task SolvePart2(string fileName, int expected)
	{
		var actual = 0;
		var seafloor = await fileName.ReadAndParseFileAsync<Seafloor>();
		do
		{
			actual++;
			seafloor.Increment();
		}
		while (seafloor.Values.Any(i => i > 0));
		Assert.Equal(expected, actual);
	}
}

public class Seafloor : Grid<int>, IParseable<Seafloor>
{
	public Seafloor(IDictionary<Point, int> dictionary) : base(dictionary) { }

	public IEnumerable<KeyValuePair<Point, int>> GetNeighbors(Point point)
	{
		return from kvp in this
			   where kvp.Key != point
			   let horizontal = Math.Abs(kvp.Key.X - point.X)
			   where horizontal < 2
			   let vertical = Math.Abs(kvp.Key.Y - point.Y)
			   where vertical < 2
			   where horizontal == 1 || vertical == 1
			   select kvp;
	}

	public int FlashCount { get; private set; } = 0;

	public void Increment()
	{
		// incrment all the points
		foreach (var (point, _) in this)
		{
			this[point]++;
		}

		// get each point > 9
		while (this.Values.Any(t => t > 9))
		{
			foreach (var (point, _) in this.Where(kvp => kvp.Value > 9))
			{
				FlashCount++;
				// set to zero
				this[point] = int.MinValue;
				// get the neighbors
				var neighbors = GetNeighbors(point);
				// increment them
				foreach (var (p, _) in neighbors)
				{
					this[p]++;
				}
			}
		}

		// set the negatives values to zero
		foreach (var (point, _) in this.Where(kvp => kvp.Value < 0))
		{
			// set them to zero
			this[point] = 0;
		}
	}

	#region iparseable implementation
	public new static Seafloor Parse(string s, IFormatProvider? provider)
	{
		IDictionary<Point, int> dictionary = Grid<int>.Parse(s, provider);
		return new(dictionary);
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Seafloor result)
	{
		if (Grid<int>.TryParse(s, provider, out var grid))
		{
			result = new(grid);
			return true;
		}
		result = default!;
		return false;
	}
	#endregion iparseable implementation
}

public class Grid<T> : Dictionary<Point, T>, IParseable<Grid<T>>
	where T : IParseable<T>
{
	public Grid(IDictionary<Point, T> dictionary) : base(dictionary) { }

	public T this[int x, int y]
	{
		get => this[new Point(x: x, y: y)];
		set => this[new Point(x: x, y: y)] = value;
	}

	#region iparseable implementation
	public static Grid<T> Parse(string s, IFormatProvider? provider)
		=> TryParse(s, provider, out var result) ? result : throw new Exception();

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Grid<T> result)
	{
		var dictionary = new Dictionary<Point, T>();
		var y = 0;
		foreach (var line in s!.Split(Environment.NewLine))
		{
			var x = 0;
			foreach (var @char in line)
			{
				var key = new Point(x: x, y: y);
				var ok = T.TryParse(@char.ToString(), provider, out var value);
				if (!ok)
				{
					result = default!;
					return false;
				}
				dictionary.Add(key, value);
				x++;
			}
			y++;
		}
		result = new Grid<T>(dictionary);
		return true;
	}
	#endregion iparseable implementation
}
