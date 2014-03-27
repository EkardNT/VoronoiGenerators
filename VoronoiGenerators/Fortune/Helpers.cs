using System;
namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Contains utility method for various stages of Fortune's algorithm.
	/// </summary>
	internal static class Helpers
	{
		public const double Tolerance = 0.000001;

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
			if (a.X - b.X == 0)
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
				throw new CollinearPointsException(a, b, c);

			var k = -(a.X * a.X * (b.X - c.X) - a.X * (b.X * b.X + b.Y * b.Y - c.X * c.X - c.Y * c.Y) + a.Y * a.Y * (b.X - c.X) + b.X * b.X * c.X - b.X * (c.X * c.X + c.Y * c.Y) + b.Y * b.Y * c.X) / kDenominator;
			var h = -(2 * k * (a.Y - b.Y) - a.X * a.X - a.Y * a.Y + b.X * b.X + b.Y * b.Y) / hDenominator;
			return new Vector(h, k);
		}

		/// <summary>
		/// Gets the x coordinate of the breakpoint between two adjacent arcs on the
		/// beach line.
		/// </summary>
		/// <remarks>
		/// It is important to note the difference between finding the breakpoints
		/// between two sites versus between two arcs. Between any two sites there
		/// will be exactly two breakpoints, which trace out (in opposite directions)
		/// the Voronoi edge between the two sites. However, between adjacent arcs
		/// on the beach line there is exactly one breakpoint. If this confuses you,
		/// remember that if the parabola defined by a site is interrupted by one
		/// or more other sites' parabolas, then the separated parts of the original
		/// parabola are considered distinct arcs on the beach line, even though they
		/// are caused by the same site.
		/// 
		/// Another important fact is that although the given left and right arcs
		/// are indeed to the left and right of each other, respectively, there is
		/// no need at all for the site that causes the left arc to be located to
		/// the left of the site that causes the right arc. For a trivial example of
		/// this, consider the case where both sites are located on the same x-coordinate.
		/// </remarks>
		public static double GetDividingBreakpointX(double sweepLineY, BeachArc leftArc, BeachArc rightArc, bool breakVerticalTiesLeft)
		{
			double
				lx = leftArc.Site.Position.X,
				ly = leftArc.Site.Position.Y,
				rx = rightArc.Site.Position.X,
				ry = rightArc.Site.Position.Y;

			// Special case: if the two sites have the same y coordinate, 
			// then both breakpoints are tracing out a vertical line
			// perfectly centered between both sites (because Voronoi
			// edges are perpendicular *bisectors*). We handle this
			// case to avoid a division by 0 when we compute the 
			// breakpoint X coordinates.
			if (ly == ry)
				return 0.5 * (lx + rx);

			// The sweep line must be below both arcs' sites.
			if (sweepLineY >= leftArc.Site.Position.Y
				|| sweepLineY >= rightArc.Site.Position.Y)
				throw new ArgumentException("Sweep line must be below both arcs' sites.");

			// Equation of parabola for left arc is: y = la * x^2 + lb * x + lc
			double
				la = 1.0 / (2.0 * (ly - sweepLineY)),
				lb = 1.0 / (ly - sweepLineY),
				lc = (lx * lx + ly * ly - sweepLineY * sweepLineY) / (2.0 * (ly - sweepLineY));
			// Equation of parabola for right arc is: y = ra * x^2 + rb * x + rc
			double
				ra = 1.0 / (2.0 * (ry - sweepLineY)),
				rb = 1.0 / (ry - sweepLineY),
				rc = (rx * rx + ry * ry - sweepLineY * sweepLineY) / (2.0 * (ry - sweepLineY));

			// Solve for x coordinate first using quadratic equation.
			// (la - ra) * x^2 + (lb - rb) * x + (lc - rc) = 0
			double
				finalA = la - ra,
				finalB = lb - rb,
				finalC = lc - rc;

			// We always expect to find two real roots because there are two breakpoints
			// between any two pairs of sites, so discriminant should be greater than 0.
			double discriminant = finalB * finalB - 4.0 * finalA * finalC;
			if (discriminant <= 0)
				throw new ArgumentException("Discriminant was <= 0.");

			double
				leftBreakpointX = (-finalB - Math.Sqrt(discriminant)) / (2.0 * finalA),
				rightBreakpointX = (-finalB + Math.Sqrt(discriminant)) / (2.0 * finalA);

			// Now we must decide which of the two roots to return. We say that the 
			// arcs are 'normal' when the site that defines the left arc is to the
			// left of the site that defines the right arc, and that the arcs are
			// 'inverted' when left arc's site is actually to the right of the right
			// arc's site.
			//
			// Our condition is: 
			//		If the arcs are normal, return the right breakpoint. 
			//		If the arcs are inverted, return the left.
			if (lx < rx)
				return rightBreakpointX;
			else if (lx > rx)
				return leftBreakpointX;
			// There is an important special case to consider, however. If the sites'
			// x coordinates are the same, then one of them is vertically above the
			// other and the arcs are neither normal nor inverted. In this case, the
			// choice of which breakpoint should be returned belongs to whichever
			// algorithm is calling this method.
			else return breakVerticalTiesLeft ? leftBreakpointX : rightBreakpointX;
		}

		/// <summary>
		/// Determines whether the two breakpoints defined by three contiguous arcs
		/// on the beach line converge.
		/// </summary>
		/// <param name="leftArcSitePosition">The position of the site that defines the left arc.</param>
		/// <param name="middleArcSitePosition">The position of the site that defines the middle arc.</param>
		/// <param name="rightArcSitePosition">The position of the site that defines the right arc.</param>
		public static bool TestBreakpointsConvergence(Vector leftArcSitePosition, Vector middleArcSitePosition, Vector rightArcSitePosition)
		{
			if (AreCollinear(leftArcSitePosition, middleArcSitePosition, rightArcSitePosition))
				return false;

			// Compute parametric forms of the perpendicular bisectors, with
			// the base points on the line between both pairs of sites and
			// the positive positive directions of the lines pointing to the
			// right.
			Vector
				basePoint1 = 0.5 * (leftArcSitePosition + middleArcSitePosition),
				basePoint2 = 0.5 * (middleArcSitePosition + rightArcSitePosition);
			Vector
				direction1 = (middleArcSitePosition - leftArcSitePosition).Normalize().Perpendicular(),
				direction2 = (rightArcSitePosition - middleArcSitePosition).Normalize().Perpendicular();
			
			// Sanity check that neither of the two direction vectors' magnitudes are zero.
			if ((direction1.X == 0 && direction1.Y == 0) || (direction2.X == 0 && direction2.Y == 0))
				throw new SanityCheckFailedException("Both of a direction vector's components were 0.");

			// Check for perpendicularity
			if(direction2.X * direction1.Y == direction2.Y * direction1.X)
				return false;

			if(direction1.X != 0)
			{
				double 
					time2 = (direction1.X * (basePoint2.Y - basePoint1.Y) + direction1.Y * (basePoint1.X - basePoint2.X)) / (direction2.X * direction1.Y - direction2.Y * direction1.X),
					time1 = (time2 * direction2.X + basePoint2.X - basePoint1.X) / direction1.X;
				return time2 > 0 && time1 > 0;
			}
			else // direction1.Y != 0
			{
				double
					time2 = (direction1.X * (basePoint1.Y - basePoint2.Y) + direction1.Y * (basePoint2.X - basePoint1.X)) / (direction2.Y * direction1.X - direction2.X * direction1.Y),
					time1 = (time2 * direction1.Y + basePoint2.Y - basePoint1.Y) / direction1.Y;
				return time2 > 0 && time1 > 0;
			}
		}

		/// <summary>
		/// Determines whether three points are collinear.
		/// </summary>
		/// <remarks>
		/// Works by finding the area of the triangle enclosed by the
		/// three points, and testing whether it is within some tolerance
		/// to zero.
		/// </remarks>
		public static bool AreCollinear(Vector a, Vector b, Vector c)
		{
			var triangleArea = Math.Abs(0.5 * (a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)));
			return triangleArea < Tolerance;
		}
	}
}