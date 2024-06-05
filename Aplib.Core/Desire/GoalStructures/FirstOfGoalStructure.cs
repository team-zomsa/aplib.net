using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Represents a goal structure that will complete if any of its children complete.
    /// </summary>
    /// <remarks>
    /// The children of this goal structure will be executed in the order they are given.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The beliefset of the agent.</typeparam>
    public class FirstOfGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>, IDisposable
        where TBeliefSet : IBeliefSet
    {
        private readonly IEnumerator<IGoalStructure<TBeliefSet>> _childrenEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this GoalStructure, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="children">The children of the goal structure.</param>
        public FirstOfGoalStructure(IMetadata metadata, params IGoalStructure<TBeliefSet>[] children) : base(metadata, children)
        {
            _childrenEnumerator = _children.GetEnumerator();
            _childrenEnumerator.MoveNext();
            _currentGoalStructure = _childrenEnumerator.Current;
        }

        /// <inheritdoc cref="FirstOfGoalStructure{TBeliefSet}(IMetadata,IGoalStructure{TBeliefSet}[])"/>
        public FirstOfGoalStructure(params IGoalStructure<TBeliefSet>[] children) : this(new Metadata(), children) { }

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
                    case CompletionStatus.Success:
                        Status = CompletionStatus.Success;
                        return;
                }

                if (_childrenEnumerator.MoveNext())
                {
                    _currentGoalStructure = _childrenEnumerator.Current;
                    Status = CompletionStatus.Unfinished;

                    // Update the Status of the new goal structure
                    continue;
                }

                Status = CompletionStatus.Failure;
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
        /// Disposes of the goal structure.
        /// </summary>
        /// <param name="disposing">Whether we are actually disposing.</param>
        protected virtual void Dispose(bool disposing) => _childrenEnumerator.Dispose();
    }
}
