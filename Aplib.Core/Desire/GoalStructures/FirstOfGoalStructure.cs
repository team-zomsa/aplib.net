using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Logging;
using System.Collections.Generic;
using System.Linq;
using static Aplib.Core.CompletionStatus;

namespace Aplib.Core.Desire.GoalStructures
{
    /// <summary>
    /// Represents a goal structure that will complete if any one of its children completes.
    /// </summary>
    /// <remarks>
    /// The children of this goal structure will be executed in the order they are given.
    /// </remarks>
    /// <typeparam name="TBeliefSet">The beliefset of the agent.</typeparam>
    public class FirstOfGoalStructure<TBeliefSet> : GoalStructure<TBeliefSet>, System.IDisposable
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

        /// <summary>
        /// Updates the status of the <see cref="FirstOfGoalStructure{TBeliefSet}" />.
        /// The goal structure status is set to:
        /// <list type="bullet">
        ///     <item>
        ///         <term><see cref="Success"/></term>
        ///         <description>When any one of its children is successful.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Failure"/></term>
        ///         <description>When all children fail.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Unfinished"/></term>
        ///         <description>Otherwise.</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            if (Status != Unfinished) return;

            // Loop through all the children until one of them is unfinished or successful,
            // or the end of the enumerator is reached.
            do
            {
                _currentGoalStructure = _childrenEnumerator.Current;
                _currentGoalStructure!.UpdateStatus(beliefSet);
            }
            while (_currentGoalStructure.Status == Failure && _childrenEnumerator.MoveNext());

            Status = _currentGoalStructure.Status;
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();

            _childrenEnumerator.Reset();
            _childrenEnumerator.MoveNext();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the goal structure.
        /// </summary>
        /// <param name="disposing">Whether we are actually disposing.</param>
        protected virtual void Dispose(bool disposing) => _childrenEnumerator.Dispose();

        /// <inheritdoc />
        public override IEnumerable<ILoggable> GetLogChildren() => _children.OfType<ILoggable>();
    }
}
