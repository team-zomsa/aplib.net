using Aplib.Core.Belief.BeliefSets;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class Action<TBeliefSet> : IAction<TBeliefSet>, IDocumented
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected readonly System.Action<TBeliefSet> _effect;

        /// <inheritdoc />
        public IMetadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>=
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        /// <param name="effect">The effect of the action.</param>
        public Action(IMetadata metadata, System.Action<TBeliefSet> effect)
        {
            Metadata = metadata;
            _effect = effect;
        }

        /// <inheritdoc cref="Action{TBeliefSet}(IMetadata,System.Action{TBeliefSet})"/>
        public Action(System.Action<TBeliefSet> effect) : this(new Metadata(), effect) { }

        /// <inheritdoc/>
        public virtual void Execute(TBeliefSet beliefSet) => _effect(beliefSet);
    }
}
