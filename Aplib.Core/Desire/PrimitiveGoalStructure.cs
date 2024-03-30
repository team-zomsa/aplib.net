using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using static Aplib.Core.Desire.Goals.GoalState;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Represents a goal structure that will complete if any of its children complete.
    /// </summary>
    /// <remarks>
    /// This is the most primitive goal structure. It is used to represent a single goal that is not part of a larger structure.
    /// This goal structure will only return the goal it was created with if the goal is not yet finished.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The beliefset of the agent.</typeparam>
    public class PrimitiveGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        private readonly Goal _goal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveGoalStructure"/> class.
        /// </summary>
        /// <param name="goal">The goal to fulfill.</param>
        public PrimitiveGoalStructure(Goal goal) : base(children: System.Array.Empty<GoalStructure<TBeliefSet>>())
        {
            _goal = goal;
        }

        /// <inheritdoc />
        public override Goal? GetCurrentGoal(TBeliefSet beliefSet) => _goal!.GetState(beliefSet) switch
        {
            Unfinished => _goal,
            _ => null
        };
    }
}
