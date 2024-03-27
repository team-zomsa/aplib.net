using Aplib.Core.Belief;
using Aplib.Core.Desire;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;

namespace Aplib.Core
{
    /// <summary>
    /// Represents an agent that performs actions based on goals and beliefs.
    /// </summary>
    public class BdiAgent : IAgent
    {
        /// <summary>
        /// Gets the beliefset of the agent.
        /// </summary>
        private BeliefSet _state { get; }

        /// <summary>
        /// Gets the desire of the agent.
        /// </summary>
        /// <remarks>
        /// The desire contains all goal structures and the current goal.
        /// </remarks>
        private DesireSet _desire { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="state">The beliefset of the agent.</param>
        /// <param name="desire"></param>
        public Agent(BeliefSet state, DesireSet desire)
        {
            _state = state;
            _desire = desire;
        }

        /// <inheritdoc/>
        public void Update()
        {
            // Update the beliefset
            _state.UpdateBeliefs();

            Goal goal = _desire.GetCurrentGoal();
            Tactic tactic = goal.Tactic;
            Action action = tactic.GetAction();

            // Execute the action
            action.Execute();
        }
    }
}
