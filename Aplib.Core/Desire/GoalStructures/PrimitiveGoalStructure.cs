using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Represents a goal structure that will complete if any of its children complete.
    /// </summary>
    /// <remarks>
    /// This is the most primitive goal structure. It is used to represent a single goal that is not part of a larger
    /// structure.
    /// This goal structure will only return the goal it was created with if the goal is not yet finished.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The beliefset of the agent.</typeparam>
    public class PrimitiveGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        private readonly IGoal<TBeliefSet> _goal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this GoalStructure, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="goal">The goal to fulfill.</param>
        public PrimitiveGoalStructure(IMetadata metadata, IGoal<TBeliefSet> goal)
            : base(metadata, System.Array.Empty<IGoalStructure<TBeliefSet>>()) => _goal = goal;

        /// <inheritdoc cref="PrimitiveGoalStructure{TBeliefSet}(IMetadata,IGoal{TBeliefSet})"/>
        public PrimitiveGoalStructure(IGoal<TBeliefSet> goal) : this(new Metadata(), goal) { }

        /// <inheritdoc />
        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet) => _goal;

        /// <summary>
        /// This method updates the status of the <see cref="FirstOfGoalStructure{TBeliefSet}" />.
        /// The goal structure status is set to the status of the underlying <see cref="IGoal{TBeliefSet}"/>.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            _goal.UpdateStatus(beliefSet);
            Status = _goal.Status;
        }
    }
}
