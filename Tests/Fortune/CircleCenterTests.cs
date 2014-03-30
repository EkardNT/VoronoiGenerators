using NUnit.Framework;
using VoronoiGenerators;
using VoronoiGenerators.Fortune;

namespace Tests.Fortune
{
	[TestFixture]
	public class CircleCenterTests
	{
		[Test]
		public void ReturnsCorrectCenter()
		{
			// First three vectors are inputs, last is expected output.
			var tests = new[,] 
			{
				{new Vector(0, 0), new Vector(0, 10), new Vector(10, 0), new Vector(5, 5)},
				{new Vector(-1,0), new Vector(0, 1), new Vector(1, 0), new Vector(0, 0)},
				{new Vector(0, 10), new Vector(10, 0), new Vector(10, 30), new Vector(15, 15)}
			};

			for (int i = 0; i < tests.GetLength(0); i++)
			{
				var output = Helpers.CircleCenter(tests[i, 0], tests[i, 1], tests[i, 2]);
				Assert.That(output == tests[i, 3], Is.True, string.Format("For inputs {0}, {1}, {2} expected output {3} but output was {4}.",
					tests[i, 0],
					tests[i, 1],
					tests[i, 2],
					tests[i, 3],
					output));
			}
		}

		[Test]
		public void FailsForCollinearPoints()
		{
			var tests = new[,]
			{
				{new Vector(), new Vector(), new Vector()},
				{new Vector(-1000, -1000), new Vector(), new Vector(1000, 1000)},
				{new Vector(123, 0), new Vector(234, 0), new Vector(345, 0)},
				{new Vector(0, 123), new Vector(0, 234), new Vector(0, 345)}
			};

			for (int i = 0; i < tests.GetLength(0); i++)
			{
				Assert.Throws(typeof(CollinearPointsException), () => { Helpers.CircleCenter(tests[i, 0], tests[i, 1], tests[i, 2]); });
			}
		}
	}
}