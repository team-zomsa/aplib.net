namespace Aplib.Core
{
    /// <summary>
    /// Defines an agent that can play a game.
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// Gets the status of the agent.
        /// </summary>
        /// <remarks>
        /// This reflects whether the agent has achieved or failed its goals.
        /// </remarks>
        public CompletionStatus Status { get; }

        /// <summary>
        /// Updates the agent's state and goals.
        /// </summary>
        /// <remarks>This method will get called every frame of the game.</remarks>
        public void Update();
    }
}
