using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day16
	{
		[Theory]
		[InlineData(
			new[] { 7, 3, 47, 40, 4, 50, 55, 2, 20, 38, 6, 12, },
			71)]
		public void Example1(int[] values, int expected)
		{
			var sum = 0;

			var regices = new[]
			{
				new Regex(@"\b([1-3]|[5-7])\b"),
				new Regex(@"\b([6-9]|1[0-1]|3[3-9]|4[0-4])\b"),
				new Regex(@"\b(1[3-9]|2\d|3\d|40|4[5-9]|50)\b"),
			};

			foreach (var value in values)
			{
				var found = false;

				foreach (var regex in regices)
				{
					if (regex.IsMatch(value.ToString()))
					{
						found = true;
						break;
					}
				}

				if (!found) sum += value;
			}

			Assert.Equal(expected, sum);
		}

		[Theory]
		[InlineData(@"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12
", 71)]
		public void Example1_Attempt2(string s, int expected)
		{
			var rules = Rule.ParseRules(s).ToList();
			var tickets = Ticket.ParseTickets(s).ToList();
			var sum = tickets.SelectMany(t => t.GetInvalidValues(rules)).Sum();
			Assert.Equal(expected, sum);
		}

		private static IEnumerable<int> GetTicketValues(string s)
		{
			var start = s.IndexOf("ticket", StringComparison.InvariantCultureIgnoreCase);
			var substring = s[start..];
			var matches = Regex.Matches(substring, @"\d+");

			foreach (Match match in matches)
			{
				yield return int.Parse(match.Groups[0].Value);
			}
		}

		[Theory]
		[InlineData("day16.txt", 20_091)]
		public async Task Part1(string filename, int expected)
		{
			var s = await filename.ReadAllTextAsync();
			var rules = Rule.ParseRules(s).ToList();
			var tickets = Ticket.ParseTickets(s).ToList();
			var sum = tickets.SelectMany(t => t.GetInvalidValues(rules)).Sum();
			Assert.Equal(expected, sum);
		}

		[Theory]
		[InlineData(@"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9", "row", "class", "seat")]
		public void Example2(string s, params string[] expected)
		{
			var rules = Rule.ParseRules(s).ToList();
			var tickets = Ticket.ParseTickets(s).ToList();
			var actual = GetRuleNames(tickets, rules);
			Assert.Equal(expected, actual);
		}

		private static IList<string> GetRuleNames(IList<Ticket> tickets, IList<Rule> rules)
		{
			var count = tickets.First().Values.Count;

			var cube = new bool[rules.Count, tickets.Count, tickets[0].Values.Count];

			for (var x = 0; x < cube.GetLength(0); x++)
			{
				var rule = rules[x];

				for (var y = 0; y < cube.GetLength(1); y++)
				{
					var ticket = tickets[y];

					for (var z = 0; z < cube.GetLength(2); z++)
					{
						var value = ticket.Values[z];

						cube[x, y, z] = rule.Contains(value);
					}
				}
			}

			var possibilities = (from _ in Enumerable.Range(0, count)
								 let b = Enumerable.Range(0, rules.Count)
								 select b.ToList()
								).ToList();

			// for each rule, for each ticket, if a value fails that rule remove that rule from consideration

			for (var x = 0; x < cube.GetLength(0); x++)
			{
				for (var y = 0; y < cube.GetLength(1); y++)
				{
					for (var z = 0; z < cube.GetLength(2); z++)
					{
						if (!cube[x, y, z])
						{
							// value z, fails rule x, on ticket y
							possibilities[z].Remove(x);
						}
					}
				}
			}

			while (possibilities.Any(p => p.Count > 1))
			{
				for (var a = 0; a < possibilities.Count; a++)
				{
					if (possibilities[a].Count > 1) continue;

					for (var b = 0; b < possibilities.Count; b++)
					{
						if (a == b) continue;

						possibilities[b].Remove(possibilities[a][0]);
					}
				}
			}

			return possibilities.Select(p => p[0]).Select(i => rules[i].Name).ToList();
		}

		[Theory]
		[InlineData("day16.txt", 232_534_313_0651ul)]
		public async Task Part2(string filename, ulong expected)
		{
			var s = await filename.ReadAllTextAsync();
			var rules = Rule.ParseRules(s).ToList();
			var tickets = Ticket.ParseTickets(s).Where(t => t.IsValid(rules)).ToList();
			var names = GetRuleNames(tickets, rules);

			var product = 1ul;

			for (var a = 0; a < names.Count; a++)
			{
				if (!names[a].StartsWith("departure", StringComparison.InvariantCultureIgnoreCase)) continue;

				var ticket = tickets[0];
				product *= (ulong)ticket.Values[a];
			}

			Assert.Equal(expected, product);
		}
	}

	public record Rule(string Name, int Min1, int Max1, int Min2, int Max2)
	{
		private readonly static Regex _regex = new(@"^([\s\w]+): (\d+)-(\d+) or (\d+)-(\d+)\r?$", RegexOptions.Multiline);

		public bool Contains(int value) => (value >= Min1 && value <= Max1) || (value >= Min2 && value <= Max2);

		public static IEnumerable<Rule> ParseRules(string s)
		{
			var matches = _regex.Matches(s);

			foreach (Match match in matches)
			{
				var name = match.Groups[1].Value;
				var min1 = int.Parse(match.Groups[2].Value);
				var max1 = int.Parse(match.Groups[3].Value);
				var min2 = int.Parse(match.Groups[4].Value);
				var max2 = int.Parse(match.Groups[5].Value);

				yield return new Rule(name, min1, max1, min2, max2);
			}
		}
	}

	public record Ticket(IList<int> Values)
	{
		private readonly static Regex _regex = new(@"^(?:(\d+),?)+\r?$", RegexOptions.Multiline);

		public IEnumerable<int> GetInvalidValues(IEnumerable<Rule> rules)
		{
			foreach (var value in Values)
			{
				if (rules.Any(r => r.Contains(value))) continue;

				yield return value;
			}
		}

		public bool IsValid(IEnumerable<Rule> rules) => !GetInvalidValues(rules).Any();

		public static IEnumerable<Ticket> ParseTickets(string s)
		{
			var matches = _regex.Matches(s);

			foreach (Match match in matches)
			{
				var list = match.Groups[1].Captures.Select(c => c.Value).Select(int.Parse).ToList();

				yield return new Ticket(list);
			}
		}
	}
}
