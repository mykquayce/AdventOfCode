namespace AdventOfCode2021.Tests.Models;

[Flags]
public enum ChangeTypes : byte
{
	Decreased = 1,
	Increased = 2,
	NoChange = 4,
}
