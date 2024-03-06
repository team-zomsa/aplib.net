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
        /// <param name="name">The name of this Tactic, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Tactic, used to explain this goal in several contexts.
        /// </param>
        public PrimitiveTactic(Action action, string name, string? description = null) : base(name, description)
            => Action = action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic"/> class with the specified action and guard.
        /// </summary>
        /// <param name="action">The action of the primitive tactic.</param>
        /// <param name="guard">The guard of the primitive tactic.</param>
        /// <param name="name">The name of this Tactic, used to quickly display this goal in several contexts.</param>
        /// <param name="description">
        /// The description of this Tactic, used to explain this goal in several contexts.
        /// </param>
        public PrimitiveTactic(Action action, Func<bool> guard, string name, string? description = null)
            : base(guard, name, description) => Action = action;

        /// <inheritdoc/>
        public override Action? GetAction() => IsActionable() ? Action : null;

        /// <inheritdoc/>
        public override bool IsActionable() => base.IsActionable() && Action.IsActionable();
    }
}
