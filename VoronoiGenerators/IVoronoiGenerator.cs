using System.Collections.Generic;

namespace VoronoiGenerators
{
	/// <summary>
	/// Represents an algorithm which can compute Voronoi diagrams.
	/// </summary>
	public interface IVoronoiGenerator
	{
		/// <summary>
		/// Computes the Voronoi diagram defined by a series of input Sites.
		/// Implementations of this method must be thread safe.
		/// </summary>
		/// <param name="sites">The Sites which define the Voronoi diagram.</param>
		DCEL ComputeVoronoiDiagram(IEnumerable<Site> sites);
	}
}