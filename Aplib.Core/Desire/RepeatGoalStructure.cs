using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;
using static Aplib.Core.Desire.GoalStructureState;

namespace Aplib.Core.Desire
{
    /// <summary>
    /// Represents a goal structure that will complete if any of its children complete.
    /// </summary>
    /// <remarks>
    /// This structure will repeatedly execute the goal it was created with until the goal is finished.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The beliefset of the agent.</typeparam>
    public class RepeatGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatGoalStructure"/> class.
        /// </summary>
        /// <param name="goalStructure">The goalstructure to repeat</param>
        public RepeatGoalStructure(GoalStructure<TBeliefSet> goalStructure) : base(
            children: new List<GoalStructure<TBeliefSet>> { goalStructure })
        {
            _currentGoalStructure = goalStructure;
        }

        /// <inheritdoc />
        public override Goal? GetCurrentGoal(TBeliefSet beliefSet) => _currentGoalStructure!.State switch
        {
            Unfinished or Failure => _currentGoalStructure.GetCurrentGoal(beliefSet),
            Success => FinishRepeat(),
            _ => throw new NotImplementedException()
        };

        private Goal? FinishRepeat()
        {
            State = Success;
            return null;
        }
    }
}
