using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using static Aplib.Core.Desire.Goals.GoalState;

namespace Aplib.Core.Desire
{
    public class RepeatGoalStructure : PrimitiveGoalStructure
    {
        /// <inheritdoc />
        public RepeatGoalStructure(Goal goal) : base(goal)
        { }

        /// <inheritdoc />
        public override Goal? DetermineCurrentGoal(IBeliefSet beliefSet) => _currentGoal.GetState(beliefSet) switch
        {
            Unfinished or Failure => _currentGoal,
            Success               => null
        };
    }
}
