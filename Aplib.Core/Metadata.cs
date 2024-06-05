using System;

namespace Aplib.Core
{
    /// <summary>
    /// Data structure to store information about a component which may be useful for debugging or logging.
    /// </summary>
    public class Metadata : IMetadata
    {
        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public string? Name { get; }

        /// <inheritdoc />
        public string? Description { get; }

        /// <summary>
        /// Store information about a BDI cycle component which may be useful for debugging or logging or general overviews.
        /// </summary>
        /// <param name="name">The name used to display the component.</param>
        /// <param name="description">The description used to describe the component.</param>
        public Metadata(string? name = null, string? description = null) : this(Guid.NewGuid(), name, description) { }

        /// <summary>
        /// Store information about a BDI cycle component which may be useful for debugging or logging or general overviews.
        /// </summary>
        /// <remarks>This constructor is mainly for testing.</remarks>
        /// <param name="id">A unique identifier for the component.</param>
        /// <param name="name">The name used to display the component.</param>
        /// <param name="description">The description used to describe the component.</param>
        internal Metadata(Guid id, string? name = null, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
