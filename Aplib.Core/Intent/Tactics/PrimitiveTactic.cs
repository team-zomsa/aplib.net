using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Logging;
using System.Collections.Generic;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a primitive tactic
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class PrimitiveTactic<TBeliefSet> : Tactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the action of the primitive tactic.
        /// </summary>
        protected readonly IAction<TBeliefSet> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic{TBeliefSet}"/> class with the specified action
        /// and guard.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="action">The action of the primitive tactic.</param>
        /// <param name="guard">The guard of the primitive tactic.</param>
        public PrimitiveTactic(IMetadata metadata, IAction<TBeliefSet> action, System.Predicate<TBeliefSet> guard)
            : base(metadata, guard) => _action = action;

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IMetadata,IAction{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public PrimitiveTactic(IAction<TBeliefSet> action, System.Predicate<TBeliefSet> guard)
            : this(new Metadata(), action, guard)
        {
        }

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IMetadata,IAction{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public PrimitiveTactic(IMetadata metadata, IAction<TBeliefSet> action) : this(metadata, action, _ => true) { }

        /// <inheritdoc cref="PrimitiveTactic{TBeliefSet}(IMetadata,IAction{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public PrimitiveTactic(IAction<TBeliefSet> action) : this(new Metadata(), action, _ => true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveTactic{TBeliefSet}"/> class with the specified action
        /// and guard.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="queryAction">The queryable action of the primitive tactic.</param>
        /// <param name="guard">The guard of the primitive tactic.</param>
        public PrimitiveTactic
            (IMetadata metadata, IQueryable<TBeliefSet> queryAction, System.Predicate<TBeliefSet> guard)
            : this
            (
                metadata,
                action: queryAction,
                beliefSet => guard(beliefSet) && queryAction.Query(beliefSet)
            )
        {
        }

        /// <inheritdoc
        ///     cref="PrimitiveTactic{TBeliefSet}(IMetadata,IQueryable{TBeliefSet},System.Predicate{TBeliefSet})" />
        public PrimitiveTactic(IQueryable<TBeliefSet> queryAction, System.Predicate<TBeliefSet> guard)
            : this(new Metadata(), queryAction, guard)
        {
        }

        /// <inheritdoc
        ///     cref="PrimitiveTactic{TBeliefSet}(IMetadata,IQueryable{TBeliefSet},System.Predicate{TBeliefSet})" />
        public PrimitiveTactic(IMetadata metadata, IQueryable<TBeliefSet> queryAction)
            : this(metadata, queryAction, _ => true)
        {
        }

        /// <inheritdoc
        ///     cref="PrimitiveTactic{TBeliefSet}(IMetadata,IQueryable{TBeliefSet},System.Predicate{TBeliefSet})"/>
        public PrimitiveTactic(IQueryable<TBeliefSet> queryAction)
            : this(new Metadata(), queryAction, _ => true)
        {
        }

        /// <inheritdoc />
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
            => IsActionable(beliefSet) ? _action : null;

        /// <inheritdoc />
        public override IEnumerable<ILoggable> GetLogChildren() =>
            _action is ILoggable loggable ? new[] { loggable } : System.Linq.Enumerable.Empty<ILoggable>();
    }
}
