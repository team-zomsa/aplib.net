using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that executes the first enabled action from a list of sub-tactics.
    /// </summary>
    public class FirstOfTactic<TBeliefSet> : AnyOfTactic<TBeliefSet> where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic{TBeliefSet}"/> class with the specified sub-tactics.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic(Metadata? metadata = null, params Tactic<TBeliefSet>[] subTactics)
            : base(metadata, subTactics) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfTactic{TBeliefSet}"/> class with the specified sub-tactics and guard condition.
        /// </summary>
        /// <param name="guard">The guard condition.</param>
        /// <param name="metadata">
        /// Metadata about this tactic, used to quickly display the tactic in several contexts.
        /// </param>
        /// <param name="subTactics">The list of sub-tactics.</param>
        public FirstOfTactic(System.Func<TBeliefSet, bool> guard, Metadata? metadata = null, params Tactic<TBeliefSet>[] subTactics)
            : base(guard, metadata, subTactics) { }

        /// <inheritdoc />
        public override IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet)
        {
            foreach (Tactic<TBeliefSet> subTactic in _subTactics)
            {
                IAction<TBeliefSet>? action = subTactic.GetAction(beliefSet);

                if (action is not null)
                    return action;
            }

            return null;
        }
    }
}
