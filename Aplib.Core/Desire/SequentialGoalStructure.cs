using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Desire
{
    public class SequentialGoalStructure : GoalStructure, IDisposable
    {
        protected readonly IEnumerator<GoalStructure> _childrenEnumerator; // TODO autoproperty?


        /// <inheritdoc />
        public override Goal? DetermineCurrentGoal(IBeliefSet beliefSet) => GetNextGoal(beliefSet);

        /// <summary>
        /// Returns the goal from the first child which returns a goal.
        /// </summary>
        /// <param name="beliefSet"></param>
        /// <returns>TODO</returns>
        protected Goal? GetNextGoal(IBeliefSet beliefSet)
        {
            // throw new NotImplementedException("Why move to the next one immediatly?");
            // if (!_childrenEnumerator.MoveNext())
            //     return null;
            // return _childrenEnumerator.Current!.DetermineCurrentGoal(beliefSet) ?? GetNextGoal(beliefSet);

            Goal? currentGoal = _childrenEnumerator.Current!.DetermineCurrentGoal(beliefSet); // TODO what happens once Current is undefined?
            if (currentGoal is not null) return currentGoal;
            // TODO what if the previous goal becomes uncompleted due to e.g. enemies?

            return _childrenEnumerator.MoveNext() ? DetermineCurrentGoal(beliefSet) : null;
        }

        /// <inheritdoc />
        protected override void ProcessInterrupt() => _childrenEnumerator.Reset();

        /// <inheritdoc />
        public SequentialGoalStructure(IList<GoalStructure> children) : base(children)
        {
            if (!children.Any())
                throw new ArgumentException("Collection of children is empty", nameof(children));
            _childrenEnumerator = _children.GetEnumerator();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _childrenEnumerator.Dispose();

            // Tell the Garbage Collector that it does not need to run the finalizer of this class anymore
            // https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1816#rule-description
            GC.SuppressFinalize(this);
        }
    }
}
