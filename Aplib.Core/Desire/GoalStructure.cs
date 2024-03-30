using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Describes a structure of goals that need to be fulfilled.
    /// </summary>
    public abstract class GoalStructure<TBeliefSet> : IGoalStructure<TBeliefSet> where TBeliefSet : IBeliefSet
    {
        /// <inheritdoc />
        public event EventHandler<InterruptableEventArgs<TBeliefSet>>? OnInterrupt;

        /// <inheritdoc />
        public event EventHandler<InterruptableEventArgs<TBeliefSet>>? OnReinstate;

        /// <summary>
        /// Gets or sets the state of the goal structure.
        /// </summary>
        /// <remarks>
        /// By default, the state is set to <see cref="GoalStructureState.Unfinished" />.
        /// However, this can be changed by the goal structure.
        /// </remarks>
        public GoalStructureState State { get; protected set; } = GoalStructureState.Unfinished;

        /// <summary>
        /// The children of the goal structure.
        /// </summary>
        protected readonly IEnumerable<IGoalStructure<TBeliefSet>> _children;

        /// <summary>
        /// The goalstructure that is currently being fulfilled.
        /// </summary>
        protected IGoalStructure<TBeliefSet>? _currentGoalStructure;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="children">The children of the goal structure.</param>
        protected GoalStructure(IEnumerable<IGoalStructure<TBeliefSet>> children) => _children = children;

        /// <summary>
        /// Gets the current goal using the given <see cref="IBeliefSet" />.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        /// <returns>The current goal to be fulfilled.</returns>
        public abstract IGoal? GetCurrentGoal(TBeliefSet beliefSet);

        /// <inheritdoc />
        public void Interrupt(TBeliefSet beliefSet)
        {
            OnInterrupt?.Invoke(this, new InterruptableEventArgs<TBeliefSet>(beliefSet));

            foreach (IGoalStructure<TBeliefSet> child in _children)
                child.Interrupt(beliefSet);
        }

        /// <inheritdoc />
        public void Reinstate(TBeliefSet beliefSet)
        {
            OnReinstate?.Invoke(this, new InterruptableEventArgs<TBeliefSet>(beliefSet));
            foreach (IGoalStructure<TBeliefSet> child in _children)
                child.Reinstate(beliefSet);
        }

        /// <summary>
        /// Updates the state of the goal structure.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public abstract void UpdateState(TBeliefSet beliefSet);
    }
}
