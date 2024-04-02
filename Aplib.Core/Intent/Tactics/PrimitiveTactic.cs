using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a primitive tactic
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class PrimitiveTactic<TBeliefSet> : Tactic<TBeliefSet> where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the action of the primitive tactic.
        /// </summary>
        protected readonly IAction<TBeliefSet> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic{TBeliefSet}"/> class with the specified action.
        /// </summary>
        /// <param name="action">The action of the primitive tactic.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        public PrimitiveTactic(IAction<TBeliefSet> action, Metadata? metadata = null)
            : base(metadata) => _action = action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic{TBeliefSet}"/> class with the specified action and guard.
        /// </summary>
        /// <param name="action">The action of the primitive tactic.</param>
        /// <param name="guard">The guard of the primitive tactic.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        public PrimitiveTactic(IAction<TBeliefSet> action, System.Func<TBeliefSet, bool> guard, Metadata? metadata = null)
            : base(guard, metadata) => _action = action;

        /// <inheritdoc/>
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet) => IsActionable(beliefSet) ? _action : null;

        /// <inheritdoc/>
        public override bool IsActionable(TBeliefSet beliefSet) => base.IsActionable(beliefSet) && _action.IsActionable(beliefSet);
    }
}
