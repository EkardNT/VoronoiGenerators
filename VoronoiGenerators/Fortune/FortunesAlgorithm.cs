using System.Collections.Generic;

namespace VoronoiGenerators
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
			throw new System.NotImplementedException();
		}
	}
}