using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;

namespace Aplib.Core.Intent.Tactics
{
    /// <summary>
    /// Represents a tactic that an agent can use to achieve its goals.
    /// A tactic is a strategy for achieving a particular goal.
    /// </summary>
    /// <typeparam name="TBeliefSet">The type of the belief set that the tactic uses.</typeparam>
    public interface ITactic<in TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the first enabled action of the tactic.
        /// </summary>
        /// <remark>
        /// An action is a specific operation that an agent can perform.
        /// The action returned by this method is the first action that the tactic can perform,
        /// given the current state of the world as represented by the belief set.
        /// </remark>
        /// <returns>
        /// A concrete <see cref="IAction{TBeliefSet}"/> that the tactic can perform, or null if no actions are enabled.
        /// </returns>
        public IAction<TBeliefSet>? GetAction(TBeliefSet beliefSet);

        /// <summary>
        /// Determines whether the tactic is actionable.
        /// </summary>
        /// <returns>True if the tactic is actionable, false otherwise.</returns>
        public bool IsActionable(TBeliefSet beliefSet);
    }
}
