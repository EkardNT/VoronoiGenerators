namespace VoronoiGenerators
{
	/// <summary>
	/// A two-dimensional vector.
	/// </summary>
	public struct Vector
	{
		/// <summary>
		/// X coordinate.
		/// </summary>
		public double X;
		/// <summary>
		/// Y coordinate.
		/// </summary>
		public double Y;

		/// <summary>
		/// Initializes a new Vector.
		/// </summary>
		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
}