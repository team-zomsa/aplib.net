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
        public abstract List<PrimitiveTactic> GetFirstEnabledActions();

        /// <summary>
        /// Determines whether the tactic is actionable.
        /// </summary>
        /// <returns>True if the tactic is actionable, false otherwise.</returns>
        public virtual bool IsActionable() => Guard();
    }
}