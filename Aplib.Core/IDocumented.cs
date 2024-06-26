// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

namespace Aplib.Core
{
    /// <summary>
    /// Represents an object that contains general information on an instance, such as <see cref="IMetadata" />.
    /// </summary>
    public interface IDocumented
    {
        /// <summary>
        /// Gets the metadata of the instance. This metadata may be useful for debugging or logging.
        /// </summary>
        public IMetadata Metadata { get; }
    }
}
