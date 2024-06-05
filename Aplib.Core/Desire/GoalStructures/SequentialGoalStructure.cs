using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Represents a sequential goal structure.
    /// </summary>
    /// <remarks>
    /// This class is a specific type of goal structure where goals are processed sequentially.
    /// All goals must be completed in order for the goal structure to be completed.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The type of belief set that this goal structure operates on.</typeparam>
    public class SequentialGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>, IDisposable
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets or sets the enumerator for the children of the goal structure.
        /// </summary>
        private readonly IEnumerator<IGoalStructure<TBeliefSet>> _childrenEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this GoalStructure, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="children">The children of the goal structure.</param>
        public SequentialGoalStructure(IMetadata metadata, params IGoalStructure<TBeliefSet>[] children)
            : base(metadata, children)
        {
            if (children.Length <= 0)
                throw new ArgumentException("Collection of children is empty", nameof(children));

            _childrenEnumerator = _children.GetEnumerator();
            _childrenEnumerator.MoveNext();
            _currentGoalStructure = _childrenEnumerator.Current;
        }

        /// <inheritdoc cref="SequentialGoalStructure{TBeliefSet}(IMetadata?,IGoalStructure{TBeliefSet}[])" />
        public SequentialGoalStructure(params IGoalStructure<TBeliefSet>[] children) : this(new Metadata(), children)
        {
        }

        /// <inheritdoc />
        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
            => _currentGoalStructure!.GetCurrentGoal(beliefSet);

        /// <inheritdoc />
        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            // Loop through all the children until one of them is unfinished or successful.
            // This loop is here to prevent tail recursion.
            while (true)
            {
                if (Status == CompletionStatus.Success) return;

                _currentGoalStructure!.UpdateStatus(beliefSet);

                switch (_currentGoalStructure.Status)
                {
                    case CompletionStatus.Unfinished:
                        return;
                    case CompletionStatus.Failure:
                        Status = CompletionStatus.Failure;
                        return;
                    case CompletionStatus.Success:
                    default:
                        break;
                }

                if (_childrenEnumerator.MoveNext())
                {
                    _currentGoalStructure = _childrenEnumerator.Current;
                    Status = CompletionStatus.Unfinished;

                    // Update the state of the new goal structure
                    continue;
                }

                Status = CompletionStatus.Success;
                return;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the enumerator.
        /// </summary>
        /// <param name="disposing">Whether the object is being disposed.</param>
        protected virtual void Dispose(bool disposing) => _childrenEnumerator.Dispose();
    }
}
