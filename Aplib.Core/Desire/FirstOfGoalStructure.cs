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
        protected IEnumerator<GoalStructure<TBeliefSet>> _childrenEnumerator { get; set; }

        public FirstOfGoalStructure(IList<GoalStructure<TBeliefSet>> children) : base(children)
        {
            _childrenEnumerator = children.GetEnumerator();
            _childrenEnumerator.MoveNext();
            _currentGoalStructure = _childrenEnumerator.Current;
        }

        public override Goal? GetCurrentGoal(TBeliefSet beliefSet)
        {
            if (State == GoalStructureState.Success) return null;

            switch (_currentGoalStructure.State)
            {
                case GoalStructureState.Unfinished:
                    return _currentGoalStructure.GetCurrentGoal(beliefSet);
                case GoalStructureState.Success:
                    State = GoalStructureState.Success;
                    return null;
                case GoalStructureState.Failure:
                default:
                    break;
            }

            if (_childrenEnumerator.MoveNext())
            {
                _currentGoalStructure = _childrenEnumerator.Current!;
                return _currentGoalStructure.GetCurrentGoal(beliefSet);
            }

            State = GoalStructureState.Failure;
            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _childrenEnumerator.Dispose();
        }
    }
}
