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
        /// Data about the Tactic such as a name and description, this may be useful for debugging or logging.
        /// </summary>
        public Metadata Metadata { get; }
        
        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected Func<bool> Guard { get; set; } = () => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/>.
        /// </summary>
        /// <param name="name">The name of this Tactic, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Tactic, used to explain this goal in several contexts.
        /// </param>
        protected Tactic(string name, string? description = null) => Metadata = new Metadata(name, description);

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/> class with a specified guard.
        /// </summary>
        /// <param name="guard">The guard of the tactic.</param>
        /// <param name="name">The name of this Tactic, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Tactic, used to explain this goal in several contexts.
        /// </param>
        protected Tactic(Func<bool> guard, string name, string? description = null)
        {
            Guard = guard;
            Metadata = new Metadata(name, description);
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
