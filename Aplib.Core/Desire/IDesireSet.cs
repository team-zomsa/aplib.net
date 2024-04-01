using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Represents a set of goals that the agent has.
    /// This is the main structure that the agent will use to determine what it should do next.
    /// </summary>
    /// <typeparam name="TBeliefSet"></typeparam>
    public interface IDesireSet<TBeliefSet> :  ICompletable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Stores the main goal structure of the agent.
        /// </summary>
        GoalStructure<TBeliefSet> MainGoal { get; }
        
        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        IGoal? GetCurrentGoal(TBeliefSet beliefSet);
    }
}
