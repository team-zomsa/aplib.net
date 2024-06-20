using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using static Aplib.Core.CompletionStatus;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Represents a goal structure that will complete if any of its children complete.
    /// This structure will repeatedly execute the goal it was created with until the goal is finished,
    /// or the maximum number of retries is reached.
    /// </summary>
    /// <typeparam name="TBeliefSet">The belief set of the agent.</typeparam>
    public class RepeatGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// The maximum number of times to retry the goal after it has failed.
        /// If this is <c>null</c>, the goal will be retried indefinitely.
        /// </summary>
        private readonly int? _maxRetries;

        /// <summary>
        /// The number of times the goal has been retried so far.
        /// </summary>
        // ReSharper disable once RedundantDefaultMemberInitializer
        private int _retryCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="goalStructure">The GoalStructure to repeat.</param>
        /// <param name="maxRetries">
        /// The maximum number of times to retry the goal after it has failed.
        /// If omitted, the goal will be retried indefinitely.
        /// </param>
        public RepeatGoalStructure(IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure, int maxRetries)
            : base(metadata, new[] { goalStructure })
        {
            if (maxRetries < 0)
                throw new System.ArgumentException
                    ($"{nameof(maxRetries)} must be greater than or equal to zero.", nameof(maxRetries));

            _currentGoalStructure = goalStructure;
            _maxRetries = maxRetries;
        }

        /// <inheritdoc />
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure, int maxRetries)
            : this(new Metadata(), goalStructure, maxRetries)
        {
        }

        /// <inheritdoc
        ///     cref="GoalStructure{TBeliefSet}(IMetadata,System.Collections.Generic.IEnumerable{IGoalStructure{TBeliefSet}})" />
        public RepeatGoalStructure(IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure)
            : base(metadata, new[] { goalStructure })
        {
            _currentGoalStructure = goalStructure;
            _maxRetries = null;
        }

        /// <inheritdoc />
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure) : this(new Metadata(), goalStructure) { }

        /// <inheritdoc />
        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
            => _currentGoalStructure!.GetCurrentGoal(beliefSet);


        /// <summary>
        /// Updates the status of the <see cref="RepeatGoalStructure{TBeliefSet}" />.
        /// The goal structure status is set to:
        /// <list type="bullet">
        ///     <item>
        ///         <term><see cref="Success"/></term>
        ///         <description>When the underlying goal structure is successful.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Failure"/></term>
        ///         <description>
        ///             When the underlying goal structure fails and the maximum number of retries has been reached.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Unfinished"/></term>
        ///         <description>
        ///             When the underlying goal structure is unfinished.
        ///             The underlying goal structure will be retried when it fails.
        ///         </description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            if (Status != Unfinished) return;

            _currentGoalStructure!.UpdateStatus(beliefSet);

            if (_currentGoalStructure.Status == Failure && (_maxRetries is null || _retryCount < _maxRetries))
            {
                _currentGoalStructure.Reset();
                _retryCount++;
            }

            Status = _currentGoalStructure.Status;
        }
    }
}
