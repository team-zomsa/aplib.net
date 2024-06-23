using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes the first enabled action from a list of subtactics.
    /// </summary>
    public class FirstOfTactic<TBeliefSet> : Tactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the subtactics of the tactic.
        /// </summary>
        protected readonly LinkedList<ITactic<TBeliefSet>> _subtactics;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic{TBeliefSet}"/> class with the specified
        /// subtactics and guard condition.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="guard">The guard condition.</param>
        /// <param name="subtactics">The list of subtactics.</param>
        public FirstOfTactic
        (
            IMetadata metadata,
            System.Predicate<TBeliefSet> guard,
            params ITactic<TBeliefSet>[] subtactics
        )
            : base(metadata, guard) => _subtactics = new LinkedList<ITactic<TBeliefSet>>(subtactics);

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])"/>
        public FirstOfTactic(IMetadata metadata, params ITactic<TBeliefSet>[] subtactics)
            : this(metadata, _ => true, subtactics)
        {
        }

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])"/>
        public FirstOfTactic
            (System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            : this(new Metadata(), guard, subtactics)
        {
        }

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])"/>
        public FirstOfTactic(params ITactic<TBeliefSet>[] subtactics) : this(new Metadata(), _ => true, subtactics) { }

        /// <inheritdoc />
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
        {
            if (!IsActionable(beliefSet)) return null;

            return _subtactics
                .Select(subTactic => subTactic.GetAction(beliefSet))
                .OfType<IAction<TBeliefSet>>()
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public override IEnumerable<ILoggable> GetLogChildren() => _subtactics.OfType<ILoggable>();
    }
}
