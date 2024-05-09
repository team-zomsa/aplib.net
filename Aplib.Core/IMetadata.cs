using System;

namespace Aplib.Core
{
    /// <summary>
    /// A collection of generic metadata for BDI cycle components.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// Gets the unique identifier of the BDI cycle component.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the name used to display the BDI cycle component during debugging, logging, or general overviews.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gets the description used to describe the BDI cycle component during debugging, logging, or general overviews.
        /// </summary>
        public string? Description { get; }
    }
}
