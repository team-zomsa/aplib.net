namespace Aplib.Core.Desire
{
    /// <summary>
    /// Tactics are the real meat of <see cref="Goal"/>s, as they define how the agent can approach the goal in hopes
    /// of finding a solution which meets the Goal's predicate. A tactic represents a smart combination of
    /// <see cref="Action"/>s, which are executed in a Believe Desire Intent Cycle.
    /// </summary>
    /// <seealso cref="Goal"/>
    /// <seealso cref="Action"/>
    public abstract class Tactic
    {
        /// <summary>
        /// Execute the next cycle in the Believe Desire Intent Cycle.
        /// </summary>
        public abstract void IterateBdiCycle();
    }
}