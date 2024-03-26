using System;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a primitive tactic
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
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        public PrimitiveTactic(Action action, Metadata metadata)
            : base(metadata) => Action = action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic"/> class with the specified action and guard.
        /// </summary>
        /// <param name="action">The action of the primitive tactic.</param>
        /// <param name="guard">The guard of the primitive tactic.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        public PrimitiveTactic(Action action, Func<bool> guard, Metadata metadata)
            : base(guard, metadata) => Action = action;

        /// <inheritdoc/>
        public override Action? GetAction() => IsActionable() ? Action : null;

        /// <inheritdoc/>
        public override bool IsActionable() => base.IsActionable() && Action.IsActionable();
    }
}
