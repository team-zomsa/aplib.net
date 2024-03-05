using System;
using System.Collections.Generic;

namespace Aplib.Core.Tactics
{
    /// <summary>
    /// Represents a tactic in the Aplib.Core namespace.
    /// </summary>
    public abstract class Tactic
    {
        /// <summary>
        /// Gets or sets the sub-tactics of the tactic.
        /// </summary>
        protected LinkedList<Tactic> SubTactics { get; set; }

        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected Func<bool> Guard { get; set; } = () => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/> class with the specified tactic type and sub-tactics.
        /// </summary>
        /// <param name="tacticType">The type of the tactic.</param>
        /// <param name="subTactics">The sub-tactics of the tactic.</param>
        protected Tactic(List<Tactic> subTactics)
        {
            SubTactics = new();

            foreach (Tactic tactic in subTactics)
            {
                _ = SubTactics.AddLast(tactic);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic"/> class with the specified tactic type, sub-tactics, and guard.
        /// </summary>
        /// <param name="tacticType">The type of the tactic.</param>
        /// <param name="subTactics">The sub-tactics of the tactic.</param>
        /// <param name="guard">The guard of the tactic.</param>
        protected Tactic(List<Tactic> subTactics, Func<bool> guard)
            : this(subTactics) => Guard = guard;

        /// <summary>
        /// Gets the first enabled primitive actions.
        /// </summary>
        /// <returns>A list of primitive tactics that are enabled.</returns>
        public abstract List<PrimitiveTactic> GetFirstEnabledActions();

        /// <summary>
        /// Determines whether the tactic is actionable.
        /// </summary>
        /// <returns>True if the tactic is actionable, false otherwise.</returns>
        public virtual bool IsActionable() => Guard();
    }
}
