namespace Aplib.Core
{
    /// <summary>
    /// Represents an object that contains an instance of <see cref="IMetadata"/>.
    /// </summary>
    public interface IDocumented
    {
        /// <summary>
        /// Gets the metadata of this BDI cycle component.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public IMetadata Metadata { get; }
    }
}
