using NUnit.Framework;
using System;
using VoronoiGenerators;
using VoronoiGenerators.Fortune;

namespace Tests.Fortune
{
	[TestFixture]
	public class AreCollinearTests
	{
		public const int Seed = 8101991;

		[Test]
		public void CorrectlyIdentifiesCollinearScenarios()
		{
			var rand = new Random(Seed);

			foreach (int xDelta in new[] { -1, 0, 1 })
			{
				foreach (int yDelta in new[] { -1, 0, 1 })
				{
					double
						startX = rand.NextDouble() * 200.0 - 100.0,
						startY = rand.NextDouble() * 200.0 - 100.0;
					double
						xStep = xDelta * rand.NextDouble() * 50.0,
						yStep = yDelta * rand.NextDouble() * 50.0;
					Vector
						a = new Vector(startX, startY),
						b = new Vector(startX + xStep, startY + yStep),
						c = new Vector(startX + 2.0 * xStep, startY + 2.0 * yStep);
					Assert.That(Helpers.AreCollinear(a, b, c), Is.True, string.Format(
						"Expected Helpers.AreCollinear() to report that the points {0}, {1}, {2} are collinear, but it reported that they are non-collinear.",
						a,
						b,
						c));
				}
			}
		}

		[Test]
		public void CorrectlyIdentifiesNonCollinearScenarios()
		{
			var rand = new Random(Seed);

			foreach (int xDelta in new[] { -1, 0, 1 })
			{
				foreach (int yDelta in new[] { -1, 0, 1 })
				{
					double
						startX = rand.NextDouble() * 200.0 - 100.0,
						startY = rand.NextDouble() * 200.0 - 100.0;
					double
						xStep = xDelta * rand.NextDouble() * 50.0 + 10.0,
						yStep = yDelta * rand.NextDouble() * 50.0 + 10.0;
					Vector
						a = new Vector(startX, startY),
						b = new Vector(startX + xStep, startY + yStep),
						c = new Vector(startX - 2.0 * xStep, startY + 2.0 * yStep);
					Assert.That(Helpers.AreCollinear(a, b, c), Is.False, string.Format(
						"Expected Helpers.AreCollinear() to report that the points {0}, {1}, {2} are non-collinear, but it reported that they are collinear.",
						a,
						b,
						c));
				}
			}
		}
	}
}