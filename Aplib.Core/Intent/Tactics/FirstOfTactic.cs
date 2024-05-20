﻿using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes the first enabled action from a list of sub-tactics.
    /// </summary>
    public class FirstOfTactic<TBeliefSet> : AnyOfTactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic{TBeliefSet}"/> class with the specified
        /// sub-tactics and guard condition.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="guard">The guard condition.</param>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic
            (Metadata metadata, System.Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            : base(metadata, guard, subTactics)
        {
        }

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(Metadata,System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])"/>
        public FirstOfTactic(Metadata metadata, params ITactic<TBeliefSet>[] subTactics)
            : this(metadata, _ => true, subTactics)
        {
        }

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(Metadata,System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])"/>
        public FirstOfTactic
            (System.Func<TBeliefSet, bool> guard, params ITactic<TBeliefSet>[] subTactics)
            : this(new Metadata(), guard, subTactics)
        {
        }

        /// <inheritdoc cref="FirstOfTactic{TBeliefSet}(Metadata,System.Func{TBeliefSet,bool},ITactic{TBeliefSet}[])"/>
        public FirstOfTactic(params ITactic<TBeliefSet>[] subTactics) : this(_ => true, subTactics) { }

        /// <inheritdoc />
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
        {
            foreach (ITactic<TBeliefSet> subTactic in _subTactics)
            {
                IAction<TBeliefSet>? action = subTactic.GetAction(beliefSet);

                if (action is not null) return action;
            }

            return null;
        }
    }
}
