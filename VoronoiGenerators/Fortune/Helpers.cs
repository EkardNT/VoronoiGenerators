using System;
namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Contains utility method for various stages of Fortune's algorithm.
	/// </summary>
	public static class Helpers
	{
		/// <summary>
		/// Given three non-collinear points, finds the center of the
		/// circle that passes through all three points.
		/// </summary>
		/// <remarks>
		/// Given the points a, b, c, we want to find the circle center (h, k).
		/// If we call the radius of the circle r, we have the three equations
		///		1. (a.X - h)^2 + (a.Y - k)^2 = r^2
		///		2. (b.X - h)^2 + (b.Y - k)^2 = r^2
		///		3. (c.X - h)^2 + (c.Y - k)^2 = r^2
		///	This is three equations and three unknowns, but note we don't
		///	actually care about finding r here. I solved this by settings
		///	the left sides of equations 1 and 2, and 1 and 3 equal to each
		///	other, resulting in two equations with two unknowns, h and k.
		///	I solved for k first, then plugged k back into one of the two
		///	equations to solve for h.
		/// </remarks>
		public static Vector CircleCenter(Vector a, Vector b, Vector c)
		{
			// a.X - b.X can't be 0, so if it currently is rotate
			// the point definitions.
			if(a.X - b.X == 0)
			{
				var temp = a;
				a = b;
				b = c;
				c = temp;
			}

			// If either of these two denominators is 0, the sites are collinear.
			var kDenominator = 2 * (a.X * (b.Y - c.Y) - a.Y * (b.X - c.X) + b.X * c.Y - b.Y * c.X);
			var hDenominator = 2 * (a.X - b.X);
			if (kDenominator == 0 || hDenominator == 0)
				throw new ArgumentException("Collinear points.");

			var k = -(a.X * a.X * (b.X - c.X) - a.X * (b.X * b.X + b.Y * b.Y - c.X * c.X - c.Y * c.Y) + a.Y * a.Y * (b.X - c.X) + b.X * b.X * c.X - b.X * (c.X * c.X + c.Y * c.Y) + b.Y * b.Y * c.X) / kDenominator;
			var h = -(2 * k * (a.Y - b.Y) - a.X * a.X - a.Y * a.Y + b.X * b.X + b.Y * b.Y) / hDenominator;
			return new Vector(h, k);
		}
	}
}