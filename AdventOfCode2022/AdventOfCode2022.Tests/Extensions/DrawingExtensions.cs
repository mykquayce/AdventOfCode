namespace System.Drawing;

public static class DrawingExtensions
{
	public static void Deconstruct(this Point point, out int x, out int y)
	{
		x = point.X;
		y = point.Y;
	}

	public static bool Touching(this Point left, Point right)
	{
		return Math.Abs(left.X - right.X) <= 1 && Math.Abs(left.Y - right.Y) <= 1;
	}
}
