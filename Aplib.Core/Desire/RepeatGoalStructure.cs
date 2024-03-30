using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
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
        /// Initializes a new instance of the <see cref="RepeatGoalStructure" /> class.
        /// </summary>
        /// <param name="goalStructure">The goalstructure to repeat</param>
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure) : base(
            new List<IGoalStructure<TBeliefSet>> { goalStructure }) =>
            _currentGoalStructure = goalStructure;

        /// <inheritdoc />
        public override IGoal? GetCurrentGoal(TBeliefSet beliefSet) => _currentGoalStructure!.State switch
        {
            Unfinished or Failure => _currentGoalStructure.GetCurrentGoal(beliefSet),
            _ => FinishRepeat(beliefSet)
        };

        /// <inheritdoc />
        public override void UpdateState(TBeliefSet beliefSet)
        {
            _currentGoalStructure!.UpdateState(beliefSet);

            if (_currentGoalStructure.State == Failure) _currentGoalStructure.Reinstate(beliefSet);

            State = _currentGoalStructure.State switch
            {
                Failure or Unfinished => Unfinished,
                _ => Success
            };
        }

        private IGoal? FinishRepeat(TBeliefSet beliefSet)
        {
            State = Success;
            return _currentGoalStructure!.GetCurrentGoal(beliefSet);
        }
    }
}
