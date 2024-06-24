namespace Aplib.Core
{
    /// <summary>
    /// Represents the state of a completable object.
    /// </summary>
    public enum CompletionStatus
    {
        /// <summary>
        /// The status of a completable object that is not yet completed.
        /// </summary>
        Unfinished,

        /// <summary>
        /// The status of a completable object that has been successfully completed.
        /// </summary>
        Success,

        /// <summary>
        /// The status of a completable object that has failed to complete.
        /// </summary>
        Failure,
    }
}
