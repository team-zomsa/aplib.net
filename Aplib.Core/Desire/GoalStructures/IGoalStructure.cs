using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Represents a goal structure.
    /// </summary>
    /// <remarks>
    /// A goal structure is a structure of predicates that must be fulfilled in order to complete a test.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public interface IGoalStructure<in TBeliefSet> : ICompletable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet);

        /// <summary>
        /// Updates the state of the goal structure.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        void UpdateStatus(TBeliefSet beliefSet);
    }
}
