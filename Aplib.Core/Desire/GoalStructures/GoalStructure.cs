using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using System.Collections.Generic;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Describes a structure of goals that need to be fulfilled.
    /// </summary>
    public abstract class GoalStructure<TBeliefSet> : IGoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <inheritdoc />
        public CompletionStatus Status { get; protected set; }

        /// <summary>
        /// The children of the goal structure.
        /// </summary>
        protected readonly IEnumerable<IGoalStructure<TBeliefSet>> _children;

        /// <summary>
        /// The goal structure that is currently being fulfilled.
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
        public abstract IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet);

        /// <summary>
        /// Updates the state of the goal structure.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public abstract void UpdateStatus(TBeliefSet beliefSet);
    }
}
