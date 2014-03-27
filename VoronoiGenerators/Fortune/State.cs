using System.Collections.Generic;
namespace VoronoiGenerators.Fortune
{
	/// <summary>
	/// Contains the state of a currently running instance of Fortune's algorithm.
	/// </summary>
	internal class State
	{
		/// <summary>
		/// Y coordinate of the horizontal sweep line.
		/// </summary>
		public double SweepLineY;

		/// <summary>
		/// Stores the known upcoming sweep line events. Specifically,
		/// all site events are known in advance, but circle events
		/// are added and removed as the algorithm progresses.
		/// </summary>
		public PriorityQueue<Event> EventQueue;

		/// <summary>
		/// The leftmost arc on the beach line.
		/// </summary>
		public BeachArc BeachLine;

		/// <summary>
		/// The set of output faces.
		/// </summary>
		public List<Face> Faces;

		/// <summary>
		/// The set of output Voronoi vertexes.
		/// </summary>
		public List<Vertex> Vertexes;

		/// <summary>
		/// The set of output half edges.
		/// </summary>
		public List<HalfEdge> HalfEdges;
	}
}