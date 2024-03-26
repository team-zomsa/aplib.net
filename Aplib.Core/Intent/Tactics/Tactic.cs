using System;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Tactics are the real meat of <see cref="Desire.Goals.Goal"/>s, as they define how the agent can approach the goal in hopes
    /// of finding a solution which makes the Goal's heuristic function evaluate to being completed. A tactic represents
    /// a smart combination of <see cref="Action"/>s, which are executed in a Believe Desire Intent Cycle.
    /// </summary>
    /// <seealso cref="Desire.Goals.Goal"/>
    /// <seealso cref="Intent.Actions.Action"/>
    public abstract class Tactic
    {
        /// <summary>
        /// Gets the metadata of the tactic.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }
        
        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected Func<bool> Guard { get; set; } = () => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/>.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        protected Tactic(Metadata metadata) => Metadata = metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/> class with a specified guard.
        /// </summary>
        /// <param name="guard">The guard of the tactic.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        protected Tactic(Func<bool> guard, Metadata metadata)
        {
            Guard = guard;
            Metadata = metadata;
        }

        /// <summary>
        /// Gets the enabled action.
        /// </summary>
        /// <returns>An action that is enabled.</returns>
        public abstract Action? GetAction();

        /// <summary>
        /// Determines whether the tactic is actionable.
        /// </summary>
        /// <returns>True if the tactic is actionable, false otherwise.</returns>
        public virtual bool IsActionable() => Guard();
    }
}
