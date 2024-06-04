using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Tactics are the real meat of <see cref="Desire.Goals.Goal{TBeliefSet}"/>s, as they define how the agent can approach the goal in hopes
    /// of finding a solution which makes the Goal's heuristic function evaluate to being completed. A tactic represents
    /// a smart combination of <see cref="Action{TBeliefSet}"/>s, which are executed in a Belief Desire Intent Cycle.
    /// </summary>
    /// <seealso cref="Desire.Goals.Goal{TBeliefSet}"/>
    /// <seealso cref="Action{TBeliefSet}"/>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public abstract class Tactic<TBeliefSet> : ITactic<TBeliefSet>, IDocumented
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the guard of the tactic.
        /// </summary>
        protected System.Func<TBeliefSet, bool> _guard;

        /// <inheritdoc />
        public IMetadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tactic{TBeliefSet}"/> class with a specified guard.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="guard">The guard of the tactic.</param>
        protected Tactic(IMetadata metadata, System.Func<TBeliefSet, bool> guard)
        {
            _guard = guard;
            Metadata = metadata;
        }

        /// <inheritdoc cref="Tactic{TBeliefSet}(IMetadata,System.Func{TBeliefSet,bool})" />
        protected Tactic(System.Func<TBeliefSet, bool> guard) : this(new Metadata(), guard) { }

        /// <inheritdoc />
        protected Tactic(IMetadata metadata) : this(metadata, _ => true) { }

        /// <inheritdoc />
        protected Tactic() : this(new Metadata(), _ => true) { }

        /// <inheritdoc />
        public abstract IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet);

        /// <inheritdoc />
        public virtual bool IsActionable(TBeliefSet beliefSet) => _guard(beliefSet);

        /// <summary>
        /// Implicitly lifts an action into a tactic.
        /// </summary>
        /// <inheritdoc cref="LiftingExtensionMethods.Lift{TBeliefSet}(IAction{TBeliefSet},IMetadata)" path="/param[@name='action']"/>
        /// <returns>The most logically matching tactic, wrapping around <paramref name="action"/>.</returns>
        public static implicit operator Tactic<TBeliefSet>(Action<TBeliefSet> action) => action.Lift();
    }
}
