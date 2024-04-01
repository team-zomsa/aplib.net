namespace Aplib.Core
{
    /// <summary>
    /// Defines an object that can be completed.
    /// </summary>
    public interface ICompletable
    {
        /// <summary>
        /// Gets the completion status of the object.
        /// </summary>
        public CompletionStatus Status { get; }
    }
}
