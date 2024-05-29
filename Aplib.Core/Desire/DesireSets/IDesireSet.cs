using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire.DesireSets
{
    /// <summary>
    /// Represents a set of goals that the agent has.
    /// This is the main structure that the agent will use to determine what it should do next.
    /// </summary>
    /// <typeparam name="TBeliefSet"></typeparam>
    public interface IDesireSet<in TBeliefSet> : ICompletable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet);

        /// <summary>
        /// Updates the status of this <see cref="IDesireSet{TBeliefSet}"/>.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        void Update(TBeliefSet beliefSet);
    }
}
