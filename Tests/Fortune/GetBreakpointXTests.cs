using System;
using System.Collections.Generic;
using NUnit.Framework;
using VoronoiGenerators;
using VoronoiGenerators.Fortune;

namespace Tests.Fortune
{
	[TestFixture]
	public class GetBreakpointXTests
	{
		[Test]
		public void ThrowsIfSweeplineNotBelowBothSites()
		{
			// Throws when above both points.
			Assert.Throws<ArgumentException>(() => Helpers.GetBreakpointX(5, new Vector(-1, 0), new Vector(1, 0), true));
			// Throws when above only one point.
			Assert.Throws<ArgumentException>(() => Helpers.GetBreakpointX(5, new Vector(-1, 10), new Vector(1, 0), true));
			Assert.Throws<ArgumentException>(() => Helpers.GetBreakpointX(5, new Vector(-1, 0), new Vector(1, 10), true));
		}
	}
}