namespace System.Drawing;

public static class DrawingExtensions
{
	public static bool Touching(this Point left, Point right)
	{
		return Math.Abs(left.X - right.X) <= 1 && Math.Abs(left.Y - right.Y) <= 1;
	}
}
