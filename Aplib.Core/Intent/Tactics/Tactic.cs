using System;
using System.Collections.Generic;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Tactics are the real meat of <see cref="Goal"/>s, as they define how the agent can approach the goal in hopes
    /// of finding a solution which makes the Goal's heuristic function evaluate to being completed. A tactic represents
    /// a smart combination of <see cref="Intent.Action.Action"/>s, which are executed in a Believe Desire Intent Cycle.
    /// </summary>
    /// <seealso cref="Goal"/>
    /// <seealso cref="Intent.Action.Action"/>
    public abstract class Tactic
    {
        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected Func<bool> Guard { get; set; } = () => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/>.
        /// </summary>
        protected Tactic()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/> class with a specified guard.
        /// </summary>
        /// <param name="guard">The guard of the tactic.</param>
        protected Tactic(Func<bool> guard) => Guard = guard;

        /// <summary>
        /// Gets the first enabled primitive actions.
        /// </summary>
        /// <returns>A list of primitive tactics that are enabled.</returns>
        public abstract List<PrimitiveTactic> GetFirstEnabledTactics();

        /// <summary>
        /// Determines whether the tactic is actionable.
        /// </summary>
        /// <returns>True if the tactic is actionable, false otherwise.</returns>
        public virtual bool IsActionable() => Guard();
    }
}
