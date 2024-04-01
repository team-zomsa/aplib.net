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
    public class BdiAgent<TBeliefSet> : IAgent
        where TBeliefSet : IBeliefSet
    {
        /// <inheritdoc />
        public CompletionStatus Status => _desire.Status;

        /// <summary>
        /// Gets the desire of the agent.
        /// </summary>
        /// <remarks>
        /// The desire contains all goal structures and the current goal.
        /// </remarks>
        private IDesireSet<TBeliefSet> _desire { get; }

        /// <summary>
        /// Gets the beliefset of the agent.
        /// </summary>
        private TBeliefSet _state { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent" /> class.
        /// </summary>
        /// <param name="state">The beliefset of the agent.</param>
        /// <param name="desire"></param>
        public BdiAgent(TBeliefSet state, IDesireSet<TBeliefSet> desire)
        {
            _state = state;
            _desire = desire;
        }

        /// <inheritdoc />
        public void Update()
        {
            // If the agent has already finished, do nothing
            if (Status != CompletionStatus.Unfinished)
                return;

            _state.UpdateBeliefs();

            _desire.UpdateStatus(_state);
            IGoal goal = _desire.GetCurrentGoal(_state);
            Tactic tactic = goal.Tactic;
            Action action = tactic.GetAction()!;

            // Execute the action
            action.Execute();
        }
    }
}
