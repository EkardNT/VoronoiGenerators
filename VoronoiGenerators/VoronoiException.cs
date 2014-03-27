using System;

namespace VoronoiGenerators
{
	/// <summary>
	/// Base class for custom exceptions thrown by the 
	/// VoronoiGenerators library.
	/// </summary>
	public class VoronoiException : Exception
	{
		public VoronoiException(string message) : base(message)
		{

		}

		public VoronoiException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}