﻿using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes any of the provided sub-tactics.
    /// </summary>
    public class AnyOfTactic<TBeliefSet> : Tactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the sub-tactics of the tactic.
        /// </summary>
        protected readonly LinkedList<ITactic<TBeliefSet>> _subTactics;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyOfTactic{TBeliefSet}"/> class with the specified sub-tactics
        /// and an optional guard condition.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="guard">The guard condition.</param>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public AnyOfTactic
        (
            Metadata metadata,
            Func<TBeliefSet, bool> guard,
            params ITactic<TBeliefSet>[] subTactics
        )
            : base(metadata)
        {
            _guard = guard;
            _subTactics = new LinkedList<ITactic<TBeliefSet>>(subTactics);
        }

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(Metadata,Func{TBeliefSet,bool},ITactic{TBeliefSet}[])"/>
        public AnyOfTactic
            (Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            : this(new Metadata(), guard, subTactics)
        {
        }

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(Metadata,Func{TBeliefSet,bool},ITactic{TBeliefSet}[])" />
        public AnyOfTactic(Metadata metadata, params ITactic<TBeliefSet>[] subTactics)
            : this(metadata, _ => true, subTactics)
        {
        }

        /// <inheritdoc cref="AnyOfTactic{TBeliefSet}(Metadata,Func{TBeliefSet,bool},ITactic{TBeliefSet}[])" />
        public AnyOfTactic(params ITactic<TBeliefSet>[] subTactics)
            : this(new Metadata(), subTactics)
        {
        }

        /// <inheritdoc/>
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
        {
            List<IAction<TBeliefSet>> actions = new();

            foreach (ITactic<TBeliefSet> subTactic in _subTactics)
            {
                IAction<TBeliefSet>? action = subTactic.GetAction(beliefSet);

                if (action is not null)
                    actions.Add(action);
            }

            if (actions.Count == 0)
                return null;

            return actions[ThreadSafeRandom.Next(actions.Count)];
        }
    }
}
