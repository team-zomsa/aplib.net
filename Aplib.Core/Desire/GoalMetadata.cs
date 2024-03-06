namespace Aplib.Core.Desire
{
    /// <summary>
    ///     Data structure to store information about a goal which may be useful for debugging or logging.
    /// </summary>
    public class GoalMetadata
    {
        /// <summary>
        ///     The name used to display the current goal during debugging, logging, or general overviews.
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     The description used to describe the current goal during debugging, logging, or general overviews.
        /// </summary>
        public readonly string Description;

        /// <summary>
        ///     Store information about a goal which may be useful for debugging or logging.
        /// </summary>
        /// <param name="name">The name used to display the goal.</param>
        /// <param name="description">The description used to describe the goal.</param>
        public GoalMetadata(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}