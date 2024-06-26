// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

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
