﻿using Aplib.Core.Belief;
using System;

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
        protected new Action<TBeliefSet, TQuery> _effect { get; set; }

        /// <summary>
        /// Gets or sets the query of the action.
        /// </summary>
        protected Func<TBeliefSet, TQuery?> _query { get; set; }

        /// <summary>
        /// Gets or sets the result of the query.
        /// </summary>
        protected TQuery? _storedQueryResult { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAction{TBeliefSet,TQuery}" /> class.
        /// </summary>
        /// <param name="effect">The effect of the action.</param>
        /// <param name="query">The query of the action.</param>
        /// <param name="metadata">
        /// Metadata about this action, used to quickly display the action in several contexts.
        /// </param>
        public QueryAction(Action<TBeliefSet, TQuery> effect,
            Func<TBeliefSet, TQuery?> query,
            Metadata? metadata = null)
            : base(metadata)
        {
            _effect = effect;
            _query = query;
        }

        /// <inheritdoc />
        public override void Execute(TBeliefSet beliefSet) => _effect(beliefSet, arg2: _storedQueryResult!);

        /// <summary>
        /// Queries the environment for the guarded item and returns whether the query is not null.
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
