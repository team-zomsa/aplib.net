using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Desire
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
        private IEnumerator<IGoalStructure<TBeliefSet>> _childrenEnumerator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstOfGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="children">The children of the goal structure.</param>
        public FirstOfGoalStructure(IList<IGoalStructure<TBeliefSet>> children) : base(children)
        {
            _childrenEnumerator = children.GetEnumerator();
            _childrenEnumerator.MoveNext();
            _currentGoalStructure = _childrenEnumerator.Current;
        }

        /// <inheritdoc />
        public override IGoal GetCurrentGoal(TBeliefSet beliefSet) => _currentGoalStructure!.GetCurrentGoal(beliefSet);

        /// <inheritdoc />
        public override void UpdateState(TBeliefSet beliefSet)
        {
            // Loop through all the children until one of them is unfinished or successful.
            // This loop is here to prevent tail recursion.
            while (true)
            {
                if (Status == CompletionStatus.Success) return;
                _currentGoalStructure!.UpdateState(beliefSet);

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
