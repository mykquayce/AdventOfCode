using System.Drawing;
using Xunit;

namespace AdventOfCode2021.Tests;

public class Day15
{
	[Fact]
	public void Test1()
	{
		var tree = new Dictionary<char, IDictionary<char, byte>>
		{
			['A'] = new Dictionary<char, byte>
			{
				['B'] = 6,
				['D'] = 1,
			},
			['B'] = new Dictionary<char, byte>
			{
				['A'] = 6,
				['C'] = 5,
				['D'] = 2,
				['E'] = 2,
			},
			['C'] = new Dictionary<char, byte>
			{
				['B'] = 5,
				['E'] = 5,
			},
			['D'] = new Dictionary<char, byte>
			{
				['A'] = 1,
				['B'] = 2,
				['E'] = 1,
			},
			['E'] = new Dictionary<char, byte>
			{
				['B'] = 2,
				['C'] = 5,
				['D'] = 1,
			},
		};

		var results = tree.Keys
			.Select(vertex => (vertex, (distance: int.MaxValue, previous: '\0')))
			.ToDictionary();
		results['A'] = (0, '\0');
		var unvisited = tree.Keys.ToList();

		while (unvisited.Any())
		{
			var (current, distanceFromStart) = (from kvp in results
												where unvisited.Contains(kvp.Key)
												orderby kvp.Value.distance
												select (kvp.Key, kvp.Value.distance)
											   ).FirstOrDefault();

			var neighbors = tree[current];

			foreach (var (neighbor, distanceToNext) in neighbors)
			{
				var distance = distanceFromStart + distanceToNext;

				if ((distance) < results[neighbor].distance)
				{
					results[neighbor] = (distance, current);
				}
			}
			unvisited.Remove(current);
		}

		// get the route
		IEnumerable<char> recurse(char current)
		{
			if (current == '\0') yield break;
			yield return current;
			var (_, previous) = results[current];
			foreach (var c in recurse(previous))
			{
				yield return c;
			}
		}

		var route = recurse('C').Reverse().ToList();
	}

