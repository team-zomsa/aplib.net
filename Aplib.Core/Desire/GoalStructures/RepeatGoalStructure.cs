using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using System.Collections.Generic;
using static Aplib.Core.CompletionStatus;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatGoalStructure{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="metadata">
        /// Metadata about this goal, used to quickly display the goal in several contexts.
        /// </param>
        /// <param name="goalStructure">The GoalStructure to repeat.</param>
        public RepeatGoalStructure(IMetadata metadata, IGoalStructure<TBeliefSet> goalStructure)
            : base(metadata, new List<IGoalStructure<TBeliefSet>> { goalStructure })
            => _currentGoalStructure = goalStructure;

        /// <inheritdoc cref="RepeatGoalStructure{TBeliefSet}(IMetadata,IGoalStructure{TBeliefSet})"/>
        public RepeatGoalStructure(IGoalStructure<TBeliefSet> goalStructure) : this(new Metadata(), goalStructure) { }

        /// <inheritdoc />
        public override IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
            => _currentGoalStructure!.GetCurrentGoal(beliefSet);


        /// <summary>
        /// This method updates the status of the <see cref="RepeatGoalStructure{TBeliefSet}" />.
        /// The goal structure status is set to:
        /// <list type="table">
        /// <item>
        /// <term><see cref="Success"/></term>
        /// <term>When the underlying <see cref="IGoalStructure{TBeliefSet}"/> is successful.</term>
        /// </item>
        /// <item>
        /// <term><see cref="Failure"/></term>
        /// <term>Never.</term>
        /// </item>
        /// <item>
        /// <term><see cref="Unfinished"/></term>
        /// <term>
        /// When the underlying <see cref="IGoalStructure{TBeliefSet}"/> is unfinished.
        /// The goal structure will continue to execute the underlying goal structure until it is successful.
        /// </term>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public override void UpdateStatus(TBeliefSet beliefSet)
        {
            if (Status != Unfinished) return;

            _currentGoalStructure!.UpdateStatus(beliefSet);

            if (_currentGoalStructure.Status == Failure) _currentGoalStructure.Reset();

            Status = _currentGoalStructure.Status;
        }
    }
}
