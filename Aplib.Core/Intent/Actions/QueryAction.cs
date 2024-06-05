using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core.Intent.Actions
{
    /// <summary>
    /// Describes an action that can be executed and guarded with a query that stores a result.
    /// The result can be used in the effect.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    /// <typeparam name="TQuery">The type of the query of the action</typeparam>
    public class QueryAction<TBeliefSet, TQuery> : Action<TBeliefSet>, IQueryable<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the effect of the action.
        /// </summary>
        protected readonly new System.Action<TBeliefSet, TQuery> _effect;

        /// <summary>
        /// Gets or sets the query of the action.
        /// </summary>
        protected readonly System.Func<TBeliefSet, TQuery?> _query;

        /// <summary>
        /// Gets or sets the result of the query.
        /// </summary>
        protected TQuery? _storedQueryResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAction{TBeliefSet,TQuery}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="query">The query of the action.</param>
        public QueryAction
            (IMetadata metadata, System.Action<TBeliefSet, TQuery> effect, System.Func<TBeliefSet, TQuery?> query)
            : base(metadata, _ => { })
        {
            _effect = effect;
            _query = query;
        }

        /// <inheritdoc
        ///     cref="QueryAction{TBeliefSet,TQuery}(Aplib.Core.IMetadata,System.Action{TBeliefSet,TQuery},System.Func{TBeliefSet,TQuery})" />
        public QueryAction(System.Action<TBeliefSet, TQuery> effect, System.Func<TBeliefSet, TQuery?> query)
            : this(new Metadata(), effect, query)
        {
        }

        /// <inheritdoc />
        public override void Execute(TBeliefSet beliefSet) => _effect(beliefSet, _storedQueryResult!);

        /// <summary>
        /// Queries the environment for the queried item and returns whether the query is not null.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>True if the query is not null; otherwise, false.</returns>
        public bool Query(TBeliefSet beliefSet)
        {
            // Query the environment for the guarded item.
            _storedQueryResult = _query(beliefSet);

            // Only return true if the query is not null.
            return _storedQueryResult is not null;
        }
    }
}
