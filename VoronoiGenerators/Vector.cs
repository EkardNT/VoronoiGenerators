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
		public readonly double X;
		/// <summary>
		/// Y coordinate.
		/// </summary>
		public readonly double Y;

		/// <summary>
		/// Initializes a new Vector.
		/// </summary>
		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Returns a Vector perpendicular to this Vector,
		/// rotated to the right of this Vector according
		/// to the right hand rule.
		/// </summary>
		public Vector Perpendicular()
		{
			return new Vector(Y, -X);
		}

		public double Length()
		{
			return System.Math.Sqrt(X * X + Y * Y);
		}

		public Vector Normalize()
		{
			var length = Length();
			return new Vector(X / length, Y / length);
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

		public static Vector operator-(Vector a, Vector b)
		{
			return new Vector(a.X - b.X, a.Y - b.Y);
		}

		public static Vector operator+(Vector a, Vector b)
		{
			return new Vector(a.X + b.X, a.Y + b.Y);
		}

		public static double Dot(Vector a, Vector b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		public static Vector operator*(double factor, Vector v)
		{
			return new Vector(v.X * factor, v.Y * factor);
		}
	}
}