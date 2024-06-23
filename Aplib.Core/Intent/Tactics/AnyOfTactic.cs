using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes any of the provided subtactics.
    /// </summary>
    public class AnyOfTactic<TBeliefSet> : Tactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the subtactics of the tactic.
        /// </summary>
        protected internal readonly LinkedList<ITactic<TBeliefSet>> _subtactics;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOfTactic{TBeliefSet}"/> class with the specified subtactics
        /// and an optional guard condition.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="guard">The guard condition.</param>
        /// <param name="subtactics">The list of subtactics.</param>
        public AnyOfTactic
        (
            IMetadata metadata,
            System.Predicate<TBeliefSet> guard,
            params ITactic<TBeliefSet>[] subtactics
        )
            : base(metadata, guard) => _subtactics = new LinkedList<ITactic<TBeliefSet>>(subtactics);

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])"/>
        public AnyOfTactic
            (System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            : this(new Metadata(), guard, subtactics)
        {
        }

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public AnyOfTactic(IMetadata metadata, params ITactic<TBeliefSet>[] subtactics)
            : this(metadata, _ => true, subtactics)
        {
        }

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public AnyOfTactic(params ITactic<TBeliefSet>[] subtactics)
            : this(new Metadata(), _ => true, subtactics)
        {
        }

        /// <inheritdoc/>
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
        {
            if (!IsActionable(beliefSet)) return null;

            List<IAction<TBeliefSet>> actions = new();

            foreach (ITactic<TBeliefSet> subTactic in _subtactics)
            {
                IAction<TBeliefSet>? action = subTactic.GetAction(beliefSet);

                if (action is not null) actions.Add(action);
            }

            if (actions.Count == 0) return null;

            return actions[ThreadSafeRandom.Next(actions.Count)];
        }

        /// <inheritdoc/>
        public override IEnumerable<ILoggable> GetLogChildren() => _subtactics.OfType<ILoggable>();
    }
}
