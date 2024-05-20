namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// A belief represents/encapsulates an observation (i.e., piece of information of the game state as perceived by an agent).
    /// </summary>
    public interface IBelief
    {
        /// <summary>
        /// Updates the belief based on information of the game state.
        /// </summary>
        void UpdateBelief();
    }
}
