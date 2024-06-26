// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

namespace Aplib.Core.Belief.Beliefs
{
    /// <summary>
    /// Specifies the update mode of a sampled memory belief.
    /// </summary>
    public enum UpdateMode
    {
        /// <summary>
        /// Update the observation every cycle.
        /// </summary>
        AlwaysUpdate,

        /// <summary>
        /// Update the observation whenever a memory sample is stored.
        /// </summary>
        UpdateWhenSampled
    }
}
