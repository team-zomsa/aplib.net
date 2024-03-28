using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using static Aplib.Core.Desire.Goals.GoalState;

namespace Aplib.Core.Desire
{
    public class PrimitiveGoalStructure : GoalStructure
    {
        /// <inheritdoc />
        public PrimitiveGoalStructure(Goal goal) : base(children: System.Array.Empty<GoalStructure>())
        {
            // We use _currentGoal as 'the' goal. It will never change hereafter.
            _currentGoal = goal;
        }

        public override Goal? DetermineCurrentGoal(IBeliefSet beliefSet) => _currentGoal!.GetState(beliefSet) switch
        {
            Unfinished => _currentGoal,
            _          => null
        };

        /// <inheritdoc />
        protected override void ProcessInterrupt() { /* Do nothing */ }
    }
}
