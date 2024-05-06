using Aplib.Core.Belief;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class Action<TBeliefSet> : IAction<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected readonly System.Action<TBeliefSet> _effect;

        /// <summary>
        /// Gets the metadata of the action.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>=
        /// <param name="effect">The effect of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public Action(System.Action<TBeliefSet> effect, Metadata? metadata = null)
        {
            _effect = effect;
            Metadata = metadata ?? new Metadata();
        }

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Action{TQuery}" /> class.
        /// </summary>
        /// <remarks>Only meant for internal use</remarks>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        protected Action(Metadata? metadata = null) : this(_ => { }, metadata) { }

        /// <inheritdoc/>
        public virtual void Execute(TBeliefSet beliefSet) => _effect(beliefSet);
    }
}
