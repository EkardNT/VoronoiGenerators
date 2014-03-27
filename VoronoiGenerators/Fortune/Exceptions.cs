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
}