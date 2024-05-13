﻿using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Tactics are the real meat of <see cref="Desire.Goals.Goal{TBeliefSet}"/>s, as they define how the agent can approach the goal in hopes
    /// of finding a solution which makes the Goal's heuristic function evaluate to being completed. A tactic represents
    /// a smart combination of <see cref="Action{TBeliefSet}"/>s, which are executed in a Believe Desire Intent Cycle.
    /// </summary>
    /// <seealso cref="Desire.Goals.Goal{TBeliefSet}"/>
    /// <seealso cref="Action{TBeliefSet}"/>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public abstract class Tactic<TBeliefSet> : ITactic<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected System.Func<TBeliefSet, bool> _guard;

        /// <summary>
        /// Gets the metadata of the tactic.
        /// </summary>
        /// <remark>
        /// This metadata may be useful for debugging or logging.
        /// </remark>
        public Metadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic{TBeliefSet}"/> class with a specified guard.
        /// </summary>
        /// <param name="guard">The guard of the tactic.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        protected Tactic(System.Func<TBeliefSet, bool> guard, Metadata metadata)
        {
            _guard = guard;
            Metadata = metadata;
        }

        /// <inheritdoc cref="Tactic{TBeliefSet}(System.Func{TBeliefSet,bool},Aplib.Core.Metadata)" />
        protected Tactic(System.Func<TBeliefSet, bool> guard) : this(guard, new Metadata()) { }

        /// <inheritdoc cref="Tactic{TBeliefSet}(System.Func{TBeliefSet,bool},Aplib.Core.Metadata)" />
        protected Tactic(Metadata metadata) : this(_ => true, metadata) { }

        /// <inheritdoc cref="Tactic{TBeliefSet}(System.Func{TBeliefSet,bool},Aplib.Core.Metadata)" />
        protected Tactic() : this(_ => true, new Metadata()) { }

        /// <inheritdoc />
        public abstract IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet);

        /// <inheritdoc />
        public virtual bool IsActionable(TBeliefSet beliefSet) => _guard(beliefSet);
    }
}
