using System.Collections.Generic;
namespace VoronoiGenerators
{
	/// <summary>
	/// A doubly-connected edge list data structure.
	/// </summary>
	public class DCEL
	{
		/// <summary>
		/// All the Vertexes contained in this doubly-connected edge list.
		/// </summary>
		public IReadOnlyCollection<Vertex> Vertexes { get; internal set; }

		/// <summary>
		/// All the Faces contained in this doubly-connected edge list.
		/// </summary>
		public IReadOnlyCollection<Face> Faces { get; internal set; }

		/// <summary>
		/// All the HalfEdges contained in this doubly-connected edge list.
		/// </summary>
		public IReadOnlyCollection<HalfEdge> HalfEdges { get; internal set; }
	}

	/// <summary>
	/// An HalfEdge in a doubly-connected HalfEdge list.
	/// </summary>
	public class HalfEdge
	{
		/// <summary>
		/// The next HalfEdge following this HalfEdge in a
		/// linked list around the Interior Face.
		/// </summary>
		public HalfEdge Next { get; internal set; }

		/// <summary>
		/// The HalfEdge that is on the exterior side of this HalfEdge,
		/// whose Origin is this HalfEdge's destination.
		/// </summary>
		public HalfEdge Twin { get; internal set; }

		/// <summary>
		/// The Face that is on the interior side of this HalfEdge.
		/// </summary>
		public Face Interior { get; internal set; }

		/// <summary>
		/// The Vertex at which this HalfEdge begins.
		/// </summary>
		public Vertex Origin { get; internal set; }
	}

	/// <summary>
	/// A face in a doubly-connected HalfEdge list.
	/// </summary>
	public class Face
	{
		/// <summary>
		/// The user data object taken from the input Site
		/// that generated this Face.
		/// </summary>
		public object UserData { get; internal set; }

		/// <summary>
		/// One of the HalfEdges which borders and is oriented toward this Face.
		/// </summary>
		public HalfEdge BorderingEdge { get; internal set; }
	}

	/// <summary>
	/// A vertex in a doubly-connected HalfEdge list.
	/// </summary>
	public class Vertex
	{
		/// <summary>
		/// The Vertex's position in the plane.
		/// </summary>
		public Vector Position { get; internal set; }

		/// <summary>
		/// One of the possibly many HalfEdges that have
		/// this Vertex as its Origin.
		/// </summary>
		public HalfEdge Leaving { get; internal set; }
	}
}