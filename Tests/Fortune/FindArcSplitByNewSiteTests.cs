using System;
using System.Collections.Generic;
using NUnit.Framework;
using VoronoiGenerators;
using VoronoiGenerators.Fortune;

namespace Tests.Fortune
{
	[TestFixture]
	public class FindArcSplitByNewSiteTests
	{
		private class TestCase
		{
			public object RootNode;
			public object ExpectedResult;
			public Vector NewSitePosition;
		}

		[Test]
		public void ThrowsForEmptyTree()
		{
			Assert.Throws<ArgumentNullException>(() => Helpers.FindArcSplitByNewSite(new Vector(0, 0), null));
		}

		[Test]
		public void FindsCorrectArcNode()
		{
			// First object is root node, second object is expected result arc node.
			var tests = new List<TestCase>();

			// A single arc.
			{
				var singleArc = new ArcNode(new Site(new Vector(0, 1)));
				tests.Add(new TestCase
				{
					RootNode = singleArc,
					ExpectedResult = singleArc,
					NewSitePosition = new Vector(0, 0)
				});
			}
			// Simple tree with three arcs.
			{
				/*
				var leftArc = new ArcNode(new Site(new Vector(0, 10)));
				var middleArc = new ArcNode(new Site(new Vector(5, 2)));
				var rightArc = new ArcNode(leftArc.DefiningSite);
				var leftBP = new BreakpointNode(leftArc.DefiningSite, middleArc.DefiningSite, null, true);
				var rightBP = new BreakpointNode(leftArc.DefiningSite, middleArc.DefiningSite, null, false);
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					leftArc, new Vector(-5, 0)));
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					middleArc, new Vector(5, 0)));
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					rightArc, new Vector(500, 0)));
				 */
			}
			// Three arc tree with sites vertically aligned.
			{
				var leftArc = new ArcNode(new Site(new Vector(0, 10)));
				var middleArc = new ArcNode(new Site(new Vector(0, 5)));
				var rightArc = new ArcNode(leftArc.DefiningSite);
				var leftBP = new BreakpointNode(leftArc.DefiningSite, middleArc.DefiningSite, null, true);
				var rightBP = new BreakpointNode(leftArc.DefiningSite, middleArc.DefiningSite, null, false);
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					leftArc, new Vector(-250, 0)));
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					middleArc, new Vector(-1, 0)));
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					middleArc, new Vector(0, 0)));
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					middleArc, new Vector(1, 0)));
				tests.Add(SetupTripleArcTestCase(
					leftBP, rightBP, leftArc, middleArc, rightArc,
					rightArc, new Vector(250, 0)));
			}

			foreach (var test in tests)
				Assert.That(Helpers.FindArcSplitByNewSite(test.NewSitePosition, test.RootNode), Is.EqualTo(test.ExpectedResult));
		}

		private TestCase SetupTripleArcTestCase(BreakpointNode leftBP, BreakpointNode rightBP, ArcNode leftArc,
			ArcNode middleArc, ArcNode rightArc, ArcNode expectedResult, Vector newSitePos)
		{
			leftArc.Parent = leftBP;
			middleArc.Parent = rightBP;
			rightArc.Parent = rightBP;
			leftBP.LeftChild = leftArc;
			leftBP.RightChild = rightBP;
			rightBP.Parent = leftBP;
			rightBP.LeftChild = middleArc;
			rightBP.RightChild = rightArc;
			return new TestCase
			{
				RootNode = leftBP,
				ExpectedResult = expectedResult,
				NewSitePosition = newSitePos
			};
		}
	}
}