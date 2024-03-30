namespace Aplib.Core.Desire.Goals
{
    /// <summary>
    /// Represents the state of a goal.
    /// </summary>
    public enum GoalState
    {
        /// <summary>
        /// The goal has not yet been completed.
        /// </summary>
        Unfinished,

        /// <summary>
        /// The goal has been completed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// The goal has failed.
        /// </summary>
        Failure
    }
}
