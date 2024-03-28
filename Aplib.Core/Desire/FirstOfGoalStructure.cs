using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System.Collections.Generic;

namespace Aplib.Core.Desire
{
    public class FirstOfGoalStructure : SequentialGoalStructure
    {
        protected Goal? _currentGoal;

        /// <inheritdoc />
        public override Goal? DetermineCurrentGoal(IBeliefSet beliefSet)
        {
            _currentGoal ??= GetNextGoal(beliefSet); // Should only happen the first time this method is called
            if (_currentGoal is null) return null; // Can be the case when no next goal can be found

            switch (_currentGoal!.GetState(beliefSet))
            {
                case GoalState.Success:
                    return null;
                case GoalState.Failure:
                    _currentGoal = GetNextGoal(beliefSet);
                    return DetermineCurrentGoal(beliefSet);

                default:
                    return _currentGoal;
            }
        }

        /// <inheritdoc />
        public FirstOfGoalStructure(IList<GoalStructure> children) : base(children)
        { }
    }
}
