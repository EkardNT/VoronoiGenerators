namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Represents a sweep line event.
	/// </summary>
	internal abstract class Event
	{
		/// <summary>
		/// The y coordinate at which this sweep line event occurs.
		/// </summary>
		public double YCoordinate { get; private set; }

		public Event(double yCoordinate)
		{
			YCoordinate = yCoordinate;
		}
	}

	/// <summary>
	/// Site events occur when the sweep line meets an input
	/// Site. They results in the emergence of a new arc
	/// on the beach line and the creation of two new half edges
	/// representing the edge that will be traced out by the
	/// two new breakpoints.
	/// </summary>
	internal class SiteEvent : Event
	{
		/// <summary>
		/// The Site that will cross the sweep line and cause
		/// this SiteEvent.
		/// </summary>
		public Site Site { get; private set; }

		public SiteEvent(Site site) : base(site.Position.Y)
		{
			Site = site;
		}
	}

	/// <summary>
	/// Circle events occur when an arc disappears from
	/// the beach line and two breakpoints meet. They
	/// results in the termination of the two breakpoints
	/// and the creation of a new Voronoi vertex.
	/// </summary>
	internal class CircleEvent : Event
	{
		/// <summary>
		/// The arc which will disappear from the beach line
		/// as a result of this CircleEvent.
		/// </summary>
		public BeachArc DisappearingArc { get; private set; }

		public CircleEvent(BeachArc disappearingArc, double lowestCircleY) : base(lowestCircleY)
		{
			DisappearingArc = disappearingArc;
		}
	}
}