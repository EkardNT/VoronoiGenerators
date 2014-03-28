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
			ArcNode splitArcNode;
			BreakpointNode parentNode;
			FindArcSplitByNewSite(ev.Site.Position, state, out splitArcNode, out parentNode);

			// If the arc being split has a circle event, it is a false alarm,
			// so delete it from the event queue.
		}

		private void FindArcSplitByNewSite(Vector newSitePosition, State state, out ArcNode splitArcNode, out BreakpointNode parentNode)
		{
			parentNode = null;
			var node = state.BeachLine;
			BreakpointNode bpNode;
			while((bpNode = node as BreakpointNode) != null)
			{
				var breakpointX = Helpers.GetBreakpointX(
					state.SweepLineY, 
					bpNode.LeftSite.Position, 
					bpNode.RightSite.Position, 
					bpNode.IsLeftBreakpoint);
				parentNode = bpNode;
				node = newSitePosition.X <= breakpointX
					? bpNode.LeftChild
					: bpNode.RightChild;
			}
			splitArcNode = (ArcNode)node;
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