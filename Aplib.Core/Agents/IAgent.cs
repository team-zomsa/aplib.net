namespace Aplib.Core.Agents
{
    /// <summary>
    /// Defines a testing agent that can be updated for every frame of the game.
    /// </summary>
    public interface IAgent : ICompletable
    {
        /// <summary>
        /// Updates the agent's beliefs, desires, and intentions.
        /// </summary>
        /// <remarks>This method will get called for every frame of the game.</remarks>
        public void Update();
    }
}
