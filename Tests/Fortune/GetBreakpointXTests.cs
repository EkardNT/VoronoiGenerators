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
		private class ReturnsCorrectValueTestCase
		{
			public Vector Site1, Site2;
			public double SweepLineY, LeftExpectedValue, RightExpectedValue;
		}

		[Test]
		public void ThrowsIfSweeplineNotBelowBothSites()
		{
			// Throws when above both points.
			Assert.Throws<ArgumentException>(() => Helpers.GetBreakpointX(5, new Vector(-1, 0), new Vector(1, 0), true));
			// Throws when above only one point.
			Assert.Throws<ArgumentException>(() => Helpers.GetBreakpointX(5, new Vector(-1, 10), new Vector(1, 0), true));
			Assert.Throws<ArgumentException>(() => Helpers.GetBreakpointX(5, new Vector(-1, 0), new Vector(1, 10), true));
		}

		[Test]
		public void ReturnsCorrectValue()
		{
			var testCases = new[]
			{
				new ReturnsCorrectValueTestCase
				{
					Site1 = new Vector(0, 10),
					Site2 = new Vector(0, 5),
					SweepLineY = 0,
					LeftExpectedValue = -7.0710678118654755,
					RightExpectedValue = 7.0710678118654755
				}
			};

			foreach (var test in testCases)
			{
				Assert.That(Helpers.GetBreakpointX(test.SweepLineY, test.Site1, test.Site2, true), Is.EqualTo(test.LeftExpectedValue));
				Assert.That(Helpers.GetBreakpointX(test.SweepLineY, test.Site1, test.Site2, false), Is.EqualTo(test.RightExpectedValue));
			}

		}
	}
}