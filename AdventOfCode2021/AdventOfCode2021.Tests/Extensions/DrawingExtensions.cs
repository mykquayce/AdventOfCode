namespace System.Drawing;

public static class DrawingExtensions
{
	public static void Deconstruct(this Point point, out int x, out int y)
	{
		x = point.X;
		y = point.Y;
	}
}
