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
