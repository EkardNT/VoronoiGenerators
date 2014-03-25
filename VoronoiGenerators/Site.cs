namespace VoronoiGenerators
{
	/// <summary>
	/// An input site to Voronoi diagram generators.
	/// </summary>
	public class Site
	{
		/// <summary>
		/// The position of the Site in the plane.
		/// </summary>
		public Vector Position { get; private set; }

		/// <summary>
		/// If required by the user, holds custom data
		/// that will be copied to the Face that this
		/// Site generates.
		/// </summary>
		public object UserData { get; private set; }

		/// <summary>
		/// Initializes a new Site.
		/// </summary>
		public Site(Vector position, object userData = null)
		{
			Position = position;
			UserData = userData;
		}
	}
}