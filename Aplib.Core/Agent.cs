using Aplib.Core.Desire;
using Aplib.Core.Stubs;

namespace Aplib.Core
{
    /// <summary>
    /// Represents an agent that performs actions based on goals and beliefs.
    /// </summary>
    public class Agent : IAgent
    {
        /// <summary>
        /// Gets the beliefset of the agent.
        /// </summary>
        public Beliefset State { get; }

        /// <summary>
        /// Gets the goal structure of the agent.
        /// </summary>
        public GoalStructure Goals { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="state">The beliefset of the agent.</param>
        /// <param name="goals">The goal structure of the agent.</param>
        public Agent(Beliefset state, GoalStructure goals)
        {
            State = state;
            Goals = goals;
        }

        /// <inheritdoc/>
        public void Update()
        {
            // Update the beliefset
            State.UpdateBeliefs();

            Goal goal = Goals.GetCurrentGoal();
            Tactic tactic = goal.Tactic;
            Action action = tactic.GetAction();

            // Execute the action
            action.Execute();
        }
    }
}
