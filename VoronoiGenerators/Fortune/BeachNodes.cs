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

		/// <summary>
		/// Left child of this node in the beach line status tree data structure.
		/// </summary>
		public object LeftChild;

		/// <summary>
		/// Right child of this node in the beach line status tree data structure.
		/// </summary>
		public object RightChild;

		/// <summary>
		/// Parent of this node in the beach line status tree data structure.
		/// </summary>
		public BreakpointNode Parent;

		public BreakpointNode(Site leftSite, Site rightSite, HalfEdge halfEdge, bool isLeftBreakpoint)
		{
			LeftSite = leftSite;
			RightSite = rightSite;
			HalfEdge = halfEdge;
			IsLeftBreakpoint = isLeftBreakpoint;
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
		public PriorityQueueItem<CircleEvent> DisappearanceEvent;

		/// <summary>
		/// Parent of this node in the beach line status tree data structure.
		/// </summary>
		public BreakpointNode Parent;

		public ArcNode(Site definingSite)
		{
			DefiningSite = definingSite;
		}

		public override string ToString()
		{
			return string.Format(
				"[ArcNode DefiningSite={0} DisappearanceEvent={1}]",
				DefiningSite,
				DisappearanceEvent == null ? null : DisappearanceEvent.Item);
		}
	}
}