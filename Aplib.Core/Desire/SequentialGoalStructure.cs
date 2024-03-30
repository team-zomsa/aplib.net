using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private IEnumerator<GoalStructure<TBeliefSet>> _childrenEnumerator { get; set; }

        /// <inheritdoc />
        public override Goal? GetCurrentGoal(TBeliefSet beliefSet)
        {
            // Check if the current goal is completed
            if (IsCurrentGoalCompleted()) return null;

            // Check if the previous goals are still completed
            IEnumerator<GoalStructure<TBeliefSet>> enumerator = _children.GetEnumerator();
            while (enumerator.Current != _childrenEnumerator.Current)
            {
                if (enumerator.Current!.State == GoalStructureState.Success) continue;

                State = GoalStructureState.Unfinished;

                // If the goal is not completed, retry the goal and reset the enumerator.
                _childrenEnumerator = enumerator;
                return enumerator.Current.GetCurrentGoal(beliefSet);
            }

            Goal? currentGoal =
                _childrenEnumerator.Current!.GetCurrentGoal(beliefSet); // TODO what happens once Current is undefined?
            if (currentGoal is not null) return currentGoal;

            // If the goal is completed, check if all the goals are completed
            if (!_childrenEnumerator.MoveNext() && _childrenEnumerator.Current.State == GoalStructureState.Success)
            {
                State = GoalStructureState.Success;
                return null!;
            }

            // We have tried all the goals, but the last one is not completed. Or something else went wrong.
            State = GoalStructureState.Failure;
            return null!;
        }

        private bool IsCurrentGoalCompleted()
        {
            switch (_currentGoalStructure!.State)
            {
                case GoalStructureState.Success:
                    _childrenEnumerator.MoveNext();
                    _currentGoalStructure = _childrenEnumerator.Current;
                    break;
                case GoalStructureState.Failure:
                    State = GoalStructureState.Failure;
                    return true;
                case GoalStructureState.Unfinished:
                default:
                    break;
            }

            return false;
        }

        /// <inheritdoc />
        protected override void ProcessInterrupt() => _childrenEnumerator.Reset();

        /// <inheritdoc />
        public SequentialGoalStructure(IList<GoalStructure<TBeliefSet>> children) : base(children)
        {
            if (children.Count <= 0)
                throw new ArgumentException("Collection of children is empty", nameof(children));
            _childrenEnumerator = _children.GetEnumerator();
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
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _childrenEnumerator.Dispose();
        }
    }
}
