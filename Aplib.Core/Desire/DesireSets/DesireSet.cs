using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Collections;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using System.Linq;

namespace Aplib.Core.Desire.DesireSets
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
        /// Each of these goal structures has an accompanying guard that must be fulfilled to
        /// activate the goal structure (i.e., push the goal structure on top of the goal structure stack).
        /// All active goal structures of the agent that still need to be finished are pushed on the stack.
        /// </summary>
        private readonly OptimizedActivationStack
            <(IGoalStructure<TBeliefSet> goalStructure, System.Func<TBeliefSet, bool> guard)> _goalStructureStack;

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
            _goalStructureStack.Activate(new((_mainGoal, _ => false), _goalStructureStack));
        }

        /// <summary>
        /// Pushes side goal structures on the stack if their guard is fulfilled.
        /// </summary>
        /// <param name="beliefSet">The belief set to check the guards of the goal structures with.</param>
        private void ActivateRelevantGoalStructures(TBeliefSet beliefSet)
        {
            // Filter all the goal structures by their guards.
            var itemsToActivate = _goalStructureStack.ActivatableStackItems
                .Where(item => item.Data.guard(beliefSet));

            // (Re)activate the filtered goal structures.
            foreach (var item in itemsToActivate) _goalStructureStack.Activate(item);
        }

        /// <inheritdoc />
        public IGoal<TBeliefSet> GetCurrentGoal(TBeliefSet beliefSet)
        {
            IGoalStructure<TBeliefSet> currentGoalStructure = _goalStructureStack.Peek().goalStructure;

            return currentGoalStructure.GetCurrentGoal(beliefSet);
        }

        /// <summary>
        /// Activates side goal structures when their guard is satisfied, and updates the activation stack
        /// by popping goal structures from the top of the stack when they are finished.
        /// </summary>
        /// <param name="beliefSet">The belief set of the agent.</param>
        public void Update(TBeliefSet beliefSet)
        {
            ActivateRelevantGoalStructures(beliefSet);

            // Each loop, either the size of the stack is decremented or the loop is exited early.
            while (_goalStructureStack.Count > 0)
            {
                IGoalStructure<TBeliefSet> currentGoalStructure = _goalStructureStack.Peek().goalStructure;

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
