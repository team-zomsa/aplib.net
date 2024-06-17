using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using System.Collections.Generic;

namespace Aplib.Core.Desire.GoalStructures
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
        /// Initializes a new instance of the <see cref="RepeatGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="goalStructure">The GoalStructure to repeat.</param>
        public RepeatGoalStructure(IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure)
            : base(metadata, new List<IGoalStructure<TBeliefSet>> { goalStructure })
            => _currentGoalStructure = goalStructure;

        /// <inheritdoc cref="RepeatGoalStructure{TBeliefSet}(IMetadata,IGoalStructure{TBeliefSet})"/>
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure) : this(new Metadata(), goalStructure) { }

        /// <inheritdoc />
        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet) => _currentGoalStructure!.Status switch
        {
            CompletionStatus.Unfinished or CompletionStatus.Failure => _currentGoalStructure.GetCurrentGoal(beliefSet),
            CompletionStatus.Success => FinishRepeat(beliefSet),
            _ => throw new System.InvalidOperationException
                ($"An unknown variant of the {nameof(CompletionStatus)} enum was encountered.")
        };

        /// <inheritdoc />
        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            _currentGoalStructure!.UpdateStatus(beliefSet);

            Status = _currentGoalStructure.Status switch
            {
                CompletionStatus.Failure or CompletionStatus.Unfinished => CompletionStatus.Unfinished,
                CompletionStatus.Success => CompletionStatus.Success,
                _ => throw new System.InvalidOperationException
                    ($"An unknown variant of the {nameof(CompletionStatus)} enum was encountered.")
            };
        }

        private IGoal<TBeliefSet> FinishRepeat(TBeliefSet beliefSet)
        {
            Status = CompletionStatus.Success;
            return _currentGoalStructure!.GetCurrentGoal(beliefSet);
        }
    }
}
