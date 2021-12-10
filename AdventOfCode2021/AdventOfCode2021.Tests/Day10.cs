using Xunit;

namespace AdventOfCode2021.Tests;

public class Day10
{
	[Theory]
	[InlineData("()")]
	[InlineData("[]")]
	[InlineData("([])")]
	[InlineData("{()()()}")]
	[InlineData("<([{}])>")]
	[InlineData("[<>({}){}[([])<>]]")]
	[InlineData("(((((((((())))))))))")]
	public void Test1(string line)
	{
		var openers = new Dictionary<char, char>
		{
			['('] = ')',
			['['] = ']',
			['{'] = '}',
			['<'] = '>',
		};
		var closers = new Dictionary<char, char>
		{
			[')'] = '(',
			[']'] = '[',
			['}'] = '{',
			['>'] = '<',
		};

		var stack = new Stack<char>();
		foreach (char @char in line)
		{
			if (openers.ContainsKey(@char))
			{
				stack.Push(@char);
				continue;
			}

			var expectedOpener = closers[@char];
			var actualOpener = stack.Pop();

			if (actualOpener != expectedOpener)
			{
				throw new Exception($"Expected {expectedOpener} but found {actualOpener}");
			}
		}
	}

	[Theory]
	[InlineData("[({(<(())[]>[[{[]{<()<>>", default)]
	[InlineData("[(()[<>])]({[<{<<[]>>(", default)]
	[InlineData("{([(<{}[<>[]}>{[]{[(<()>", '}')]
	[InlineData("(((({<>}<{<{<>}{[]{[]{}", default)]
	[InlineData("[[<[([]))<([[{}[[()]]]", ')')]
	[InlineData("[{[{({}]{}}([{[{{{}}([]", ']')]
	[InlineData("{<[[]]>}<{[{[{[]{()[[[]", default)]
	[InlineData("[<(<(<(<{}))><([]([]()", ')')]
	[InlineData("<{([([[(<>()){}]>(<<{{", '>')]
	[InlineData("<{([{{}}[<[[[<>{}]]]>[]]", default)]
	public void FindIncompleteTests(string line, char? expectedUnexpectedChar)
	{
		try
		{
			ProcessLine(line);
		}
		catch (LineCorruptedException ex)
		{
			Assert.Equal(expectedUnexpectedChar, ex.UnexpectedChar);
		}
		catch (IncompleteLineException) { }
	}

	[Theory]
	[InlineData(@"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]", 26_397)]
	public void Test2(string input, int expected)
	{
		var actual = 0;
		foreach (var line in input.Split(Environment.NewLine))
		{
			try
			{
				ProcessLine(line);
			}
			catch (LineCorruptedException ex)
			{
				var score = _corruptedScores[ex.UnexpectedChar];
				actual += score;
			}
			catch (IncompleteLineException) { }
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day10.txt", 392_421)]
	public async Task SolvePart1(string fileName, int expected)
	{
		var actual = 0;
		await foreach (var line in fileName.ReadLinesAsync())
		{
			try
			{
				ProcessLine(line);
			}
			catch (LineCorruptedException ex)
			{
				var score = _corruptedScores[ex.UnexpectedChar];
				actual += score;
			}
			catch (IncompleteLineException) { }
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("[({(<(())[]>[[{[]{<()<>>", "}}]])})]")]
	[InlineData("[(()[<>])]({[<{<<[]>>(", ")}>]})")]
	[InlineData("(((({<>}<{<{<>}{[]{[]{}", "}}>}>))))")]
	[InlineData("{<[[]]>}<{[{[{[]{()[[[]", "]]}}]}]}>")]
	[InlineData("<{([{{}}[<[[[<>{}]]]>[]]", "])}>")]
	public void Test3(string line, string expected)
	{
		try { ProcessLine(line); }
		catch (IncompleteLineException ex)
		{
			var missingChars = ex.SuperfluousChars.Select(c => _openers[c]);
			Assert.Equal(expected, missingChars);
		}
	}

	[Theory]
	[InlineData("}}]])})]", 288_957)]
	[InlineData(")}>]})", 5_566)]
	[InlineData("}}>}>))))", 1_480_781)]
	[InlineData("]]}}]}]}>", 995_444)]
	[InlineData("])}>", 294)]
	public void Test4(string missingChars, int expected)
	{
		var actual = 0;
		foreach (char @char in missingChars)
		{
			actual *= 5;
			var score = _incompleteScores[@char];
			actual += score;
		}
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("Day10.txt", 2_769_449_099)]
	public async Task SolvePart2(string fileName, int expected)
	{
		var scores = new List<long>();
		await foreach (var line in fileName.ReadLinesAsync())
		{
			try { ProcessLine(line); }
			catch (LineCorruptedException) { }
			catch (IncompleteLineException ex)
			{
				var missingChars = ex.SuperfluousChars.Select(c => _openers[c]);
				var score = 0L;
				foreach (char @char in missingChars)
				{
					score *= 5;
					score += _incompleteScores[@char];
				}
				scores.Add(score);
			}
		}
		var actual = scores.OrderBy(l => l).Skip((int)(scores.Count / 2d)).First();
		Assert.Equal(expected, actual);
	}

	private readonly static IReadOnlyDictionary<char, char> _openers = new Dictionary<char, char>
	{
		['('] = ')',
		['['] = ']',
		['{'] = '}',
		['<'] = '>',
	};

	private readonly static IReadOnlyDictionary<char, char> _closers = new Dictionary<char, char>
	{
		[')'] = '(',
		[']'] = '[',
		['}'] = '{',
		['>'] = '<',
	};

	private readonly static IReadOnlyDictionary<char, int> _corruptedScores = new Dictionary<char, int>
	{
		[')'] = 3,
		[']'] = 57,
		['}'] = 1_197,
		['>'] = 25_137,
	};

	private readonly static IReadOnlyDictionary<char, int> _incompleteScores = new Dictionary<char, int>
	{
		[')'] = 1,
		[']'] = 2,
		['}'] = 3,
		['>'] = 4,
	};

	private static void ProcessLine(string line)
	{
		var stack = new Stack<char>();
		foreach (char @char in line)
		{
			if (_openers.ContainsKey(@char))
			{
				stack.Push(@char);
				continue;
			}

			var expectedOpener = _closers[@char];
			var actualOpener = stack.Peek();

			if (expectedOpener != actualOpener)
			{
				throw new LineCorruptedException(@char);
			}

			stack.Pop();
			continue;
		}

		if (stack.Any())
		{
			throw new IncompleteLineException(stack.ToArray());
		}
	}

	public class LineCorruptedException : Exception
	{
		public LineCorruptedException(char unexpectedChar)
		{
			UnexpectedChar = unexpectedChar;
		}

		public char UnexpectedChar { get; }
	}

	public class IncompleteLineException : Exception
	{
		public IncompleteLineException(IReadOnlyList<char> superfluousChars)
		{
			SuperfluousChars = superfluousChars;
		}

		public IReadOnlyList<char> SuperfluousChars { get; }
	}
}
