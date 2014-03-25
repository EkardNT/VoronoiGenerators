namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Contains the state of a currently running instance of Fortune's algorithm.
	/// </summary>
	internal class State
	{
		/// <summary>
		/// Y coordinate of the horizontal sweepline.
		/// </summary>
		public double SweeplineY;

		/// <summary>
		/// Stores the known upcoming sweepline events. Specifically,
		/// all site events are known in advance, but circle events
		/// are added and removed as the algorithm progresses.
		/// </summary>
		public PriorityQueue<Event> EventQueue;

		/// <summary>
		/// The leftmost arc on the beach line.
		/// </summary>
		public BeachArc BeachLine;
	}
}