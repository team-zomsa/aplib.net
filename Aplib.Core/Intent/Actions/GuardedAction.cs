using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded with a query that stores the result of the guard.
    /// The result can be used in the effect.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    /// <typeparam name="TQuery">The type of the query of the action</typeparam>
    public class GuardedAction<TBeliefSet, TQuery> : Action<TBeliefSet>, IQueryable<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the result of the guard.
        /// </summary>
        protected TQuery? _storedGuardResult { get; set; }

        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected new System.Action<TBeliefSet, TQuery> _effect { get; set; }

        /// <summary>
        /// Gets or sets the guard of the action.
        /// </summary>
        protected System.Func<TBeliefSet, TQuery?> _query { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardedAction{TBeliefSet,TQuery}"/> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="query">The guard of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public GuardedAction(System.Action<TBeliefSet, TQuery> effect,
            System.Func<TBeliefSet, TQuery?> query,
            Metadata? metadata = null)
            : base(metadata)
        {
            _effect = effect;
            _query = query;
        }

        /// <inheritdoc/>
        public override void Execute(TBeliefSet beliefSet) => _effect(beliefSet, _storedGuardResult!);

        /// <summary>
        /// Queries the environment for the guarded item and returns whether the guard is not null.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>True if the guard is not null; otherwise, false.</returns>
        public bool Query(TBeliefSet beliefSet)
        {
            // Query the environment for the guarded item
            _storedGuardResult = _query(beliefSet);

            // Only return true if the guard is not null
            return _storedGuardResult is not null;
        }
    }
}
