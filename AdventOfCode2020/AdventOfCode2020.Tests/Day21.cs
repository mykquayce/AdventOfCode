using AdventOfCode2020.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests
{
	public class Day21
	{
		[Theory]
		[InlineData(@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)
", "kfcds", "nhms", "sbzzf", "sbzzf", "trh")]
		public void Example1(string input, params string[] expected)
		{
			var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var foods = lines.Select(Food.Parse).ToList();
			RemoveAllergicIngredients(foods).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			Assert.Equal(expected, foods.SelectMany(f => f.Ingredients).OrderBy(s => s));
		}

		private static IEnumerable<KeyValuePair<string, string>> RemoveAllergicIngredients(List<Food> foods)
		{
			// for each allergen
			while (foods.Any(f => f.Allergens.Any()))
			{
				var allergens = foods.SelectMany(f => f.Allergens).Distinct().ToList();

				foreach (var allergen in allergens)
				{
					string? ingredient = default;

					// foods that contain the allergen
					var allergics = foods.Where(f => f.Allergens.Contains(allergen)).ToList();

					switch (allergics.Count)
					{
						case 0: continue;
						case 1:
							// if those foods only have one ingredient and one allergen..
							if (allergics.Count == 1)
							{
								if (allergics[0].Ingredients.Count == 1 && allergics[0].Allergens.Count == 1)
								{
									ingredient = allergics[0].Ingredients[0];
								}
							}
							break;
						default:
							// find the overlap between the two foods
							var ingredients = (from s in allergics.SelectMany(a => a.Ingredients)
											   group s by s into gg
											   where gg.Count() == allergics.Count
											   select gg.Key
											).ToList();

							if (ingredients.Count == 1) ingredient = ingredients[0];
							break;
					}

					if (string.IsNullOrWhiteSpace(ingredient)) continue;

					yield return new KeyValuePair<string, string>(ingredient, allergen);

					for (var a = 0; a < foods.Count; a++)
					{
						foods[a].Ingredients.Remove(ingredient);
						foods[a].Allergens.Remove(allergen);
					}
				}
			}
		}

		[Theory]
		[InlineData("day21.txt", 2_461)]
		public async Task Part1(string filename, int expected)
		{
			var foods = await filename.ReadLinesAsync(Food.Parse).ToListAsync();
			RemoveAllergicIngredients(foods);
			Assert.Equal(expected, foods.SelectMany(f => f.Ingredients).Count());
		}

		[Theory]
		[InlineData(@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)
", "mxmxvkd,sqjhc,fvjkl")]
		public void Example2(string input, string expected)
		{
			var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			var foods = lines.Select(Food.Parse).ToList();
			var dictionary = RemoveAllergicIngredients(foods).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			var actual = string.Join(',', from kvp in dictionary
										  orderby kvp.Value
										  select kvp.Key);

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("day21.txt", "ltbj,nrfmm,pvhcsn,jxbnb,chpdjkf,jtqt,zzkq,jqnhd")]
		public async Task Part2(string filename, string expected)
		{
			var foods = await filename.ReadLinesAsync(Food.Parse).ToListAsync();
			var dictionary = RemoveAllergicIngredients(foods).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			var actual = string.Join(',', from kvp in dictionary
										  orderby kvp.Value
										  select kvp.Key);

			Assert.Equal(expected, actual);
		}
	}

	public record Food(IList<string> Ingredients, IList<string> Allergens)
	{
		public override string ToString() => $"{string.Join(' ', Ingredients)}(contains {string.Join(", ", Allergens)})";

		public static Food Parse(string s)
		{
			static IEnumerable<string> split(string s) => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimEnd(','));

			var match = Regex.Match(s, @"^(.+) \(contains (.+)\)$", RegexOptions.Multiline);

			var ingredients = split(match.Groups[1].Value).ToList();
			var allergens = split(match.Groups[2].Value).ToList();

			return new Food(ingredients, allergens);
		}
	}
}
