using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core.Agents
{
    /// <summary>
    /// Represents an agent that performs actions based on goals and beliefs.
    /// </summary>
    public class BdiAgent<TBeliefSet> : IAgent
        where TBeliefSet : IBeliefSet
    {
        /// <summary>
        /// Gets the beliefset of the agent.
        /// </summary>
        private readonly TBeliefSet _beliefSet;

        /// <summary>
        /// Gets the desire of the agent.
        /// </summary>
        /// <remarks>
        /// The desire contains all goal structures and the current goal.
        /// </remarks>
        private readonly IDesireSet<TBeliefSet> _desireSet;

        /// <inheritdoc />
        public CompletionStatus Status => _desireSet.Status;

        /// <summary>
        /// Initializes a new instance of the <see cref="BdiAgent{TBeliefSet}" /> class.
        /// </summary>
        /// <param name="beliefSet">The beliefset of the agent.</param>
        /// <param name="desireSet"></param>
        public BdiAgent(TBeliefSet beliefSet, IDesireSet<TBeliefSet> desireSet)
        {
            _beliefSet = beliefSet;
            _desireSet = desireSet;
        }

        /// <summary>
        /// Performs a single BDI cycle, in which the agent updates its beliefs, selects a concrete goal,
        /// chooses a concrete action to achieve the selected goal, and executes the chosen action.
        /// </summary>
        /// <remarks>This method will get called every frame of the game.</remarks>
        public void Update()
        {
            // Belief
            _beliefSet.UpdateBeliefs();

            // Desire
            _desireSet.Update(_beliefSet);
            if (Status != CompletionStatus.Unfinished) return;
            IGoal<TBeliefSet> goal = _desireSet.GetCurrentGoal(_beliefSet);

            // Intent
            ITactic<TBeliefSet> tactic = goal.Tactic;
            IAction<TBeliefSet>? action = tactic.GetAction(_beliefSet);

            // Execute the action
            action?.Execute(_beliefSet);
        }
    }
}
