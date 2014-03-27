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

		public override string ToString()
		{
			return string.Format("({0},{1})", X, Y);
		}

		public override bool Equals(object obj)
		{
			return obj is Vector && (Vector)obj == this;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public static bool operator==(Vector a, Vector b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator!=(Vector a, Vector b)
		{
			return a.X != b.X || a.Y != b.Y;
		}
	}
}