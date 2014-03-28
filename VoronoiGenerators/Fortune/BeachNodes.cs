namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// An internal node in the beach line status tree data structure,
	/// which represents a breakpoint tracing out a Voronoi edge.
	/// </summary>
	internal class BreakpointNode
	{
		/// <summary>
		/// The site defining the parabola left of the breakpoint.
		/// </summary>
		public readonly Site LeftSite;
		/// <summary>
		/// The site defining the parabola right of the breakpoint.
		/// </summary>
		public readonly Site RightSite;
		/// <summary>
		/// One of the half edges of the edge being traced out by
		/// the two breakpoints defined by the left and right sites.
		/// </summary>
		public readonly HalfEdge HalfEdge;

		/// <summary>
		/// Indicates whether this breakpoint is the left or right
		/// breakpoint of the two defined by the left and right sites.
		/// </summary>
		public readonly bool IsLeftBreakpoint;

		public object LeftChild, RightChild;

		public BreakpointNode(Site leftSite, Site rightSite, HalfEdge halfEdge, bool isLeftBreakpoint)
		{
			LeftSite = leftSite;
			RightSite = rightSite;
			HalfEdge = halfEdge;
		}
	}

	/// <summary>
	/// A leaf node in the beach line status tree data structure, which
	/// represents an arc on the beach line.
	/// </summary>
	internal class ArcNode
	{
		/// <summary>
		/// The site that defines this parabolic arc on the beach line.
		/// </summary>
		public readonly Site DefiningSite;

		/// <summary>
		/// The circle event that will (barring false alarms) cause this 
		/// arc on the beach line to disappear. Is null if no such event
		/// has yet been detected.
		/// </summary>
		public CircleEvent DisappearingEvent;

		public ArcNode(Site definingSite)
		{
			DefiningSite = definingSite;
		}
	}
}