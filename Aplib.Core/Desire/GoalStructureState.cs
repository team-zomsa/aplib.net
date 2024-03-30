namespace Aplib.Core.Desire
{
    /// <summary>
    /// Represents the state of a <see cref="GoalStructure{TBeliefSet}" />.
    /// </summary>
    public enum GoalStructureState
    {
        /// <summary>
        /// Represents a goal structure that is not yet completed.
        /// </summary>
        Unfinished,

        /// <summary>
        /// Represents a goal structure that has been successfully completed.
        /// </summary>
        Success,

        /// <summary>
        /// Represents a goal structure that has failed to complete.
        /// </summary>
        Failure
    }
}
