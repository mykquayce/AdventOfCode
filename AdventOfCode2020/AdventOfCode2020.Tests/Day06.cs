using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day06
	{
		[Theory]
		[InlineData("abcxyz", "abcx", "abcy", "abcz")]
		public void Example1(string expected, params string[] inputs)
		{
			var deduped = GetDedupedAnswers(inputs).ToArray();
			var actual = new string(deduped);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"abc

a
b
c

ab
ac

a
a
a
a

b", 1, 3, 2, 4, 1)]
		public void Example2(string text, params int[] expected)
		{
			var groups = GetGroups(text).ToList();

			Assert.NotNull(groups);
			Assert.NotEmpty(groups);
			Assert.DoesNotContain(default, groups);
			Assert.Equal(expected.Length, groups.Count);

			for (var a = 0; a < groups.Count; a++)
			{
				var people = GetPeople(groups[a]).ToList();

				Assert.NotNull(people);
				Assert.NotEmpty(people);
				Assert.DoesNotContain(default, people);
				Assert.Equal(expected[a], people.Count);
			}
		}

		[Theory]
		[InlineData(@"abc

a
b
c

ab
ac

a
a
a
a

b", 11)]
		public void Example3(string text, int expected)
		{
			var count = 0;

			foreach (var group in GetGroups(text))
			{
				var people = GetPeople(group);
				var answers = GetDedupedAnswers(people);
				count += answers.Count();
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("day06.txt", 471)]
		public async Task GroupsCheck(string filename, int expected)
		{
			var text = await filename.ReadAllTextAsync();
			var groups = GetGroups(text).ToList();
			Assert.NotNull(groups);
			Assert.NotEmpty(groups);
			Assert.Equal(expected, groups.Count);
			Assert.DoesNotContain(default, groups);
			Assert.DoesNotContain(string.Empty, groups);
		}

		[Theory]
		[InlineData("day06.txt", 6_291)]
		public async Task Part1(string filename, int expected)
		{
			var count = 0;
			var text = await filename.ReadAllTextAsync();

			foreach (var group in GetGroups(text))
			{
				var people = GetPeople(group);
				var answers = GetDedupedAnswers(people);
				count += answers.Count();
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("abc", "abc")]
		[InlineData(@"a
b
c", "")]
		[InlineData(@"ab
ac", "a")]
		[InlineData(@"a
a
a
a", "a")]
		[InlineData("b", "b")]
		public void Example4(string text, string expected)
		{
			var people = GetPeople(text).ToList();
			var answers = GetProperDedupedAnswers(people);
			Assert.Equal(expected, answers);
		}

		[Theory]
		[InlineData(@"abc

a
b
c

ab
ac

a
a
a
a

b", 6)]
		public void Example5(string text, int expected)
		{
			var count = 0;
			var groups = GetGroups(text);

			foreach (var group in groups)
			{
				var people = GetPeople(group).ToList();
				var answers = GetProperDedupedAnswers(people);
				count += answers.Count();
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("day06.txt", 3_052)]
		public async Task Part2(string filename, int expected)
		{
			var text = await filename.ReadAllTextAsync();
			var count = 0;
			var groups = GetGroups(text);

			foreach (var group in groups)
			{
				var people = GetPeople(group).ToList();
				var answers = GetProperDedupedAnswers(people);
				count += answers.Count();
			}

			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData(@"cady
ipldcyf
xybgcd
gcdy
dygbc", "cdy")]
		[InlineData(@"wtifbkzvuhnomscrxq
nvcqhximsubtrfokwz
bqznfxwohmkvutirsc
ntusvqiwhzcoxfrbkm", "bcfhikmnoqrstuvwxz")]
		[InlineData(@"cpbly
clpbye
ycblp", "bclpy")]
		[InlineData(@"wmaxezdjgrocuksbp
cuxkdjoea
efxuaockjd
kceoxldauj", "acdejkoux")]
		[InlineData(@"nxhayv
npqfohbrl
kegchuidstwnm
nzhlj
", "hn")]
		public void Example6(string group, string expected)
		{
			var people = GetPeople(group).ToList();
			var answers = GetProperDedupedAnswers(people);
			var actual = new string(answers.ToArray());
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day06.txt", @"nxhayv
npqfohbrl
kegchuidstwnm
nzhlj")]
		public async Task CheckLastGroup(string filename, string expected)
		{
			var text = await filename.ReadAllTextAsync();
			var groups = GetGroups(text);
			var last = groups.Last();
			Assert.Equal(expected, last);
		}

		private static IEnumerable<string> GetGroups(string text) => text.Split(Environment.NewLine + Environment.NewLine);
		private static IEnumerable<string> GetPeople(string group) => group.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
		private static IEnumerable<char> GetDedupedAnswers(IEnumerable<string> people) => string.Concat(people).Dedupe();
		private static IEnumerable<char> GetProperDedupedAnswers(ICollection<string> people)
		{
			Assert.DoesNotContain(default, people);
			Assert.DoesNotContain(string.Empty, people);

			var allAnswers = string.Concat(people);

			var query = from c in allAnswers
						group c by c into gg
						where gg.Count() == people.Count
						orderby gg.Key
						select gg.Key;

			return query;
		}
	}
}
