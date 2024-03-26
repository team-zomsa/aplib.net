namespace Aplib.Core
{
    /// <summary>
    /// Data structure to store information about a component which may be useful for debugging or logging.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets the name used to display the component during debugging, logging, or general overviews.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description used to describe the component during debugging, logging, or general overviews.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Store information about a component which may be useful for debugging or logging or general overviews.
        /// </summary>
        /// <param name="name">The name used to display the component.</param>
        /// <param name="description">The description used to describe the component.</param>
        public Metadata(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
