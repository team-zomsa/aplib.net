using Aplib.Core.Belief;
using Aplib.Core.DataStructures;
using Aplib.Core.Desire.Goals;

namespace Aplib.Core.Desire
{
    /// <inheritdoc />
    public class DesireSet<TBeliefSet> : IDesireSet<TBeliefSet>
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Stores the main goal structure of the agent.
        /// </summary>
        private readonly IGoalStructure<TBeliefSet> _mainGoal;

        /// <summary>
        /// Stores the side goal structures of the agent.
        /// Each of these goal structures has an accompanying guard that must be fulfilled to activate the goal structure
        /// (i.e., push the goal structure on top of the goal structure stack).
        /// All active goal structures of the agent that still need to be finished are pushed on the stack.
        /// </summary>
        private OptimizedActivationStack<(IGoalStructure<TBeliefSet>, System.Func<TBeliefSet, bool>)> _goalStructureStack { get; }

        /// <summary>
        /// If there are no goal structures left to be completed, the status of this desire set is set to the main goal status.
        /// </summary>
        public CompletionStatus Status
            => _goalStructureStack.Count == 0 ? _mainGoal.Status : CompletionStatus.Unfinished;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesireSet{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="mainGoal">The main goal structure that the agent needs to complete.</param>
        /// <param name="sideGoals">The side goal structures that could be activated during the agent playthrough.</param>
        public DesireSet(
            IGoalStructure<TBeliefSet> mainGoal,
            params (IGoalStructure<TBeliefSet> goalStructure, System.Func<TBeliefSet, bool> guard)[] sideGoals
        )
        {
            _mainGoal = mainGoal;
            _goalStructureStack = new(sideGoals);

            // Push the main goal structure on the stack.
            _goalStructureStack.Push(new((_mainGoal, _ => false), _goalStructureStack));
        }

        /// <summary>
        /// Pushes side goal structures on the stack if their guard is fulfilled.
        /// </summary>
        /// <param name="beliefSet">The belief set to check the guards of the goal structures with.</param>
        private void ActivateRelevantGoalStructures(TBeliefSet beliefSet)
        {
            foreach (var goalStructureStackItem in _goalStructureStack.ActivatableStackItems)
            {
                (_, System.Func<TBeliefSet, bool> guard) = goalStructureStackItem.Item;

                if (!guard(beliefSet)) continue;

                _goalStructureStack.Push(goalStructureStackItem);
            }
        }

        /// <inheritdoc />
        public IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
        {
            (IGoalStructure<TBeliefSet> currentGoalStructure, _) = _goalStructureStack.Peek();

            return currentGoalStructure.GetCurrentGoal(beliefSet);
        }

        /// <inheritdoc />
        public void UpdateStatus(TBeliefSet beliefSet)
        {
            ActivateRelevantGoalStructures(beliefSet);

            // Each loop, either the size of the stack is decremented or the loop is exited early.
            while (_goalStructureStack.Count > 0)
            {
                (IGoalStructure<TBeliefSet> currentGoalStructure, _) = _goalStructureStack.Peek();

                currentGoalStructure.UpdateStatus(beliefSet);

                // Early exit when an unfinished goal structure is found on the stack,
                // because we are not interested in updating the status of goal structures that are not relevant right now.
                if (currentGoalStructure.Status == CompletionStatus.Unfinished) return;

                // If the current goal structure is finished, pop it from the stack.
                _goalStructureStack.Pop();
            }
        }
    }
}
