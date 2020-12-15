using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day14
	{
		[Theory]
		[InlineData(@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0", 64, 2, 8, 11, 7, 101, 8, 0)]
		public void Example1(string s, params int[] values)
		{
			var program = DockingProgram.Parse(s);

			Assert.Equal((uint)values[0], program.Or);
			Assert.Equal((uint)values[1], program.Xor);
			Assert.Equal(values[2], program.Routines[0].Location);
			Assert.Equal((uint)values[3], program.Routines[0].Value);
			Assert.Equal(values[4], program.Routines[1].Location);
			Assert.Equal((uint)values[5], program.Routines[1].Value);
			Assert.Equal(values[6], program.Routines[2].Location);
			Assert.Equal((uint)values[7], program.Routines[2].Value);
		}

		[Theory]
		[InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 64, 2)]
		[InlineData("101010X0100X010X1X000X101X00010X0X11", 45_236_103_235, 22_928_397_224)]
		public void MaskConvertTest(string s, ulong expectedOrMask, ulong expectedXorMask)
		{
			var (orMask, xorMask) = DockingProgram.GetMasks(s);

			Assert.Equal(expectedOrMask, orMask);
			Assert.Equal(expectedXorMask, xorMask);
		}

		[Theory]
		[InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 11, 73)]
		[InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 101, 101)]
		[InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 0, 64)]
		public void ApplyMasksTests(string mask, uint value, uint expected)
		{
			var (orMask, xorMask) = DockingProgram.GetMasks(mask);
			var result = ApplyMasks(value, orMask, xorMask);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0", 165)]
		public void Example2(string s, ulong expected)
		{
			var locations = new Dictionary<int, ulong>();
			var program = DockingProgram.Parse(s);

			foreach (var routine in program.Routines)
			{
				var result = ApplyMasks(routine.Value, program.Or, program.Xor);

				if (!locations.TryAdd(routine.Location, result))
				{
					locations[routine.Location] = result;
				}
			}

			Assert.Equal(
				expected,
				locations.GetValues().Sum());
		}

		[Theory]
		[InlineData("day14.txt", 100)]
		public async Task GetProgramsTests(string filename, int expected)
		{
			var programs = await GetProgramsAsync(filename).ToListAsync();

			Assert.NotNull(programs);
			Assert.Equal(expected, programs.Count);

			foreach (var program in programs)
			{
				Assert.NotNull(program);
				Assert.NotEqual(default, program.Or);
				Assert.NotEqual(default, program.Xor);
				Assert.NotNull(program.Routines);
				Assert.NotEmpty(program.Routines);

				foreach (var routine in program.Routines)
				{
					Assert.NotNull(routine);
					Assert.NotEqual(default, routine.Location);
					Assert.NotEqual(default, routine.Value);
				}
			}
		}

		[Theory]
		[InlineData("day14.txt", 17_481_577_045_893)]
		public async Task Part1(string filename, ulong expected)
		{
			var locations = new Dictionary<int, ulong>();

			await foreach (var program in GetProgramsAsync(filename))
			{
				foreach (var routine in program.Routines)
				{
					var result = ApplyMasks(routine.Value, program.Or, program.Xor);

					if (!locations.TryAdd(routine.Location, result))
					{
						locations[routine.Location] = result;
					}
				}
			}

			Assert.Equal(
				expected,
				locations.Values.Sum());
		}

		[Theory]
		//[InlineData("000000000000000000000000000000X1001X", 18, 19, 50, 51)]
		[InlineData("00X0000X", 0, 1, 32, 33)]
		public void GetAllMasksTests(string s, params int[] masks)
		{
			//s.Replace('X', '1')

			var values = Enumerable.Range(0, byte.MaxValue).Select(b => b & 0b0010_0001).Distinct().ToList();
		}

		private static IEnumerable<ulong> GetAllMasks(uint floatingMask)
		{
			//return Enumerable.Range(0, ulong.MaxValue).Select(ul => ul & floatingMask).Distinct()
			throw new NotImplementedException();
		}

		[Theory]
		[InlineData("00X0000X", 0, 5)]
		[InlineData("0000X0XX", 0, 1, 3)]
		public void GetIndices(string s, params int[] expected)
		{
			var source = new[] { 0, 1, };
			var destination = new[] { 0, 5, };

			foreach (var value in new[] { 0, 1, 2, 3, })
			{
				var binary = Convert.ToString(value, toBase: 2).PadLeft(totalWidth: 2, paddingChar: '0');

				var result = new char[8];

				for (var a = 0; a < binary.Length; a++)
				{
					result[destination[a]] = s[source[a]];
				}
			}

			for (var index = 0; index < s.Length; index++)
			{
				var @char = s[index];

				if (@char == 'X')
				{

				}
			}
			/*
			// count the x's
			var count = s.Count(c => c == 'X');
			// for 2 ^ n
			var values = Enumerable.Range(0, (int)Math.Pow(2, count)).Select(i => Convert.ToString(i, toBase: 2).PadLeft(totalWidth: count, paddingChar: '0')).ToList();


			var indices = s.Select((c, i) => (c, i)).Where(t => t.c == 'X').Select(t => t.i).ToList();

			var count = (int)Math.Pow(2, indices.Count);

			for (var a = 0; a < indices.Count; a++)
			{
				var binary = Convert.ToString(a, toBase: 2).PadLeft(totalWidth: 64, paddingChar: '0');
				var source = a;
				var destination = indices[a];
				var value = binary[source];
			}

			foreach ((char @char, int index) in s.Select((c, i) => (c, i)).Where(t => t.c == 'X'))
			{

			}

			var actual = ;

			Assert.Equal(expected, indices);*/
		}



		private static ulong ApplyMasks(ulong value, ulong or, ulong xor) => (value | or) & ~xor;

		private async static IAsyncEnumerable<DockingProgram> GetProgramsAsync(string filename)
		{
			var text = await filename.ReadAllTextAsync();
			var matches = Regex.Matches(text, @"mask = [01X]+\r?\n?(?:mem\[\d+\] = \d+\r?\n?)+");

			foreach (Match match in matches)
			{
				var s = match.Groups[0].Value;
				var program = DockingProgram.Parse(s);
				yield return program;
			}
		}
	}

	public record DockingProgram(ulong Or, ulong Xor, IList<DockingRoutine> Routines)
	{
		private readonly static Regex _regex = new("^mask = ([01X]+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

		public static DockingProgram Parse(string s)
		{
			var lines = s.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

			var match = _regex.Match(lines[0]);

			var (or, xor) = GetMasks(match.Groups[1].Value);

			var routines = lines.Skip(1).Select(DockingRoutine.Parse).ToList();

			return new(or, xor, routines);
		}

		public static (ulong or, ulong xor) GetMasks(string s)
		{
			ulong or = 0, xor = 0;

			foreach (var c in s)
			{
				or <<= 1;
				xor <<= 1;

				or |= (ulong)(c == '1' ? 1 : 0);
				xor |= (ulong)(c == '0' ? 1 : 0);
			}

			return (or, xor);
		}
	}

	public record DockingRoutine(int Location, uint Value)
	{
		private readonly static Regex _regex = new(@"^mem\[(\d+)\] = (\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);

		public static DockingRoutine Parse(string s)
		{
			var match = _regex.Match(s);

			var location = int.Parse(match.Groups[1].Value);
			var value = uint.Parse(match.Groups[2].Value);

			return new(location, value);
		}
	}
}