	[Theory]
	[InlineData(@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581", new byte[] { 1, 0, }, 1)]
	[InlineData(@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581", new byte[] { 2, 0, }, 7)]
	[InlineData(@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581", new byte[] { 9, 9, }, 40)]
	[InlineData(@"6163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581", new byte[] { 9, 9, }, 40)]
	[InlineData(@"11637517422274862853338597396444961841755517295286
13813736722492484783351359589446246169155735727126
21365113283247622439435873354154698446526571955763
36949315694715142671582625378269373648937148475914
74634171118574528222968563933317967414442817852555
13191281372421239248353234135946434524615754563572
13599124212461123532357223464346833457545794456865
31254216394236532741534764385264587549637569865174
12931385212314249632342535174345364628545647573965
23119445813422155692453326671356443778246755488935
22748628533385973964449618417555172952866628316397
24924847833513595894462461691557357271266846838237
32476224394358733541546984465265719557637682166874
47151426715826253782693736489371484759148259586125
85745282229685639333179674144428178525553928963666
24212392483532341359464345246157545635726865674683
24611235323572234643468334575457944568656815567976
42365327415347643852645875496375698651748671976285
23142496323425351743453646285456475739656758684176
34221556924533266713564437782467554889357866599146
33859739644496184175551729528666283163977739427418
35135958944624616915573572712668468382377957949348
43587335415469844652657195576376821668748793277985
58262537826937364893714847591482595861259361697236
96856393331796741444281785255539289636664139174777
35323413594643452461575456357268656746837976785794
35722346434683345754579445686568155679767926678187
53476438526458754963756986517486719762859782187396
34253517434536462854564757396567586841767869795287
45332667135644377824675548893578665991468977611257
44961841755517295286662831639777394274188841538529
46246169155735727126684683823779579493488168151459
54698446526571955763768216687487932779859814388196
69373648937148475914825958612593616972361472718347
17967414442817852555392896366641391747775241285888
46434524615754563572686567468379767857948187896815
46833457545794456865681556797679266781878137789298
64587549637569865174867197628597821873961893298417
45364628545647573965675868417678697952878971816398
56443778246755488935786659914689776112579188722368
55172952866628316397773942741888415385299952649631
57357271266846838237795794934881681514599279262561
65719557637682166874879327798598143881961925499217
71484759148259586125936169723614727183472583829458
28178525553928963666413917477752412858886352396999
57545635726865674683797678579481878968159298917926
57944568656815567976792667818781377892989248891319
75698651748671976285978218739618932984172914319528
56475739656758684176786979528789718163989182927419
67554889357866599146897761125791887223681299833479", new byte[] { 49, 49, }, 315)]
	public void ParseInputTests(string input, byte[] end, int expected)
	{
		var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

		var points = (from y in Enumerable.Range(0, lines.Length)
					  from x in Enumerable.Range(0, lines[y].Length)
					  select new Point(x, y)
					 ).ToList();

		var riskLookup = (from p in points
						  let c = lines[p.Y][p.X]
						  let i = byte.Parse(c.ToString())
						  select (p, i)
						 ).ToDictionary();

		var neighborsLookup = (from p in points
							   let nn = GetNeighbors(p).Where(points.Contains).ToList()
							   select (p, nn)
							  ).ToDictionary();

		var results = (from p in points
					   let totalRisk = int.MaxValue
					   let previous = default(Point?)
					   let tuple = (totalRisk, previous)
					   select (p, tuple)
					  ).ToDictionary();

		results[new Point(0, 0)] = (0, default);

		ICollection<Point> unvisited = points.ToList(); // actually copies :)

		while (unvisited.Any())
		{
			var (current, totalRisk) = (from kvp in results
										where unvisited.Contains(kvp.Key)
										orderby kvp.Value.totalRisk
										select (kvp.Key, kvp.Value.totalRisk)
									   ).First();

			var riskSoFar = results[current].totalRisk;
			var neighbors = neighborsLookup[current];

			foreach (var neighbor in neighbors)
			{
				var riskOfNeighbor = riskLookup[neighbor];
				var risk = riskSoFar + riskOfNeighbor;

				if (risk < results[neighbor].totalRisk)
				{
					results[neighbor] = (risk, current);
				}
			}

			unvisited.Remove(current);
		}

		var actual = results[new Point(end[0], end[1])].totalRisk;
		Assert.Equal(expected, actual);
	}

	public static IEnumerable<Point> GetNeighbors(Point point)
	{
		yield return new(point.X, point.Y - 1); // above;
		yield return new(point.X + 1, point.Y); // right;
		yield return new(point.X, point.Y + 1); // below;
		yield return new(point.X - 1, point.Y); // left;
	}

	[Theory]
	[InlineData("Day15.txt", 769)] // too low
	public async Task SolvePart1(string fileName, int expected)
	{
		var input = await fileName.ReadFileAsync();
		var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

		var points = (from y in Enumerable.Range(0, lines.Length)
					  from x in Enumerable.Range(0, lines[y].Length)
					  select new Point(x, y)
					 ).ToList();

		var riskLookup = (from p in points
						  let c = lines[p.Y][p.X]
						  let i = byte.Parse(c.ToString())
						  select (p, i)
						 ).ToDictionary();

		var neighborsLookup = (from p in points
							   let nn = GetNeighbors(p).Where(points.Contains).ToList()
							   select (p, nn)
							  ).ToDictionary();

		var results = (from p in points
					   let totalRisk = int.MaxValue
					   let previous = default(Point?)
					   let tuple = (totalRisk, previous)
					   select (p, tuple)
					  ).ToDictionary();

		results[new Point(0, 0)] = (0, default);

		ICollection<Point> unvisited = points.ToList(); // actually copies :)

		while (unvisited.Any())
		{
			var (current, totalRisk) = (from kvp in results
										where unvisited.Contains(kvp.Key)
										orderby kvp.Value.totalRisk
										select (kvp.Key, kvp.Value.totalRisk)
									   ).First();

			var riskSoFar = results[current].totalRisk;
			var neighbors = neighborsLookup[current];

			foreach (var neighbor in neighbors)
			{
				var riskOfNeighbor = riskLookup[neighbor];
				var risk = riskSoFar + riskOfNeighbor;

				if (risk < results[neighbor].totalRisk)
				{
					results[neighbor] = (risk, current);
				}
			}

			unvisited.Remove(current);
		}

		var bottomRight = new Point(points.Max(p => p.X), points.Max(p => p.Y));
		var actual = results[bottomRight].totalRisk;
		Assert.Equal(expected, actual);
	}
}
