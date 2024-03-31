namespace Aplib.Core
{
    /// <summary>
    /// Represents the state of a completable object.
    /// </summary>
    public enum CompletionStatus
    {
        /// <summary>
        /// Represents the status of a completable object that is not yet completed.
        /// </summary>
        Unfinished,

        /// <summary>
        /// Represents the status of a completable object that has been successfully completed.
        /// </summary>
        Success,

        /// <summary>
        /// Represents the status of a completable object that has failed to complete.
        /// </summary>
        Failure
    }
}
