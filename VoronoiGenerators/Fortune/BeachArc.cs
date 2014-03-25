namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Represents one arc on the beach line.
	/// </summary>
	internal class BeachArc
	{
		/// <summary>
		/// The Site that caused this arc on the beach line to appear.
		/// </summary>
		public Site Site;

		/// <summary>
		/// The arc to the right of this arc on the beach line.
		/// </summary>
		public BeachArc Right;

		/// <summary>
		/// The arc to the left of this arc on the beach line.
		/// </summary>
		public BeachArc Left;
	}
}