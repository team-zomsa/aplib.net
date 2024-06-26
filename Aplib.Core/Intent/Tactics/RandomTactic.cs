﻿// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes a random tactic from the provided subtactics.
    /// </summary>
    public class RandomTactic<TBeliefSet> : Tactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the subtactics of the tactic.
        /// </summary>
        protected internal readonly LinkedList<ITactic<TBeliefSet>> _subtactics;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomTactic{TBeliefSet}"/> class with the specified subtactics
        /// and an optional guard condition.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="guard">The guard condition.</param>
        /// <param name="subtactics">The list of subtactics.</param>
        public RandomTactic
        (
            IMetadata metadata,
            System.Predicate<TBeliefSet> guard,
            params ITactic<TBeliefSet>[] subtactics
        )
            : base(metadata, guard) => _subtactics = new LinkedList<ITactic<TBeliefSet>>(subtactics);

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])"/>
        public RandomTactic
            (System.Predicate<TBeliefSet> guard, params ITactic<TBeliefSet>[] subtactics)
            : this(new Metadata(), guard, subtactics)
        {
        }

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public RandomTactic(IMetadata metadata, params ITactic<TBeliefSet>[] subtactics)
            : this(metadata, _ => true, subtactics)
        {
        }

        /// <inheritdoc cref="RandomTactic{TBeliefSet}(IMetadata,System.Predicate{TBeliefSet},ITactic{TBeliefSet}[])" />
        public RandomTactic(params ITactic<TBeliefSet>[] subtactics)
            : this(new Metadata(), _ => true, subtactics)
        {
        }

        /// <inheritdoc/>
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
        {
            if (!IsActionable(beliefSet)) return null;

            List<IAction<TBeliefSet>> actions = new();

            foreach (ITactic<TBeliefSet> subtactic in _subtactics)
            {
                IAction<TBeliefSet>? action = subtactic.GetAction(beliefSet);

                if (action is not null) actions.Add(action);
            }

            if (actions.Count == 0) return null;

            return actions[ThreadSafeRandom.Next(actions.Count)];
        }

        /// <inheritdoc/>
        public override IEnumerable<ILoggable> GetLogChildren() => _subtactics.OfType<ILoggable>();
    }
}
