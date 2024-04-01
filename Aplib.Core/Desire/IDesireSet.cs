using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;

namespace Aplib.Core.Desire
{
    public interface IDesireSet<TBeliefSet> :  ICompletable
        where TBeliefSet : IBeliefSet
    {
        GoalStructure<TBeliefSet> MainGoal { get; }
        
        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        IGoal? GetCurrentGoal(TBeliefSet beliefSet);
    }
}
