using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Represents a goal structure.
    /// </summary>
    /// <remarks>
    /// A goal structure is structure of predicates that must be fulfilled in order to complete a test.
    /// </remarks>
    /// <typeparam name="TBeliefSet"></typeparam>
    public interface IGoalStructure<TBeliefSet> : IInterruptable<TBeliefSet>, ICompletable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        IGoal GetCurrentGoal(TBeliefSet beliefSet);

        /// <summary>
        /// Updates the state of the goal structure.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        void UpdateState(TBeliefSet beliefSet);
    }
}
