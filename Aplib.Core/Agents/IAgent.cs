namespace Aplib.Core.Agents
{
    /// <summary>
    /// Defines an agent that can play a game.
    /// </summary>
    public interface IAgent : ICompletable
    {
        /// <summary>
        /// Updates the agent's state and goals.
        /// </summary>
        /// <remarks>This method will get called every frame of the game.</remarks>
        public void Update();
    }
}
