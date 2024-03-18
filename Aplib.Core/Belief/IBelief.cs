namespace Aplib.Core.Belief
{
    /// <summary>
    /// The <see cref="IBelief"/> interface defines a contract for classes that represent a belief.
    /// A belief is some piece of information on the game state as perceived by an agent.
    /// </summary>
    public interface IBelief
    {
        /// <summary>
        /// Updates the belief based on information of the game state.
        /// </summary>
        void UpdateBelief();
    }
}
