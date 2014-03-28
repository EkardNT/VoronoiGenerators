using NUnit.Framework;
using VoronoiGenerators;
using VoronoiGenerators.Fortune;

namespace Tests.Fortune
{
	[TestFixture]
	public class TestBreakpointsConvergenceTests
	{
		[Test]
		public void CorrectlyIdentifiesConvergingScenarios()
		{
			var tests = new[,]
			{
				{new Vector(0, 0), new Vector(1, 1), new Vector(2, 0)},
				{new Vector(0, 0), new Vector(1, 1), new Vector(2, 1)},
				{new Vector(0, 0), new Vector(0.5, 10), new Vector(1, 0)},
				{new Vector(5, 9), new Vector(3, 1), new Vector(1, 0)},
				{new Vector(1, 0), new Vector(0, 1), new Vector(2, 1)},
				{new Vector(4, 7), new Vector(3, 2), new Vector(1, 1)}
			};

			for (int i = 0; i < tests.GetLength(0); i++)
				Assert.That(Helpers.TestBreakpointsConvergence(tests[i, 0], tests[i, 1], tests[i, 2]), Is.True, string.Format(
					"Expected Helpers.TestBreakpointsConvergence() to report that the arcs defined from left to right by the sites {0}, {1}, and {2} converged, but it reported that they diverged.",
					tests[i, 0],
					tests[i, 1],
					tests[i, 2]));
		}

		[Test]
		public void CorrectlyIdentifiesDivergingScenarios()
		{
			// Sweepline at 0 for all these tests.
			var tests = new[,]
			{
				{new Vector(-1, 0), new Vector(1, 0), new Vector(3, 0)},
				{new Vector(1, 0), new Vector(3, 1), new Vector(5, 9)},
				{new Vector(1, 1), new Vector(2, 2), new Vector(3, 3)},
				{new Vector(1, 1), new Vector(3, 2), new Vector(4, 7)}
			};

			for (int i = 0; i < tests.GetLength(0); i++)
				Assert.That(Helpers.TestBreakpointsConvergence(tests[i, 0], tests[i, 1], tests[i, 2]), Is.False, string.Format(
					"Expected Helpers.TestBreakpointsConvergence() to report that the arcs defined from left to right by the sites {0}, {1}, and {2} diverged, but it reported that they converged.",
					tests[i, 0],
					tests[i, 1],
					tests[i, 2]));
		}
	}
}