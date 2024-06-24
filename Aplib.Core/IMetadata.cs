namespace Aplib.Core
{
    /// <summary>
    /// A collection of generic metadata for unique instances which should help
    /// visualize the instance with human-readable information.
    /// This metadata may be useful for debugging or logging.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// Gets the unique identifier of the instance.
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// Gets the name used to display the instance.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the description used to describe the instance.
        /// </summary>
        public string? Description { get; }
    }
}
