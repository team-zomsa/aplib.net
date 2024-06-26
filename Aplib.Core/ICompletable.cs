// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

namespace Aplib.Core
{
    /// <summary>
    /// Defines an object that can be completed.
    /// </summary>
    public interface ICompletable
    {
        /// <summary>
        /// Gets the completion status of the instance.
        /// </summary>
        public CompletionStatus Status { get; }
    }
}
