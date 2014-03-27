namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Occurs when a basic precept of Fortune's algorithm fails
	/// to pass a sanity check.
	/// </summary>
	public class SanityCheckFailedException : VoronoiException
	{
		internal SanityCheckFailedException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// Occurs when collinear points are given to an algorithm
	/// which requires non-collinear points as input.
	/// </summary>
	public class CollinearPointsException : VoronoiException
	{
		/// <summary>
		/// The first collinear point.
		/// </summary>
		public Vector P1 { get; private set; }
		/// <summary>
		/// The second collinear point.
		/// </summary>
		public Vector P2 { get; private set; }
		/// <summary>
		/// The third collinear point.
		/// </summary>
		public Vector P3 { get; private set; }

		internal CollinearPointsException(Vector p1, Vector p2, Vector p3)
			: base("Collinear points were given to an algorithm which requires non-collinear input.")
		{
		}
	}
}