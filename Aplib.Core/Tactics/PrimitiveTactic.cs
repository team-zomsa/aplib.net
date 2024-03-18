using System;
using System.Collections.Generic;

namespace Aplib.Core.Tactics
{
    /// <summary>
    /// Represents a primitive tactic in the Aplib.Core namespace.
    /// </summary>
    public class PrimitiveTactic : Tactic
    {
        /// <summary>
        /// Gets the action of the primitive tactic.
        /// </summary>
        protected readonly Action Action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic"/> class with the specified action.
        /// </summary>
        /// <param name="action">The action of the primitive tactic.</param>
        public PrimitiveTactic(Action action) => Action = action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic"/> class with the specified action and guard.
        /// </summary>
        /// <param name="action">The action of the primitive tactic.</param>
        /// <param name="guard">The guard of the tactic.</param>
        public PrimitiveTactic(Action action, Func<bool> guard) : base(guard) => Action = action;

        /// <inheritdoc/>
        public override Action? GetAction() => IsActionable() ? Action : null;

        /// <inheritdoc/>
        public override bool IsActionable() => base.IsActionable() && Action.IsActionable();
    }
}
