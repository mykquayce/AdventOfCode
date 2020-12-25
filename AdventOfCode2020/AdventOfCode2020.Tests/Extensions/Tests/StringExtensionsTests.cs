using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2020.Tests.Extensions.Tests
{
	public class StringExtensionsTests
	{
		[Theory]
		[InlineData("day06.txt", @"cady
ipldcyf
xybgcd
gcdy
dygbc")]
		public async Task StreamOneGroupFromDay6Input(string filename, string expected)
		{
			var groups = filename.ReadGroupsAsync();
			var first = await groups.FirstAsync();
			Assert.Equal(expected, first);
		}

		[Theory]
		[InlineData("day06.txt", @"cady
ipldcyf
xybgcd
gcdy
dygbc", @"rwhvugmspoyzfbnlcxqtdj
avqdpntxrclufbjswgzh
qbvwgzpfsrjtdxnculh
jhrpclwdxgqibfsntzuv")]
		public async Task StreamTwoGroupsFromDay6Input(string filename, params string[] expected)
		{
			var groups = filename.ReadGroupsAsync();
			var first = await groups.Take(2).ToListAsync();
			Assert.Equal(expected, first);
		}

		[Theory]
		[InlineData("day06.txt", @"nxhayv
npqfohbrl
kegchuidstwnm
nzhlj")]
		public async Task LastGroupFromDay6Input(string filename, string expected)
		{
			var groups = filename.ReadGroupsAsync();
			var first = await groups.LastAsync();
			Assert.Equal(expected, first);
		}

		[Theory]
		[InlineData("0", 0)]
		[InlineData("1", 1)]
		[InlineData("10", 2)]
		[InlineData("100", 4)]
		[InlineData("1000", 8)]
		public void ToBinary(string input, ushort expected)
		{
			var actual = input.ToBinary();
			Assert.Equal(expected, actual);
		}
	}
}
