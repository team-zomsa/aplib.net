namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// A belief represents/encapsulates an observation,
    /// i.e., information about the game state as perceived by an agent.
    /// </summary>
    public interface IBelief
    {
        /// <summary>
        /// Updates the belief based on information about the game state.
        /// </summary>
        void UpdateBelief();
    }
}
