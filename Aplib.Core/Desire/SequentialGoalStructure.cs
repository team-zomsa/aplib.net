using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Desire
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
        private IEnumerator<IGoalStructure<TBeliefSet>> _childrenEnumerator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="children">The children of the goal structure.</param>
        public SequentialGoalStructure(IList<IGoalStructure<TBeliefSet>> children) : base(children)
        {
            if (children.Count <= 0)
                throw new ArgumentException("Collection of children is empty", nameof(children));
            _childrenEnumerator = _children.GetEnumerator();
            _childrenEnumerator.MoveNext();
            _currentGoalStructure = _childrenEnumerator.Current;

            OnReinstate += CheckForGoalCompletion;
        }

        /// <inheritdoc />
        public override IGoal? GetCurrentGoal(TBeliefSet beliefSet) => _currentGoalStructure!.GetCurrentGoal(beliefSet);

        /// <inheritdoc />
        public override void UpdateState(TBeliefSet beliefSet)
        {
            // Loop through all the children until one of them is unfinished or successful.
            // This loop is here to prevent tail recursion.
            while (true)
            {
                if (State == GoalStructureState.Success) return;
                _currentGoalStructure!.UpdateState(beliefSet);

                switch (_currentGoalStructure.State)
                {
                    case GoalStructureState.Unfinished:
                        return;
                    case GoalStructureState.Failure:
                        State = GoalStructureState.Failure;
                        return;
                    case GoalStructureState.Success:
                    default:
                        break;
                }

                if (_childrenEnumerator.MoveNext())
                {
                    _currentGoalStructure = _childrenEnumerator.Current;
                    State = GoalStructureState.Unfinished;

                    // Update the state of the new goal structure
                    continue;
                }

                State = GoalStructureState.Success;
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

        private void CheckForGoalCompletion(object sender, InterruptableEventArgs<TBeliefSet> e)
        {
            // Check if the previous goals are still completed
            IEnumerator<IGoalStructure<TBeliefSet>> enumerator = _children.GetEnumerator();
            while (enumerator.Current != _childrenEnumerator.Current)
            {
                enumerator.MoveNext();
                enumerator.Current!.UpdateState(e.BeliefSet);
                if (enumerator.Current!.State == GoalStructureState.Success) continue;

                State = GoalStructureState.Unfinished;

                // If the goal is not completed, retry the goal and reset the enumerator.
                _childrenEnumerator = enumerator;
                _currentGoalStructure = _childrenEnumerator.Current;
                return;
            }
        }
    }
}
