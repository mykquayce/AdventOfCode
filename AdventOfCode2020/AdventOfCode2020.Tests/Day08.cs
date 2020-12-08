using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day08
	{
		[Theory]
		[InlineData("nop +0", Operations.nop, 0)]
		[InlineData("acc +1", Operations.acc, 1)]
		[InlineData("jmp +4", Operations.jmp, 4)]
		[InlineData("acc +3", Operations.acc, 3)]
		[InlineData("jmp -3", Operations.jmp, -3)]
		[InlineData("acc -99", Operations.acc, -99)]
		[InlineData("jmp -4", Operations.jmp, -4)]
		[InlineData("acc +6", Operations.acc, 6)]
		public void ParseInstructionTest(string s, Operations expectedOperation, sbyte expectedArgument)
		{
			var (operation, argument) = Instruction.Parse(s);

			Assert.Equal(expectedOperation, operation);
			Assert.Equal(expectedArgument, argument);
		}

		[Theory]
		[InlineData(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6
", 9)]
		public void ParseInstructionsTest(string s, int expectedCount)
		{
			var program = Program.Parse(s);

			Assert.NotNull(program);
			Assert.NotEmpty(program);
			Assert.Equal(expectedCount, program.Count);

			Assert.All(program, Assert.NotNull);
			Assert.All(program, i => Assert.NotEqual(default, i.Operation));
		}

		[Theory]
		[InlineData(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6
", 5)]
		public void RunFailingProgramTest(string s, int expected)
		{
			var program = Program.Parse(s);
			try
			{
				RunProgram(program);
				Assert.True(false, "should've throw an exception");
			}
			catch (InfiniteLoopException ex)
			{
				Assert.Equal(expected, ex.Accumulator);
			}
		}

		private static int RunProgram(Program program)
		{
			var last = program.Count - 1;
			var indices = new List<int>();
			int accumulator = 0, index = 0;

			while (true)
			{
				var instruction = program[index];

				if (indices.Contains(index)) throw new InfiniteLoopException(accumulator);

				var @break = index == last;

				indices.Add(index);

				RunInstruction(instruction, ref accumulator, ref index);

				if (@break) break;
			}

			return accumulator;
		}

		private static void RunInstruction(Instruction instruction, ref int accumulator, ref int index)
		{
			(accumulator, index) = instruction.Operation switch
			{
				Operations.acc => (accumulator + instruction.Argument, index + 1),
				Operations.jmp => (accumulator, index + instruction.Argument),
				Operations.nop => (accumulator, index + 1),
				_ => throw new Exception(),
			};
		}

		[Theory]
		[InlineData("day08.txt", 1_446)]
		public async Task Part1(string filename, short expected)
		{
			var instructions = await filename.ReadLinesAsync(Instruction.Parse)
				.ToListAsync();

			var program = new Program(instructions);

			try
			{
				RunProgram(program);
				Assert.True(false, "should've throw an exception");
			}
			catch (InfiniteLoopException ex)
			{
				Assert.Equal(expected, ex.Accumulator);
			}
		}

		[Theory]
		[InlineData(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6
", 8)]
		public void Example1(string s, int expected)
		{
			var accumulator = 0;
			var program = Program.Parse(s);

			foreach (var fixedProgram in GetProgramIterations(program))
			{
				try
				{
					accumulator = RunProgram(fixedProgram);
				}
				catch (InfiniteLoopException) { }
			}

			Assert.Equal(expected, accumulator);
		}

		private static IEnumerable<Program> GetProgramIterations(Program program)
		{
			for (var a = 0; a < program.Count; a++)
			{
				var instruction = program[a];

				if (instruction.Operation == Operations.acc) continue;

				var fixedInstructions = new Instruction[program.Count];

				program.CopyTo(fixedInstructions, arrayIndex: 0);

				var fixedProgram = new Program(fixedInstructions)
				{
					[a] = FixInstruction(instruction)
				};

				yield return fixedProgram;
			}
		}

		private static Instruction FixInstruction(Instruction instruction)
			=> instruction.Operation switch
			{
				Operations.acc => instruction,
				Operations.jmp => instruction with { Operation = Operations.nop, },
				Operations.nop => instruction with { Operation = Operations.jmp, },
				_ => throw new Exception(),
			};

		[Theory]
		[InlineData("day08.txt", 1_403)]
		public async Task Part2(string filename, int expected)
		{
			var accumulator = 0;
			var instructions = await filename
				.ReadLinesAsync(Instruction.Parse)
				.ToListAsync();

			var program = new Program(instructions);

			foreach (var fixedProgram in GetProgramIterations(program))
			{
				try
				{
					accumulator = RunProgram(fixedProgram);
				}
				catch (InfiniteLoopException) { }
			}

			Assert.Equal(expected, accumulator);
		}
	}

	public record Program(IList<Instruction> Instructions) : IList<Instruction>
	{
		#region IList implementation
		public Instruction this[int index]
		{
			get => Instructions[index];
			set => Instructions[index] = value;
		}
		public void Add(Instruction item) => Instructions.Add(item);
		public void Clear() => Instructions.Clear();
		public bool Contains(Instruction item) => Instructions.Contains(item);
		public void CopyTo(Instruction[] array, int arrayIndex) => Instructions.CopyTo(array, arrayIndex);
		public int Count => Instructions.Count;
		public IEnumerator<Instruction> GetEnumerator() => Instructions.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Instructions.GetEnumerator();
		public int IndexOf(Instruction item) => Instructions.IndexOf(item);
		public void Insert(int index, Instruction item) => Instructions.Insert(index, item);
		public bool IsReadOnly => Instructions.IsReadOnly;
		public bool Remove(Instruction item) => Instructions.Remove(item);
		public void RemoveAt(int index) => Instructions.RemoveAt(index);
		#endregion IList implementation

		public static Program Parse(string s)
		{
			var instructions = s
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(Instruction.Parse)
				.ToList();

			return new Program(instructions);
		}
	}

	public record Instruction(Operations Operation, short Argument)
	{
		public static Instruction Parse(string s)
		{
			var values = s.Split(' ');

			var operation = Enum.Parse<Operations>(values[0]);
			var argument = short.Parse(values[1]);

			return new Instruction(operation, argument);
		}
	}

	[Flags]
	public enum Operations : byte
	{
		acc = 1,
		jmp = 2,
		nop = 4,
	}

	public class InfiniteLoopException : Exception
	{
		public InfiniteLoopException(int accumulator) : base("Infinite loop exception.")
		{
			Accumulator = accumulator;
			Data.Add(nameof(accumulator), accumulator);
		}

		public int Accumulator { get; }
	}

}
