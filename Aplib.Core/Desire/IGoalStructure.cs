using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;

namespace Aplib.Core.Desire
{
    public interface IGoalStructure<TBeliefSet> : IInterruptable<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the state of the goal structure.
        /// </summary>
        /// <remarks>
        /// By default, the state is set to <see cref="GoalStructureState.Unfinished" />.
        /// However, this can be changed by the goal structure itself.
        /// </remarks>
        GoalStructureState State { get; }

        /// <inheritdoc />
        event EventHandler<InterruptableEventArgs<TBeliefSet>> OnInterrupt;

        /// <inheritdoc />
        event EventHandler<InterruptableEventArgs<TBeliefSet>> OnReinstate;

        /// <inheritdoc />
        void Interrupt(TBeliefSet beliefSet);

        /// <inheritdoc />
        void Reinstate(TBeliefSet beliefSet);

        /// <summary>
        /// Updates the state of the goal structure.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        void UpdateState(TBeliefSet beliefSet);

        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        IGoal? GetCurrentGoal(TBeliefSet beliefSet);
    }
}
