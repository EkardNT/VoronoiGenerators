using System.Collections.Generic;

namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// An Voronoi generator that uses Steven Fortune's sweepline algorithm
	/// to compute Voronoi diagrams.
	/// 
	/// Useful links
	///		- Mr. Fortune's original paper: http://dl.acm.org/citation.cfm?id=10549
	///		- "Computational Geometry: Algorithms and Applications": http://www.cs.uu.nl/geobook/
	///		- "Computational Geometry in C": http://cs.smith.edu/~orourke/books/compgeom.html
	/// </summary>
	public class FortunesAlgorithm : IVoronoiGenerator
	{
		public DCEL ComputeVoronoiDiagram(IEnumerable<Site> sites)
		{
			var state = new State
			{
				BeachLine = null,
				EventQueue = CreateEventQueueWithSiteEvents(sites),
				SweepLineY = double.MaxValue,
				Faces = new Dictionary<Site, Face>(),
				HalfEdges = new List<HalfEdge>(),
				Vertexes = new List<Vertex>()
			};

			DrainEvents(state);

			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Processes all events from the event queue, including any
		/// discovered circle events.
		/// </summary>
		private void DrainEvents(State state)
		{
			while(state.EventQueue.Count > 0)
			{
				var currentEvent = state.EventQueue.Dequeue();

				// The sweep line should always decrease monotonically.
				if (currentEvent.YCoordinate > state.SweepLineY)
					throw new SanityCheckFailedException(string.Format(
						"Retrieved a {0} from the event queue whose y-coordinate ({1}) was greater than the sweep line's y-coordinate ({2}). The sweep line's y coordinate should monotonically decrease as the algorithm progresses.",
						currentEvent.GetType().Name,
						currentEvent.YCoordinate,
						state.SweepLineY));

				// Advance the sweep line to the y-coordinate of the current event.
				state.SweepLineY = currentEvent.YCoordinate;

				// Handle the event based on its type.
				if (currentEvent is SiteEvent)
					HandleSiteEvent((SiteEvent)currentEvent, state);
				else
					HandleCircleEvent((CircleEvent)currentEvent, state);
			}
		}

		private void HandleSiteEvent(SiteEvent ev, State state)
		{
			// If the beach line data structure is empty, initialize
			// it with the arc created by the new site into it and return.
			if (state.BeachLine == null)
			{
				state.BeachLine = new ArcNode(ev.Site);
				return;
			}

			// Find the arc split by the new site.
			var splitArcNode = Helpers.FindArcSplitByNewSite(ev.Site.Position, state.BeachLine);

			// If the arc being split has a circle event, it is a false alarm,
			// so delete it from the event queue.
			if (splitArcNode.DisappearanceEvent != null)
				state.EventQueue.Remove(splitArcNode.DisappearanceEvent.Index);

			// The following pieces of the final Voronoi diagram are created
			// during a site event:
			//	- The Face defined by the new site.
			//	- Both HalfEdges of the edge that will be traced out by the
			//	two new breakpoints.
			var newFace = new Face {UserData = ev.Site.UserData};
			var halfEdgeFacingOldSite = new HalfEdge();
			var halfEdgeFacingNewSite = new HalfEdge();
			state.Faces.Add(ev.Site, newFace);
			state.HalfEdges.Add(halfEdgeFacingOldSite);
			state.HalfEdges.Add(halfEdgeFacingNewSite);

			// Link all the diagram pieces together that can be linked at this time.
			halfEdgeFacingOldSite.Interior = state.Faces[splitArcNode.DefiningSite];
			halfEdgeFacingNewSite.Interior = newFace;
			halfEdgeFacingOldSite.Twin = halfEdgeFacingNewSite;
			halfEdgeFacingNewSite.Twin = halfEdgeFacingOldSite;

			// During a site event, an existing arc is split into two
			// new arcs with an additional arc in the middle. We reflect
			// this change in beach line status tree data structure by
			// replacing the split arc node with a new subtree with five
			// nodes total: two for the two new breakpoints, and three
			// for the three new arcs on the beach line.
			var leftBP = new BreakpointNode(ev.Site, splitArcNode.DefiningSite, halfEdgeFacingOldSite, true);
			var rightBP = new BreakpointNode(ev.Site, splitArcNode.DefiningSite, halfEdgeFacingNewSite, false);
			var leftArc = new ArcNode(splitArcNode.DefiningSite);
			var middleArc = new ArcNode(ev.Site);
			var rightArc = new ArcNode(splitArcNode.DefiningSite);

			// Set up new subtree.
			leftBP.LeftChild = leftArc;
			leftBP.RightChild = rightBP;
			rightBP.LeftChild = middleArc;
			rightBP.RightChild = rightArc;
			rightBP.Parent = leftBP;
			leftArc.Parent = leftBP;
			middleArc.Parent = rightBP;
			rightArc.Parent = rightBP;

			// Graft new subtree into existing beach line status tree data structure.
			// First check for the special case where the split arc node was the
			// root element of the tree (aka the only element).
			if (splitArcNode.Parent == null)
			{
				if (state.BeachLine != splitArcNode)
					throw new SanityCheckFailedException(
						"The splitArcNode's parent was null, but it was not the root of the beach line status tree data structure.");
				state.BeachLine = leftBP;
			}
			// Otherwise, graft into existing tree. Here is where 
			// we would perform rebalancing operations on the tree.
			else
			{
				if (splitArcNode.Parent.LeftChild == splitArcNode)
				{
					splitArcNode.Parent.LeftChild = leftBP;
					leftBP.Parent = splitArcNode.Parent;
				}
				else
				{
					splitArcNode.Parent.RightChild = leftBP;
					leftBP.Parent = splitArcNode.Parent;
				}
			}
		}

		private void HandleCircleEvent(CircleEvent ev, State state)
		{

		}

		private PriorityQueue<Event> CreateEventQueueWithSiteEvents(IEnumerable<Site> sites)
		{
			var eventQueue = new PriorityQueue<Event>((e1, e2) => e1.YCoordinate >= e2.YCoordinate ? 1 : -1);
			foreach (var site in sites)
				eventQueue.Enqueue(new SiteEvent(site));
			return eventQueue;
		}
	}
}