using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;

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
        private readonly int? _maxRetries;

        private int _retryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="goalStructure">The GoalStructure to repeat.</param>
        public RepeatGoalStructure(IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure, int maxRetries)
            : base(metadata, new[] { goalStructure })
        {
            if (maxRetries < 0)
                throw new System.ArgumentException
                    ($"{nameof(maxRetries)} must be greater than or equal to zero.", nameof(maxRetries));

            _currentGoalStructure = goalStructure;
            _maxRetries = maxRetries;
            _retryCount = 0;
        }

        /// <inheritdoc />
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure, int maxRetries)
            : this(new Metadata(), goalStructure, maxRetries)
        {
        }

        /// <inheritdoc
        ///     cref="GoalStructure{TBeliefSet}(IMetadata,System.Collections.Generic.IEnumerable{IGoalStructure{TBeliefSet}})"/>
        public RepeatGoalStructure(IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure)
            : base(metadata, new[] { goalStructure })
        {
            _currentGoalStructure = goalStructure;
            _maxRetries = null;
            _retryCount = 0;
        }

        /// <inheritdoc
        ///     cref="GoalStructure{TBeliefSet}(Aplib.Core.IMetadata,System.Collections.Generic.IEnumerable{Aplib.Core.Desire.GoalStructures.IGoalStructure{TBeliefSet}})"/>
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure) : this(new Metadata(), goalStructure) { }

        /// <inheritdoc />
        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet) =>
            _currentGoalStructure!.GetCurrentGoal(beliefSet);

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
    }
}
