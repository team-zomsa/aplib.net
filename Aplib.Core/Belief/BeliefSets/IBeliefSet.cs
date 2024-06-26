// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

namespace Aplib.Core.Belief.BeliefSets
{
    /// <summary>
    /// A belief set defines the beliefs of an agent.
    /// </summary>
    public interface IBeliefSet
    {
        /// <summary>
        /// Updates all beliefs in the belief set.
        /// </summary>
        void UpdateBeliefs();
    }
}
